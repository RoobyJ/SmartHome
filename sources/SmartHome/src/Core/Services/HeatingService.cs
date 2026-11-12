using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;

namespace Core.Services;

public class HeatingService(
  ILoggerAdapter<HeatingService> logger,
  IGarageRepository garageRepository,
  IHeatTaskRepository heatTaskRepository,
  ICyclicHeatTaskRepository cyclicHeatTaskRepository,
  IOutsideTemperatureRepository outsideTemperatureRepository,
  IHeatingLogRepository heatingLogRepository,
  IGarageClient garageClient,
  IDateTimeProvider dateTimeProvider,
  HeatingState state)
  : IHeatingService
{
  private sealed record DeviceReading(float? Temperature, bool? HeaterOn);

  public async Task ExecuteAsync(CancellationToken ct)
  {
    var now = dateTimeProvider.Now;
    logger.LogInformation($"{nameof(HeatingService)} running at: {now}");

    List<GarageEntity> garages;
    ILookup<int, HeatTaskEntity> customTasksByGarage;
    ILookup<int, CyclicHeatTaskEntity> cyclicTasksByGarage;
#pragma warning disable CA1031 // Do not catch general exception types
    try
    {
      garages = (await garageRepository.GetGarages(ct)).ToList();
      customTasksByGarage = (await heatTaskRepository.GetActiveHeatTasksFromFuture(now, ct)).ToLookup(t => t.GarageId);
      cyclicTasksByGarage = (await cyclicHeatTaskRepository.GetActiveCyclicHeatTasks(ct)).ToLookup(t => t.GarageId);
    }
    catch (Exception ex) when (ex is not OperationCanceledException || !ct.IsCancellationRequested)
    {
      logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} could not load garages or tasks.");
      return;
    }

    // Controllers are independent devices, so read them all concurrently; a slow one costs one timeout, not N.
    var readings = await Task.WhenAll(garages.Select(garage => ReadDevice(garage, ct)));

    state.PruneLatches(now);
    List<OutsideTemperatureEntity> temperatures = [];

    // Each garage is handled independently: a failure in one garage (rejected command, DB error)
    // must never stop another garage from being switched ON or OFF.
    for (var i = 0; i < garages.Count; i++)
    {
      var garage = garages[i];
      var reading = readings[i];
      try
      {
        if (reading.Temperature != null)
        {
          temperatures.Add(new OutsideTemperatureEntity
          {
            Date = now, Temperature = reading.Temperature.Value, GarageId = garage.Id
          });
        }

        var desiredState = GetDesiredHeatingState(garage.Id, reading.Temperature, customTasksByGarage[garage.Id],
          cyclicTasksByGarage[garage.Id], now);
        await ApplyHeatingState(garage, desiredState, reading.HeaterOn, ct);
      }
      catch (Exception ex) when (ex is not OperationCanceledException || !ct.IsCancellationRequested)
      {
        logger.LogError(ex, $"{nameof(HeatingService)} failed for garage {garage.Name} (id: {garage.Id}).");
      }
    }

    try
    {
      if (temperatures.Count > 0) await outsideTemperatureRepository.AddTemperatures(temperatures, ct);
    }
    catch (Exception ex) when (ex is not OperationCanceledException || !ct.IsCancellationRequested)
    {
      logger.LogError(ex, $"{nameof(HeatingService)} could not save temperatures.");
    }
#pragma warning restore CA1031
  }

  #region private

  /// <summary>Reads temperature and heater status concurrently. Never throws (except on shutdown);
  /// an unreadable or implausible value is returned as null.</summary>
  private async Task<DeviceReading> ReadDevice(GarageEntity garage, CancellationToken ct)
  {
    var temperatureTask = SafeRead(garage, "temperature", () => garageClient.GetGarageTemperature(garage.Ip, ct), ct);
    var statusTask = SafeRead(garage, "heater status", () => garageClient.GetHeaterStatus(garage.Ip, ct), ct);
    var temperatureDto = await temperatureTask;
    var statusDto = await statusTask;

    float? temperature = temperatureDto?.Temperature;
    if (temperature != null && !StartHeatingTimeCalculator.IsPlausible(temperature.Value))
    {
      logger.LogWarning(
        $"Ignoring implausible temperature {temperature} for garage {garage.Name} (id: {garage.Id}).");
      temperature = null;
    }
    else if (temperature == null)
    {
      logger.LogInformation($"Could not read temperature for garage {garage.Name} (id: {garage.Id}).");
    }

    return new DeviceReading(temperature, statusDto?.HeatingStatus);
  }

  private async Task<T?> SafeRead<T>(GarageEntity garage, string what, Func<Task<T?>> read, CancellationToken ct)
    where T : class
  {
    try
    {
      return await read();
    }
#pragma warning disable CA1031
    catch (Exception ex) when (ex is not OperationCanceledException || !ct.IsCancellationRequested)
#pragma warning restore CA1031
    {
      logger.LogWarning($"Could not read {what} for garage {garage.Name} (id: {garage.Id}): {ex.Message}");
      return null;
    }
  }

  private bool GetDesiredHeatingState(int garageId, float? temperature, IEnumerable<HeatTaskEntity> customHeatTasks,
    IEnumerable<CyclicHeatTaskEntity> cyclicHeatTasks, DateTime now)
  {
    // Evaluate every occurrence (no short-circuit) so each started window gets latched.
    var active = false;
    foreach (var customHeatTask in customHeatTasks)
    {
      active |= IsOccurrenceActive(garageId, customHeatTask.Date, temperature, now);
    }

    foreach (var cyclicHeatTask in cyclicHeatTasks)
    {
      active |= IsOccurrenceActive(garageId, cyclicHeatTask.GetClosestDateTimeFromCyclicHeatTask(now), temperature,
        now);
    }

    return active;
  }

  /// <summary>
  ///   An occurrence is active from its computed start time until its end time.
  ///   Once started it is latched, so it keeps heating until the end time regardless of later
  ///   temperature changes or missing readings. The latch is keyed by the end time, so a task that is
  ///   deleted, deactivated or moved no longer matches and stops heating.
  /// </summary>
  private bool IsOccurrenceActive(int garageId, DateTime endTime, float? temperature, DateTime now)
  {
    if (endTime <= now) return false;
    if (state.IsLatched(garageId, endTime)) return true;

    var startTime = StartHeatingTimeCalculator.CalculateStartTime(endTime, temperature);
    if (startTime == null || now < startTime.Value) return false;

    state.Latch(garageId, endTime);
    return true;
  }

  /// <summary>
  ///   Compares the desired state with the heater's actual state read from the device,
  ///   so heaters switched outside this process (UI, restart, controller reboot) are reconciled.
  ///   If the actual state could not be read, the desired command is sent anyway (it is idempotent).
  /// </summary>
  private async Task ApplyHeatingState(GarageEntity garage, bool desiredOn, bool? actualOn, CancellationToken ct)
  {
    if (actualOn == desiredOn)
    {
      state.LastCommanded[garage.Id] = desiredOn;
      return;
    }

    var status = desiredOn ? "ON" : "OFF";
    await garageClient.ChangeHeaterStatus(status, garage.Ip, ct);

    var isChange = actualOn != null
                   || !state.LastCommanded.TryGetValue(garage.Id, out var lastCommanded)
                   || lastCommanded != desiredOn;
    state.LastCommanded[garage.Id] = desiredOn;
    if (!isChange) return;

    logger.LogInformation($"Set heater {status} in garage {garage.Name} (id: {garage.Id}) at {dateTimeProvider.Now}");
    await heatingLogRepository.AddHeatLog(
      new HeatLogEntity
      {
        Date = DateTime.UtcNow,
        Info = $"Set heater {status} in garage {garage.Name} (id: {garage.Id})"
      }, ct);
  }

  #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Dtos;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Mappers;
using Core.Models;
using SmartHome.Core.Models;

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
  public async Task ExecuteAsync(CancellationToken ct)
  {
    var now = dateTimeProvider.Now;
    logger.LogInformation($"{nameof(HeatingService)} running at: {now}");

    List<GarageEntity> garages;
    try
    {
      garages = (await garageRepository.GetGarages(ct)).ToList();
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} could not load garages.");
      return;
    }

    state.PruneLatches(now);
    List<OutsideTemperatureEntity> temperatures = [];

    // Each garage is handled independently: a failure in one garage (unreachable controller,
    // rejected command, DB error) must never stop another garage from being switched ON or OFF.
    foreach (var garage in garages)
    {
      try
      {
        var temperature = await ReadTemperature(garage, now, temperatures, ct);
        var desiredState = await GetDesiredHeatingState(garage, temperature, now, ct);
        await ApplyHeatingState(garage, desiredState, ct);
      }
      catch (OperationCanceledException) when (ct.IsCancellationRequested)
      {
        throw;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, $"{nameof(HeatingService)} failed for garage {garage.Name} (id: {garage.Id}).");
      }
    }

    try
    {
      if (temperatures.Count > 0) await outsideTemperatureRepository.AddTemperatures(temperatures, ct);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, $"{nameof(HeatingService)} could not save temperatures.");
    }
#pragma warning restore CA1031
  }

  #region private

  private async Task<float?> ReadTemperature(GarageEntity garage, DateTime now,
    List<OutsideTemperatureEntity> temperatures, CancellationToken ct)
  {
    var response = await garageClient.GetGarageTemperature(garage.Ip, ct);
    if (response == null)
    {
      logger.LogInformation($"Could not read temperature for garage {garage.Name} (id: {garage.Id}).");
      return null;
    }

    temperatures.Add(new OutsideTemperatureEntity { Date = now, Temperature = response.Temperature, GarageId = garage.Id });
    return response.Temperature;
  }

  private async Task<bool> GetDesiredHeatingState(GarageEntity garage, float? temperature, DateTime now,
    CancellationToken ct)
  {
    var customHeatTasks = await heatTaskRepository.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, now, ct);
    foreach (var customHeatTask in customHeatTasks)
    {
      if (IsOccurrenceActive(garage.Id, customHeatTask.Date, temperature, now)) return true;
    }

    var cyclicHeatTasks = await cyclicHeatTaskRepository.GetActiveCyclicHeatTasks(garage.Id, ct);
    foreach (var cyclicHeatTask in cyclicHeatTasks)
    {
      foreach (var day in cyclicHeatTask.CyclicHeatTaskDays)
      {
        var endTime = GetNextOccurrence(now, day.Day, cyclicHeatTask.Time);
        if (IsOccurrenceActive(garage.Id, endTime, temperature, now)) return true;
      }
    }

    return false;
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

  private static DateTime GetNextOccurrence(DateTime now, int dayOfWeek, TimeSpan time)
  {
    var currentDayOfWeek = (int)now.DayOfWeek;
    var daysUntilTarget = dayOfWeek - currentDayOfWeek;
    if (daysUntilTarget < 0) daysUntilTarget += 7;

    var candidate = now.Date.AddDays(daysUntilTarget) + time;
    if (candidate <= now) candidate = candidate.AddDays(7);

    return candidate;
  }

  /// <summary>
  ///   Compares the desired state with the heater's actual state read from the device,
  ///   so heaters switched outside this process (UI, restart, controller reboot) are reconciled.
  ///   If the actual state cannot be read, the desired command is sent anyway (it is idempotent).
  /// </summary>
  private async Task ApplyHeatingState(GarageEntity garage, bool desiredOn, CancellationToken ct)
  {
    var actual = await garageClient.GetHeaterStatus(garage.Ip, ct);
    if (actual != null && actual.HeatingStatus == desiredOn)
    {
      state.LastCommanded[garage.Id] = desiredOn;
      return;
    }

    var status = desiredOn ? "ON" : "OFF";
    await garageClient.ChangeHeaterStatus(status, garage.Ip, ct);

    var isChange = actual != null
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

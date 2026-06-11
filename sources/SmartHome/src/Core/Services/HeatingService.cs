using System;
using System.Collections.Concurrent;
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
  IDateTimeProvider dateTimeProvider)
  : IHeatingService
{
  private static readonly ConcurrentDictionary<int, bool> garageHeatingState = new();

  public async Task ExecuteAsync(CancellationToken ct)
  {
    var now = dateTimeProvider.Now;
    logger.LogInformation(
      $"{nameof(HeatingService)} running at: {now}. Currently heating garages: {garageHeatingState.Count(i => i.Value)}");
    try
    {
      var garages = (await garageRepository.GetGarages(ct)).ToList();
      var listOfGarageTemperatures = await GetListOfGarageTemperatures(garages, ct);

      foreach (var garage in garages)
      {
        var desiredState = await GetDesiredHeatingState(garage, listOfGarageTemperatures, ct);
        await ApplyHeatingState(garage, desiredState, ct);
      }
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} threw an exception.");
    }
  }

  #region private

  private async Task<bool> GetDesiredHeatingState(GarageEntity garage, List<GarageTemperatureDto> temperatures,
    CancellationToken ct)
  {
    var now = dateTimeProvider.Now;
    var temperature = temperatures.FirstOrDefault(i => i.Id == garage.Id)?.Temperature;

    var customHeatTasks = await heatTaskRepository.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, now, ct);
    foreach (var customHeatTask in customHeatTasks)
    {
      if (IsTaskActiveNow(customHeatTask.Date, temperature, now)) return true;
    }

    var cyclicHeatTasks = await cyclicHeatTaskRepository.GetActiveCyclicHeatTasks(garage.Id, ct);
    foreach (var cyclicHeatTask in cyclicHeatTasks)
    {
      foreach (var day in cyclicHeatTask.CyclicHeatTaskDays)
      {
        var endTime = GetNextOccurrence(now, day.Day, cyclicHeatTask.Time);
        if (endTime == null) continue;

        if (IsTaskActiveNow(endTime.Value, temperature, now)) return true;
      }
    }

    return false;
  }

  private static bool IsTaskActiveNow(DateTime endTime, float? temperature, DateTime now)
  {
    var startTime = StartHeatingTimeCalculator.CalculateStartTime(endTime, temperature);
    if (startTime == null) return false;

    return now >= startTime.Value && now < endTime;
  }

  private static DateTime? GetNextOccurrence(DateTime now, int dayOfWeek, TimeSpan time)
  {
    var currentDayOfWeek = (int)now.DayOfWeek;
    var daysUntilTarget = dayOfWeek - currentDayOfWeek;
    if (daysUntilTarget < 0) daysUntilTarget += 7;

    var candidate = now.Date.AddDays(daysUntilTarget) + time;
    if (candidate <= now) candidate = candidate.AddDays(7);

    return candidate;
  }

  private async Task ApplyHeatingState(GarageEntity garage, bool desiredOn, CancellationToken ct)
  {
    garageHeatingState.TryGetValue(garage.Id, out var currentlyOn);
    if (desiredOn == currentlyOn) return;

    var status = desiredOn ? "ON" : "OFF";
    await garageClient.ChangeHeaterStatus(status, garage.Ip, ct);

    garageHeatingState[garage.Id] = desiredOn;
    logger.LogInformation($"Set heater {status} in garage {garage.Name} (id: {garage.Id}) at {dateTimeProvider.Now}");
    await heatingLogRepository.AddHeatLog(
      new HeatLogEntity
      {
        Date = DateTime.UtcNow,
        Info = $"Set heater {status} in garage {garage.Name} (id: {garage.Id})"
      }, ct);
  }

  private async Task<List<GarageTemperatureDto>> GetListOfGarageTemperatures(IReadOnlyList<GarageEntity> garages,
    CancellationToken ct)
  {
    List<GarageTemperatureDto> listOfGarageTemperatureDtos = [];
    List<OutsideTemperatureEntity> temperatures = [];

    foreach (var garage in garages)
    {
      var response = await garageClient.GetGarageTemperature(garage.Ip, ct);
      if (response == null)
      {
        logger.LogInformation($"Could not read temperature for garage {garage.Name} (id: {garage.Id}).");
        continue;
      }

      listOfGarageTemperatureDtos.Add(new GarageTemperatureDto { Id = garage.Id, Temperature = response.Temperature });
      var entity =
        new OutsideTemperatureEntity { Date = dateTimeProvider.Now, Temperature = response.Temperature, GarageId = garage.Id };
      temperatures.Add(entity);
    }

    await outsideTemperatureRepository.AddTemperatures(temperatures, ct);

    return listOfGarageTemperatureDtos;
  }

  #endregion
}

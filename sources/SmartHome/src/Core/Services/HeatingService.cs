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
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
  private HashSet<HeatTask> garagesHeatTasks = [];

  public async Task ExecuteAsync(CancellationToken ct)
  {
    logger.LogInformation(
      $"{nameof(HeatingService)} running at: {DateTimeOffset.Now}. With actual heat-tasks: {garagesHeatTasks.Count}");
    try
    {
      await this.CheckGaragesHeatersStatuses(ct);
      var garages = (await garageRepository.GetGarages(ct)).ToList();

      var newClosestHeatTasks = await FindClosestHeatTime(garages, ct);

      newClosestHeatTasks.ForEach(i => garagesHeatTasks.Add(i));
      
      var listOfGarageTemperatures = await GetListOfGarageTemperatures(garages, ct);

      var listOfStartHeatTimes =
        StartHeatingTimeCalculator.CalculateForMultipleGarages(listOfGarageTemperatures,
          this.garagesHeatTasks.Where(i => !i.IsCurrentlyHeating).ToList());

      this.SetOnHeaters(listOfStartHeatTimes, garages, ct);
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} threw an exception.");
    }
  }

  #region private
  private async Task CheckGaragesHeatersStatuses(CancellationToken ct)
  {
    var garageHeatersIdsToTurnOff = new List<int>();
    foreach (var item in this.garagesHeatTasks)
    {
      if (item.EndTime > DateTime.Now) continue;

      garageHeatersIdsToTurnOff.Add(item.GarageId);
      item.IsCurrentlyHeating = false;
    }

    if (garageHeatersIdsToTurnOff.Count == 0) return;
    
    foreach (var garageId in garageHeatersIdsToTurnOff)
    {
      this.garagesHeatTasks.Remove(this.garagesHeatTasks.First(x => x.GarageId == garageId));
    }

    var ips = await garageRepository.GetGaragesIpsByIds(garageHeatersIdsToTurnOff, ct);

    foreach (var garageIp in ips)
    {
      await garageClient.ChangeHeaterStatus("OFF", garageIp, ct);
    }
  }

  private async Task<List<HeatTask>> FindClosestHeatTime(List<GarageEntity> garages, CancellationToken ct)
  {
    List<HeatTask> garagesClosestHeatTasks = [];
    foreach (var garage in garages)
    {
      HeatTask? closestHeatTask = null;
      var customHeatRequests = await heatTaskRepository.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, ct);
      var customHeatRequest = customHeatRequests.MinBy(i => i.Date);

      if (customHeatRequest == null)
      {
        logger.LogInformation($"No custom heat requests found for garage  {garage.Name} with id: {garage.Id}.");
      }
      else
      {
        closestHeatTask = customHeatRequest.ToHeatTask();
      }


      var cyclicHeatTasks = await cyclicHeatTaskRepository.GetActiveCyclicHeatTasks(garage.Id, ct);

      if (cyclicHeatTasks.Count == 0)
      {
        logger.LogInformation($"No cyclic heat requests found for garage  {garage.Name} with id: {garage.Id}.");
      }

      var closestCyclicHeatTask = this.GetClosestCyclicHeatTask(cyclicHeatTasks);

      closestHeatTask ??= closestCyclicHeatTask;

      if (closestCyclicHeatTask != null && customHeatRequest != null)
      {
        if (customHeatRequest.Date - dateTimeProvider.Now >
            closestCyclicHeatTask.EndTime - dateTimeProvider.Now)
        {
          closestHeatTask = closestCyclicHeatTask;
        }
      }
      if (closestHeatTask == null || (garagesHeatTasks.ToList().Find(i =>
            i.HeatTaskId == closestHeatTask.HeatTaskId && i.IsCyclic == closestHeatTask.IsCyclic)) != null) continue;
      garagesClosestHeatTasks.Add(closestHeatTask);
    }

    return garagesClosestHeatTasks;
  }

  private HeatTask? GetClosestCyclicHeatTask(IEnumerable<CyclicHeatTaskEntity> cyclicHeatTasks)
  {
    var currentDateTime = DateTime.Now;
    HeatTask? closestHeatTask = null;
    foreach (var item in cyclicHeatTasks)
    {
      foreach (var dayNumber in item.CyclicHeatTaskDays)
      {
        if (closestHeatTask == null)
        {
          closestHeatTask =
            item.ToHeatTask(DateTimeHelpers.GetDateTimeFromWeekDayNumberAndTime(dayNumber.Day, item.Time));
          continue;
        }

        if (DateTimeHelpers.GetDateTimeFromWeekDayNumberAndTime(dayNumber.Day, item.Time) - currentDateTime <
            closestHeatTask.EndTime - currentDateTime)
        {
          closestHeatTask =
            item.ToHeatTask(DateTimeHelpers.GetDateTimeFromWeekDayNumberAndTime(dayNumber.Day, item.Time));
          ;
        }
      }
    }

    return closestHeatTask;
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
        continue;
      }

      listOfGarageTemperatureDtos.Add(new GarageTemperatureDto { Id = garage.Id, Temperature = response.Temperature });
      var entity =
        new OutsideTemperatureEntity { Date = DateTime.Now, Temperature = response.Temperature, GarageId = garage.Id };
      temperatures.Add(entity);
    }

    await outsideTemperatureRepository.AddTemperatures(temperatures, ct);

    return listOfGarageTemperatureDtos;
  }

  private async void SetOnHeaters(List<HeatTask> startHeatTimes, List<GarageEntity> garages, CancellationToken ct)
  {
    foreach (var garageStartHeatTime in startHeatTimes)
    {
      if (!garageStartHeatTime.StartTime.HasValue) continue;

      if (!DateTime.Now.Date.Equals(garageStartHeatTime.StartTime.Value.Date) ||
          !(DateTime.Now.TimeOfDay.TotalSeconds > garageStartHeatTime.StartTime.Value.TimeOfDay.TotalSeconds))
      {
        continue;
      }

      var ip = garages.Find(garage => garage.Id == garageStartHeatTime.GarageId)?.Ip;

      if (String.IsNullOrEmpty(ip)) continue;


      var garageHeaterStatus = garagesHeatTasks.ToList().Find(i => i.GarageId == garageStartHeatTime.GarageId);
      if (garageHeaterStatus is { IsCurrentlyHeating: true }) continue;
      
      await garageClient.ChangeHeaterStatus("ON", ip, ct);

      garageHeaterStatus!.IsCurrentlyHeating = true;
      var text = garageStartHeatTime.IsCyclic ? "cyclic" : "";
      logger.LogInformation(
        $"Set heater ON in garage {garageStartHeatTime.GarageId} running at: {DateTimeOffset.Now}");
      await heatingLogRepository.AddHeatLog(
        new HeatLogEntity
        {
          Date = DateTime.UtcNow,
          Info =
            $"Starting heating in garage {garageStartHeatTime.GarageId} with {text} heat task id: {garageStartHeatTime.HeatTaskId}"
        }, ct);
    }
  }

  #endregion
}

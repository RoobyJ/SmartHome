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
  IGarageClient garageClient,
  IDateTimeProvider dateTimeProvider)
  : IHeatingService
{
  private readonly List<GarageHeaterStatus> garagesHeatersStatuses = [];

  public async Task ExecuteAsync(CancellationToken ct)
  {
    logger.LogInformation($"{nameof(HeatingService)} running at: {DateTimeOffset.Now}");
    try
    {
      await this.CheckGaragesHeatersStatuses(ct);
      var garages = (await garageRepository.GetGarages(ct)).ToList();


      var closestHeatTimes = await FindClosestHeatTime(garages, ct);


      //(closestHeatTimes, garages, ct);

      var listOfGarageTemperatures = await GetListOfGarageTemperatures(garages, ct);

      var listOfStartHeatTimes =
        StartHeatingTimeCalculator.CalculateForMultipleGarages(listOfGarageTemperatures, closestHeatTimes);

      //SetOnHeaters(listOfStartHeatTimes, garages, ct);
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
    foreach (var item in this.garagesHeatersStatuses)
    {
      if (item.EndTime <= DateTimeOffset.Now)
      {
        garageHeatersIdsToTurnOff.Add(item.GarageId);
      }
    }

    var ips = await garageRepository.GetGaragesIpsByIds(garageHeatersIdsToTurnOff, ct);

    foreach (var garageIp in ips)
    {
      await garageClient.ChangeHeaterStatus("OFF", garageIp, ct);
    }
  }

  private async Task<List<GarageHeatingTime>> FindClosestHeatTime(List<GarageEntity> garages, CancellationToken ct)
  {
    List<GarageHeatingTime> garagesClosestHeatingTimes = [];
    foreach (var garage in garages)
    {
      var customHeatRequest = heatTaskRepository.GetClosestActiveHeatTaskForGarageId(garage.Id);

      if (customHeatRequest == null)
      {
        logger.LogInformation($"No custom heat requests found for garage  {garage.Name} with id: {garage.Id}.");
      }

      var cyclicHeatTasks = await cyclicHeatTaskRepository.GetActiveCyclicHeatTasks(garage.Id, ct);

      var closestCyclicHeatTask = this.GetClosestCyclicHeatTask(cyclicHeatTasks);
      if (closestCyclicHeatTask != null && customHeatRequest != null)
      {
        if (customHeatRequest.Date - dateTimeProvider.Now <
            closestCyclicHeatTask.StartTime - dateTimeProvider.Now)
        {
          
        }
  
      }
      
      DateTime? closestDateTime = null;
      var isCyclic = false;
      var heatTaskId = 0;

      foreach (var cyclicHeatTask in cyclicHeatTasks)
      {
        DateTime? result;

        if (customHeatRequest != null)
        {
          var checkResult = HeatingServiceHelper.CheckWhichIsCloser(cyclicHeatTask, customHeatRequest);
          result = checkResult.ClosestDate;
          isCyclic = checkResult.IsCyclic;
          heatTaskId = checkResult.HeatTaskId;
        }
        else
        {
          result = cyclicHeatTask.GetClosestDateTimeFromCyclicHeatTask();
          isCyclic = true;
          heatTaskId = cyclicHeatTask.Id;
        }

        if (closestDateTime == null)
        {
          closestDateTime = result;
          continue;
        }

        if (result.HasValue && closestDateTime.Value.Second > result.Value.Second)
        {
          closestDateTime = result;
        }
      }

      if (closestDateTime != null)
      {
        garagesClosestHeatingTimes.Add(new GarageHeatingTime
        {
          Id = garage.Id, HeatTime = closestDateTime, HeatTaskId = heatTaskId, IsCyclic = isCyclic
        });
      }
    }

    return garagesClosestHeatingTimes;
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
          closestHeatTask = new HeatTask
          {
            Id = item.Id, StartTime = DateTimeHelpers.GetDateTimeFromWeekDayNumberAndTime(dayNumber.Day, item.Time)
          };
          continue;
        }

        if (DateTimeHelpers.GetDateTimeFromWeekDayNumberAndTime(dayNumber.Day, item.Time) - currentDateTime <
            closestHeatTask.StartTime - currentDateTime)
        {
          closestHeatTask = new HeatTask
          {
            Id = item.Id, StartTime = DateTimeHelpers.GetDateTimeFromWeekDayNumberAndTime(dayNumber.Day, item.Time)
          };
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

  // private async void CheckIfShouldBeOff(List<GarageHeatingTime> todayHeatTimes, List<Garage> garages,
  //   CancellationToken ct)
  // {
  //   foreach (var heatTime in todayHeatTimes)
  //   {
  //     var garage = garages.Find(i => i.Id == heatTime.Id);
  //     var garageHeaterStatus = garagesHeatersStatuses.Find(i => i.Id == heatTime.Id);
  //
  //     if (garage == null || garageHeaterStatus == null)
  //     {
  //       throw new Exception("Garage or garage heater status not found");
  //     }
  //
  //     if (!heatTime.HeatTime.HasValue && garageHeaterStatus.HeatingStatus)
  //     {
  //       await garageClient.ChangeHeaterStatus("OFF", garage.Ip, ct);
  //       garageHeaterStatus.HeatingStatus = false;
  //       logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
  //       await heatingLogRepository.AddHeatLog(
  //         new HeatLog { Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}" }, ct);
  //       garagesHeatersStatuses.Remove(garageHeaterStatus);
  //       continue;
  //     }
  //
  //     if (DateTime.Now.TimeOfDay.TotalSeconds > heatTime.HeatTime!.Value.TimeOfDay.TotalSeconds &&
  //         garageHeaterStatus.HeatingStatus)
  //     {
  //       await garageClient.ChangeHeaterStatus("OFF", garage.Ip, ct);
  //       garageHeaterStatus.HeatingStatus = false;
  //       logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
  //       await heatingLogRepository.AddHeatLog(
  //         new HeatLog { Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}" }, ct);
  //       garagesHeatersStatuses.Remove(garageHeaterStatus);
  //     }
  //   }
  // }
  //
  // private async void SetOnHeaters(List<GarageStartHeatTime> startHeatTimes, List<Garage> garages, CancellationToken ct)
  // {
  //   foreach (var garageStartHeatTime in startHeatTimes)
  //   {
  //     if (!garageStartHeatTime.StartHeatTime.HasValue)
  //     {
  //       continue;
  //     }
  //
  //     if (!DateTime.Now.Date.Equals(garageStartHeatTime.StartHeatTime.Value.Date) ||
  //         !(DateTime.Now.TimeOfDay.TotalSeconds > garageStartHeatTime.StartHeatTime.Value.TimeOfDay.TotalSeconds))
  //     {
  //       continue;
  //     }
  //
  //     var ip = garages.Find(garage => garage.Id == garageStartHeatTime.GarageId)?.Ip;
  //
  //     if (String.IsNullOrEmpty(ip))
  //     {
  //       continue;
  //     }
  //
  //     var garageHeaterStatus = garagesHeatersStatuses.Find(i => i.Id == garageStartHeatTime.GarageId);
  //     if (garageHeaterStatus is not { HeatingStatus: false })
  //     {
  //       continue;
  //     }
  //
  //     await garageClient.ChangeHeaterStatus("ON", ip, ct);
  //
  //     garageHeaterStatus!.HeatingStatus = true;
  //     var text = garageStartHeatTime.IsCyclic ? "cyclic" : "";
  //     logger.LogInformation(
  //       $"Set heater ON in garage {garageStartHeatTime.GarageId} running at: {DateTimeOffset.Now}");
  //     await heatingLogRepository.AddHeatLog(
  //       new HeatLog
  //       {
  //         Date = DateTime.UtcNow,
  //         Info =
  //           $"Starting heating in garage {garageStartHeatTime.GarageId} with {text} heat task id: {garageStartHeatTime.HeatTaskId}"
  //       }, ct);
  //   }
  // }

  #endregion
}

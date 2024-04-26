using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Helpers;
using Core.Interfaces;
using SmartHome.Core.DTOs;
using Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Models;
using SmartHome.Heater.Models;

namespace Core.Services;

public class HeatingService(
  ILoggerAdapter<HeatingService> logger,
  IGarageRepository garageRepository,
  IHeatTaskRepository heatTaskRepository,
  ICyclicHeatTaskRepository cyclicHeatTaskRepository,
  IOutsideTemperatureRepository outsideTemperatureRepository,
  IHeatingLogRepository heatingLogRepository,
  StartHeatingTimeCalculator startHeatingTimeCalculator,
  IGarageClient garageClient)
  : IHeatingService
{
  private readonly List<GarageHeaterStatus> garagesHeatersStatuses = [];

  public async Task ExecuteAsync(CancellationToken ct)
  {
    logger.LogInformation($"{nameof(HeatingService)} running at: {DateTimeOffset.Now}");
    try
    {
      var garages = (await garageRepository.GetGarages(ct)).ToList();

      await InitGarageHeatersStatuses(garages, ct);


      var closestHeatTimes = await FindClosestHeatTime(garages, ct);


      CheckIfShouldBeOff(closestHeatTimes, garages, ct);

      var listOfGarageTemperatures = await GetListOfGarageTemperatures(garages, ct);

      var listOfStartHeatTimes =
        startHeatingTimeCalculator.CalculateForMultipleGarages(listOfGarageTemperatures, closestHeatTimes);

      SetOnHeaters(listOfStartHeatTimes, garages, ct);
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} threw an exception.");
    }
  }


  #region private

  private async Task<List<GarageHeatingTime>> FindClosestHeatTime(List<Garage> garages, CancellationToken ct)
  {
    List<GarageHeatingTime> garagesClosestHeatingTimes = new();
    foreach (var garage in garages)
    {
      var customHeatRequests = await heatTaskRepository.GetHeatTasks(garage.Id, ct);

      var customHeatRequest = customHeatRequests.MinBy(item => Math.Abs((item.Date - DateTime.Now).Ticks));

      if (customHeatRequest == null) throw new Exception("Couldn't get custom heat request");

      var cyclicHeatTasks = await cyclicHeatTaskRepository.GetCyclicHeatTasks(garage.Id, ct);


      DateTime? closestDateTime = null;

      foreach (var cyclicHeatTask in cyclicHeatTasks)
      {
        var result = HeatingServiceHelper.CheckWhichIsCloser(cyclicHeatTask, customHeatRequest);
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
        garagesClosestHeatingTimes.Add(new GarageHeatingTime { Id = garage.Id, HeatTime = closestDateTime });
      }
    }

    return garagesClosestHeatingTimes;
  }

  private async Task<List<GarageTemperatureDto>> GetListOfGarageTemperatures(IReadOnlyList<Garage> garages, CancellationToken ct)
  {
    List<GarageTemperatureDto> listOfGarageTemperatureDtos = [];
    List<OutsideTemperature> temperatures = [];

    for (var i = 0; i < garages.Count; i++)
    {
      var response = await garageClient.GetGarageTemperature(garages[i].Ip, ct);
      if (response == null) continue;
      listOfGarageTemperatureDtos.Add(new GarageTemperatureDto { Id = i + 1, Temperature = response.Temperature });
      var entity = new OutsideTemperature { Date = DateTime.Now, Temperature = response.Temperature, GarageId = i + 1 };
      temperatures.Add(entity);
    }

    await outsideTemperatureRepository.AddTemperatures(temperatures, ct);

    return listOfGarageTemperatureDtos;
  }

  private async void CheckIfShouldBeOff(List<GarageHeatingTime> todayHeatTimes, List<Garage> garages,
    CancellationToken ct)
  {
    foreach (var heatTime in todayHeatTimes)
    {
      var garage = garages.Find(i => i.Id == heatTime.Id);
      var garageHeaterStatus = garagesHeatersStatuses.Find(i => i.Id == heatTime.Id);

      if (garage == null || garageHeaterStatus == null) throw new Exception("Garage or garage heater status not found");

      if (!heatTime.HeatTime.HasValue && garageHeaterStatus.HeatingStatus)
      {
        await garageClient.ChangeHeaterStatus("false", garage.Ip, ct);
        logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        await heatingLogRepository.AddHeatLog(
          new HeatLog { Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}" }, ct);
        continue;
      }

      if (DateTime.Now.TimeOfDay.TotalSeconds > heatTime.HeatTime!.Value.TimeOfDay.TotalSeconds &&
          garageHeaterStatus.HeatingStatus)
      {
        await garageClient.ChangeHeaterStatus("false", garage.Ip, ct);
        logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        await heatingLogRepository.AddHeatLog(
          new HeatLog { Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}" }, ct);
      }
    }
  }

  private async void SetOnHeaters(List<GarageStartHeatTime> startHeatTimes, List<Garage> garages, CancellationToken ct)
  {
    foreach (var garageStartHeatTime in startHeatTimes)
    {
      if (!garageStartHeatTime.StartHeatTime.HasValue)
      {
        continue;
      }

      if (!DateTime.Now.Date.Equals(garageStartHeatTime.StartHeatTime.Value.Date) ||
          !(DateTime.Now.TimeOfDay.TotalSeconds > garageStartHeatTime.StartHeatTime.Value.TimeOfDay.TotalSeconds))
      {
        continue;
      }

      var ip = garages.Find(garage => garage.Id == garageStartHeatTime.Id)?.Ip;
        
      if (String.IsNullOrEmpty(ip))
      {
        continue;
      }

      await garageClient.ChangeHeaterStatus("true", ip, ct);
      logger.LogInformation(
        $"Sett heater ON in garage {garageStartHeatTime.Id} running at: {DateTimeOffset.Now}");
      await heatingLogRepository.AddHeatLog(
        new HeatLog { Date = DateTime.UtcNow, Info = $"Starting heating garage {garageStartHeatTime.Id}" }, ct);
    }
  }

  private async Task InitGarageHeatersStatuses(List<Garage> garages, CancellationToken ct)
  {
    foreach (var garage in garages)
    {
      if (garagesHeatersStatuses.Exists(item => item.Id == garage.Id))
      {
        continue;
      }

      var response = await garageClient.GetHeaterStatus(garage.Ip, ct);

      if (response != null)
      {
        garagesHeatersStatuses.Add(new GarageHeaterStatus { Id = garage.Id, HeatingStatus = response.HeatingStatus });
      }
      else
      {
        await heatingLogRepository.AddHeatLog(new HeatLog { Date = new DateTime(), Info = "No response" }, ct);
      }
      
    }
  }

  #endregion
}

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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Helpers;
using SmartHome.Core.Models;
using SmartHome.Core.Services;
using SmartHome.Heater.Models;

namespace Core.Services;

public class HeatingService : IHeatingService
{
  private readonly List<GarageHeaterStatus> _garagesHeatersStatuses = new();
  private readonly ILoggerAdapter<HeatingService> _logger;
  private readonly IServiceLocator _serviceScopeFactoryLocator;
  private readonly StartHeatingTimeCalculator _startHeatingTimeCalculator;
  private readonly IGarageClient garageClient;

  public HeatingService(ILoggerAdapter<HeatingService> logger,
    IServiceLocator serviceScopeFactoryLocator, StartHeatingTimeCalculator startHeatingTimeCalculator,
    IGarageClient garageClient)
  {
    _logger = logger;
    _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
    _startHeatingTimeCalculator = startHeatingTimeCalculator;
    this.garageClient = garageClient;
  }

  public async Task ExecuteAsync(CancellationToken ct)
  {
    _logger.LogInformation($"{nameof(HeatingService)} running at: {DateTimeOffset.Now}");
    try
    {
      // EF Requires a scope so we are creating one per execution here
      using var scope = _serviceScopeFactoryLocator.CreateScope();
      var garageRepository =
        scope.ServiceProvider
          .GetService<IGarageRepository>();

      var garages = garageRepository.Get(new GarageQueryOptions { AsNoTracking = true }).ToList();

      await InitGarageHeatersStatuses(garages, ct);


      var closestHeatTimes = await FindClosestHeatTime(garages, ct);


      CheckIfShouldBeOff(closestHeatTimes, garages, ct);

      var listOfGarageTemperatures = await GetListOfGarageTemperatures(garages, ct);

      var listOfStartHeatTimes =
        _startHeatingTimeCalculator.CalculateForMultipleGarages(listOfGarageTemperatures, closestHeatTimes);

      SetOnHeaters(listOfStartHeatTimes, garages, ct);
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      _logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} threw an exception.");
    }
  }


  #region private

  private async Task<List<GarageHeatingTime>> FindClosestHeatTime(List<Garage> garages, CancellationToken ct)
  {
    using var scope = _serviceScopeFactoryLocator.CreateScope();
    var cyclicHeatRepository = scope.ServiceProvider.GetService<ICyclicHeatTaskRepository<CyclicHeatTask>>();
    var heatRepository = scope.ServiceProvider.GetService<IHeatTaskRepository<HeatTask>>();
    List<GarageHeatingTime> garagesClosestHeatingTimes = new();
    foreach (var garage in garages)
    {
      var customHeatRequests = await heatRepository.Get()
        .Where(item => item.Id == garage.Id && item.Active).ToListAsync(ct);

      var customHeatRequest = customHeatRequests.MinBy(item => Math.Abs((item.Date - DateTime.Now).Ticks));

      var cyclicHeatTasks =
        await cyclicHeatRepository
          .Get(new CyclicHeatingTaskQueryOptions { AsNoTracking = true, IncludeCyclicHeatTaskDays = true })
          .Where(item => item.Id == garage.Id && item.Active)
          .ToListAsync(ct);


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

  private async Task<List<GarageTemperatureDto>> GetListOfGarageTemperatures(List<Garage> garages, CancellationToken ct)
  {
    using var scope = _serviceScopeFactoryLocator.CreateScope();
    var temperatureRepository =
      scope.ServiceProvider
        .GetService<IOutsideTemperatureRepository<OutsideTemperature>>();
    List<GarageTemperatureDto> listOfGarageTemperatureDtos = [];

    for (var i = 0; i < garages.Count; i++)
    {
      var response = await garageClient.GetGarageTemperature(garages[i].Ip, ct);
      listOfGarageTemperatureDtos.Add(new GarageTemperatureDto { Id = i + 1, Temperature = response.Temperature });
      var entity = new OutsideTemperature { Date = DateTime.Now, Temperature = response.Temperature, GarageId = i + 1 };
      await temperatureRepository.AddAsync(entity, ct);
    }

    await temperatureRepository.UnitOfWork.SaveChangesAsync(ct);

    return listOfGarageTemperatureDtos;
  }

  private async void CheckIfShouldBeOff(List<GarageHeatingTime> todaysHeatTimes, List<Garage> garages,
    CancellationToken ct)
  {
    var scope = _serviceScopeFactoryLocator.CreateScope();
    var heatingLogRepository = scope.ServiceProvider.GetService<IHeatingLogRepository>();

    foreach (var heatTime in todaysHeatTimes)
    {
      var garage = garages.Find(i => i.Id == heatTime.Id);
      var garageHeaterStatus = _garagesHeatersStatuses.Find(i => i.Id == heatTime.Id);

      if (!heatTime.HeatTime.HasValue && garageHeaterStatus.HeatingStatus)
      {
        await garageClient.ChangeHeaterStatus("false", garage.Ip, ct);
        _logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        await heatingLogRepository.AddAsync(
          new HeatLog { Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}" }, ct);
        continue;
      }

      if (DateTime.Now.TimeOfDay.TotalSeconds > heatTime.HeatTime!.Value.TimeOfDay.TotalSeconds &&
          garageHeaterStatus.HeatingStatus)
      {
        await garageClient.ChangeHeaterStatus("false", garage.Ip, ct);
        _logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        await heatingLogRepository.AddAsync(
          new HeatLog { Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}" }, ct);
      }
    }
  }
  
  private async void SetOnHeaters(List<GarageStartHeatTime> startHeatTimes, List<Garage> garages, CancellationToken ct)
  {
    var scope = _serviceScopeFactoryLocator.CreateScope();
    var heatingLogRepository = scope.ServiceProvider.GetService<IHeatingLogRepository>();

    foreach (var garageStartHeatTime in startHeatTimes)
    {
      if (!garageStartHeatTime.StartHeatTime.HasValue)
      {
        continue;
      }

      if (DateTime.Now.Date.Equals(garageStartHeatTime.StartHeatTime.Value.Date) &&
          DateTime.Now.TimeOfDay.TotalSeconds > garageStartHeatTime.StartHeatTime.Value.TimeOfDay.TotalSeconds)
      {
        var ip = garages.Find(garage => garage.Id == garageStartHeatTime.Id).Ip;
        if (!String.IsNullOrEmpty(ip))
        {
          await garageClient.ChangeHeaterStatus("true", ip, ct);
          _logger.LogInformation(
            $"Sett heater ON in garage {garageStartHeatTime.Id} running at: {DateTimeOffset.Now}");
          await heatingLogRepository.AddAsync(
            new HeatLog { Date = DateTime.UtcNow, Info = $"Starting heating garage {garageStartHeatTime.Id}" }, ct);
        }
      }
    }
  }

  private async Task InitGarageHeatersStatuses(List<Garage> garages, CancellationToken ct)
  {
    foreach (var garage in garages)
    {
      if (_garagesHeatersStatuses.Exists(item => item.Id == garage.Id))
      {
        continue;
      }

      var response = await garageClient.GetHeaterStatus(garage.Ip, ct);
      _garagesHeatersStatuses.Add(new GarageHeaterStatus { Id = garage.Id, HeatingStatus = response.HeaterStatus });
    }
  }

  #endregion
}

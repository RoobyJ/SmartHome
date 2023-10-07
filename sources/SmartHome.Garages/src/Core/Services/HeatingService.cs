using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.DTOs;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Models;
using SmartHome.Heater.Models;

namespace SmartHome.Core.Services;

public class HeatingService : IHeatingService
{
  private readonly ILoggerAdapter<HeatingService> _logger;
  private readonly IServiceLocator _serviceScopeFactoryLocator;
  private readonly StartHeatingTimeCalculator _startHeatingTimeCalculator;
  private readonly List<GarageHeaterStatus> _garagesHeatersStatuses = new();

  public HeatingService(ILoggerAdapter<HeatingService> logger,
    IServiceLocator serviceScopeFactoryLocator, StartHeatingTimeCalculator startHeatingTimeCalculator)
  {
    _logger = logger;
    _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
    _startHeatingTimeCalculator = startHeatingTimeCalculator;
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

      await InitGarageHeatersStatuses(garages);


      var closestHeatTimes = await FindClosestHeatTime(garages, ct);


      CheckIfShouldBeOff(closestHeatTimes, garages);

      var listOfGarageTemperatures = await GetListOfGarageTemperatures(garages);

      var listOfStartHeatTimes =
        _startHeatingTimeCalculator.CalculateForMultipleGarages(listOfGarageTemperatures, closestHeatTimes);

      SetOnHeaters(listOfStartHeatTimes, garages);
      await Task.Delay(1000);
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      _logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} threw an exception.");
      // TODO: Decide if you want to re-throw which will crash the worker service
      //throw;
    }
  }


  #region private

  private async Task<List<GarageHeatingTime>> FindClosestHeatTime(List<Garage> garages, CancellationToken ct)
  {
    using var scope = _serviceScopeFactoryLocator.CreateScope();
    var cyclicHeatingRepository = scope.ServiceProvider.GetService<ICyclicHeatingRequestRepository<CyclicHeatRequest>>();
    var heatingRepository = scope.ServiceProvider.GetService<IHeatRequestRepository<HeatRequest>>();
    List<GarageHeatingTime> garagesClosestHeatingTimes = new();
    foreach (var garage in garages)
    {
      var customHeatRequests = await heatingRepository.Get(new HeatRequestQueryOptions { AsNoTracking = true })
        .Where(item => item.Id == garage.Id).ToListAsync(ct);
      
      var customHeatRequest = customHeatRequests.MinBy(item => Math.Abs((item.HeatRequest1 - DateTime.Now).Ticks));

      var cyclicHeatRequests =
        await cyclicHeatingRepository
          .Get(new CyclicHeatingRequestQueryOptions { AsNoTracking = true }).Where(item => item.Id == garage.Id)
          .ToListAsync(ct);


      DateTime? closestDateTime = null;

      foreach (var cyclicHeatRequest in cyclicHeatRequests)
      {
        var result = HeatingServiceHelper.CheckWhichIsCloser(cyclicHeatRequest.ToList(), customHeatRequest);
        if (closestDateTime == null)
        {
          closestDateTime = result;
          continue;
        }

        if (result.HasValue && closestDateTime.Value.Second > result.Value.Second) closestDateTime = result;
      }
        
      if (closestDateTime != null)
      {
        garagesClosestHeatingTimes.Add(new GarageHeatingTime { Id = garage.Id, HeatTime = closestDateTime });
      }
    }

    return garagesClosestHeatingTimes;
  }

  private async Task<List<GarageTemperatureDto>> GetListOfGarageTemperatures(List<Garage> garages)
  {
    List<GarageTemperatureDto> listOfGarageTemperatureDtos = new();

    for (var i = 0; i < garages.Count; i++)
    {
      var response = await GarageClient.GetGarageTemperature(garages[i].Ip);
      listOfGarageTemperatureDtos.Add(new GarageTemperatureDto { Id = i + 1, Temperature = response.Temperature });
    }

    return listOfGarageTemperatureDtos;
  }

  private async void CheckIfShouldBeOff(List<GarageHeatingTime> todaysHeatTimes, List<Garage> garages)
  {
    var scope = _serviceScopeFactoryLocator.CreateScope();
    var heatingLogRepository = scope.ServiceProvider.GetService<IHeatingLogRepository>();

    foreach (var heatTime in todaysHeatTimes)
    {
      var garage = garages.Find(i => i.Id == heatTime.Id);
      var garageHeaterStatus = _garagesHeatersStatuses.Find(i => i.Id == heatTime.Id);

      if (!heatTime.HeatTime.HasValue && garageHeaterStatus.HeatingStatus)
      {
        await GarageClient.ChangeHeaterStatus("false", garage.Ip);
        _logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        await heatingLogRepository.AddAsync(new HeatingLog
        {
          Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}"
        });
        continue;
      }

      if (DateTime.Now.TimeOfDay.TotalSeconds > heatTime.HeatTime.Value.TimeOfDay.TotalSeconds &&
          garageHeaterStatus.HeatingStatus)
      {
        await GarageClient.ChangeHeaterStatus("false", garage.Ip);
        _logger.LogInformation($"Set heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        await heatingLogRepository.AddAsync(new HeatingLog
        {
          Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}"
        });
      }
    }
  }

  // TODO: rework this method to much scopes!
  private async void SetOnHeaters(List<GarageStartHeatTime> startHeatTimes, List<Garage> garages)
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
          await GarageClient.ChangeHeaterStatus("true", ip);
          _logger.LogInformation(
            $"Setted heater ON in garage {garageStartHeatTime.Id} running at: {DateTimeOffset.Now}");
          await heatingLogRepository.AddAsync(new HeatingLog
          {
            Date = DateTime.UtcNow, Info = $"Starting heating garage {garageStartHeatTime.Id}"
          });
        }
      }
    }
  }

  private async Task InitGarageHeatersStatuses(List<Garage> garages)
  {
    foreach (var garage in garages)
    {
      if (_garagesHeatersStatuses.Exists(item => item.Id == garage.Id))
      {
        continue;
      }

      var response = await GarageClient.GetHeaterStatus(garage.Ip);
      _garagesHeatersStatuses.Add(new GarageHeaterStatus { Id = garage.Id, HeatingStatus = response.HeaterStatus });
    }
  }

  #endregion
}

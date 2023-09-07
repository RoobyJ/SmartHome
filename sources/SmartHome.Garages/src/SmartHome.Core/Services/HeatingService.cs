using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Models;
using SmartHome.Heater.Models;
using SmartHome.Heater.Settings;

namespace SmartHome.Core.Services;

public class HeatingService : IHeatingService
{
  private readonly ILoggerAdapter<HeatingService> _logger;
  private readonly IServiceLocator _serviceScopeFactoryLocator;
  private readonly StartHeatingTimeCalculator _startHeatingTimeCalculator;
  private readonly HeatingSettings _settings;
  private static readonly HttpClient Client = new();
  private List<GarageHeaterStatus> _garagesHeatersStatuses = new();

  public HeatingService(ILoggerAdapter<HeatingService> logger, HeatingSettings settings,
    IServiceLocator serviceScopeFactoryLocator, StartHeatingTimeCalculator startHeatingTimeCalculator)
  {
    _logger = logger;
    _settings = settings;
    _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
    _startHeatingTimeCalculator = startHeatingTimeCalculator;
  }

  public async Task ExecuteAsync()
  {
    _logger.LogInformation($"{nameof(HeatingService)} running at: {DateTimeOffset.Now}");
    try
    {
      // EF Requires a scope so we are creating one per execution here
      using var scope = _serviceScopeFactoryLocator.CreateScope();
      var garageRepository =
        scope.ServiceProvider
          .GetService<IGarageRepository>();


      var garages = garageRepository.Get(new GarageQueryOptions() { AsNoTracking = true }).ToList();

      await InitGarageHeatersStatuses(garages);


      var closestHeatTimes = FindClosestHeatTime(garages);


      this.CheckIfShouldBeOff(closestHeatTimes, garages);

      var listOfGarageTemperatures = await this.GetListOfGarageTemperatures(garages);
      
      var listOfStartHeatTimes = this._startHeatingTimeCalculator.CalculateForMultipleGarages(listOfGarageTemperatures, closestHeatTimes);
      
      this.SetOnHeaters(listOfStartHeatTimes, garages);
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

  private List<GarageHeatingTime> FindClosestHeatTime(List<Garage> garages)
  {
    List<GarageHeatingTime> garagesClosestHeatingTimes = new();
    foreach (var garage in garages)
    {
      var closestDateTime = CheckWhichIsCloser(garage.Id);
      if (closestDateTime != null)
        garagesClosestHeatingTimes.Add(new GarageHeatingTime() { Id = garage.Id, HeatTime = closestDateTime });
    }

    return garagesClosestHeatingTimes;
  }

  private DateTime? CheckWhichIsCloser(int garageId)
  {
    var todayDay = (int)DateTime.Today.DayOfWeek;
    using var scope = _serviceScopeFactoryLocator.CreateScope();
    var cyclicHeatingRepository = scope.ServiceProvider.GetService<ICyclicHeatingRequestRepository>();
    var heatingRepository = scope.ServiceProvider.GetService<IHeatingRequestRepository>();
    var customHeatRequest = heatingRepository.Get(new GarageQueryOptions() { AsNoTracking = true })
      .Where(item => item.Id == garageId).MinBy(item => Math.Abs((item.HeatRequest1 - DateTime.Now).Ticks));

    var cyclicHeatRequest =
      cyclicHeatingRepository
        .Get(new GarageQueryOptions() { AsNoTracking = true }).FirstOrDefault(item => item.Id == garageId)
        ?.ToList();

    if (customHeatRequest.HeatRequest1.Date.Day.Equals(DateTime.Now.Day) && cyclicHeatRequest?[todayDay] != null)
    {
      // Check for today comparation
      if (customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds &&
          cyclicHeatRequest[todayDay].Value.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds)
      {
        if (customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds > cyclicHeatRequest[todayDay].Value.TotalSeconds)
        {
          return customHeatRequest.HeatRequest1;
        }

        return DateTime.Now.Date + cyclicHeatRequest[todayDay].Value;
      }
    }
    else if (cyclicHeatRequest?[todayDay] != null && !customHeatRequest.HeatRequest1.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      // today
      if (DateTime.Now.TimeOfDay.TotalSeconds < cyclicHeatRequest[todayDay].Value.TotalSeconds)
        return DateTime.Now.Date + cyclicHeatRequest[todayDay].Value;
    }
    else if (cyclicHeatRequest?[todayDay] == null && customHeatRequest.HeatRequest1.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      if (DateTime.Now.TimeOfDay.TotalSeconds < customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds)
        return DateTime.Now.Date + customHeatRequest.HeatRequest1.TimeOfDay;
    }
    else if (cyclicHeatRequest != null && (customHeatRequest.HeatRequest1.Day.Equals(DateTime.Now.AddDays(1).Day) ||
                                           cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1] != null))
    {
      // Check for tomorrow
      if (!(customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds) ||
          !(cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value.TotalSeconds >
            DateTime.Now.TimeOfDay.TotalSeconds))
      {
        return null;
      }

      if (customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds >
          cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value.TotalSeconds)
      {
        return customHeatRequest.HeatRequest1;
      }

      return DateTime.Now.Date + cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value;
    }

    return null;
  }

  private async Task<List<GarageTemperatureDto>> GetListOfGarageTemperatures(List<Garage> garages)
  {
    List<GarageTemperatureDto> listOfGarageTemperatureDtos = new();

    for (int i = 0; i < garages.Count; i++)
    {
      var response = await Client.GetAsync($"http://{garages[i].Ip}/temperature/");
      var contentString = await response.Content.ReadAsStringAsync();
      var json = JsonConvert.DeserializeObject<TemperatureDto>(contentString);
      listOfGarageTemperatureDtos.Add(new GarageTemperatureDto() { Id = i + 1, Temperature = json.Temperature });
    }

    return listOfGarageTemperatureDtos;
  }

  private void CheckIfShouldBeOff(List<GarageHeatingTime> todaysHeatTimes, List<Garage> garages)
  {
    var scope = _serviceScopeFactoryLocator.CreateScope();
    var heatingLogRepository = scope.ServiceProvider.GetService<IHeatingLogRepository>();

    foreach (var heatTime in todaysHeatTimes)
    {
      var garage = garages.Find(i => i.Id == heatTime.Id);
      var garageHeaterStatus = this._garagesHeatersStatuses.Find(i => i.Id == heatTime.Id);

      if (!heatTime.HeatTime.HasValue && garageHeaterStatus.HeatingStatus)
      {
        ChangeHeaterStatus("false", garage.Ip);
        _logger.LogInformation($"Setted heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        heatingLogRepository.AddAsync(new HeatingLog()
        {
          Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}"
        });
        continue;
      }

      if (DateTime.Now.TimeOfDay.TotalSeconds > heatTime.HeatTime.Value.TimeOfDay.TotalSeconds &&
          garageHeaterStatus.HeatingStatus)
      {
        ChangeHeaterStatus("false", garage.Ip);
        _logger.LogInformation($"Setted heater OFF in garage {garage.Id} running at: {DateTimeOffset.Now}");
        heatingLogRepository.AddAsync(new HeatingLog()
        {
          Date = DateTime.UtcNow, Info = $"Ended heating garage {garage.Id}"
        });
      }
    }
  }

  // TODO: rework this method
  private void SetOnHeaters(List<GarageStartHeatTime> startHeatTimes, List<Garage> garages)
  {
    var scope = _serviceScopeFactoryLocator.CreateScope();
    var heatingLogRepository = scope.ServiceProvider.GetService<IHeatingLogRepository>();

    foreach (var garageStartHeatTime in startHeatTimes)
    {
      if (!garageStartHeatTime.StartHeatTime.HasValue) continue;
      if (DateTime.Now.Date.Equals(garageStartHeatTime.StartHeatTime.Value.Date) &&
          DateTime.Now.TimeOfDay.TotalSeconds > garageStartHeatTime.StartHeatTime.Value.TimeOfDay.TotalSeconds)
      {
        var ip = garages.Find((garage) => garage.Id == garageStartHeatTime.Id).Ip;
        if (!String.IsNullOrEmpty(ip))
        {
          ChangeHeaterStatus("true", ip);
          _logger.LogInformation(
            $"Setted heater ON in garage {garageStartHeatTime.Id} running at: {DateTimeOffset.Now}");
          heatingLogRepository.AddAsync(new HeatingLog()
          {
            Date = DateTime.UtcNow, Info = $"Starting heating garage {garageStartHeatTime.Id}"
          });
        }
      }
    }
  }

  private static async void ChangeHeaterStatus(string onOff, string ip)
  {
    var values = new Dictionary<string, string> { { "heat", $"{onOff}" } };

    var content = new FormUrlEncodedContent(values);
    await Client.PutAsync($"http://{ip}/heater/", content);
  }

  private async Task InitGarageHeatersStatuses(List<Garage> garages)
  {
    foreach (var garage in garages)
    {
      if (this._garagesHeatersStatuses.Exists(item => item.Id == garage.Id))
      {
        continue;
      }

      var response = await GetGarageHeaterStatus(garage.Ip);
      this._garagesHeatersStatuses.Add(new GarageHeaterStatus()
      {
        Id = garage.Id, HeatingStatus = response.HeaterStatus
      });
    }
  }

  private async Task<GarageHeaterStatusDto> GetGarageHeaterStatus(string garageIp)
  {
    var response = await Client.GetAsync($"http://{garageIp}/heater-status/");
    var contentString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<GarageHeaterStatusDto>(contentString);
  }

  #endregion
}

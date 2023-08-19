using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartHome.TasksManager.Core.Dtos;
using SmartHome.TasksManager.Core.Entities;
using SmartHome.TasksManager.Heater.Interfaces;
using SmartHome.TasksManager.Core.Interfaces;
using SmartHome.TasksManager.Core.Models;
using SmartHome.TasksManager.Core.Services;
using SmartHome.TasksManager.Heater.Dtos;
using SmartHome.TasksManager.Heater.Entities;
using SmartHome.TasksManager.Heater.Helpers;
using SmartHome.TasksManager.Heater.Models;
using SmartHome.TasksManager.Heater.Settings;

namespace SmartHome.TasksManager.Heater.Services;

public class HeatingService : IHeatingService
{
  private readonly ILoggerAdapter<HeatingService> _logger;
  private readonly IServiceLocator _serviceScopeFactoryLocator;
  private readonly HeatingSettings _settings;
  private static readonly HttpClient Client = new();

  public HeatingService(ILoggerAdapter<HeatingService> logger, HeatingSettings settings,
    IServiceLocator serviceScopeFactoryLocator)
  {
    _logger = logger;
    _settings = settings;
    _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
  }

  public async Task ExecuteAsync()
  {
    _logger.LogInformation($"{nameof(HeatingService)} running at: {DateTimeOffset.Now}");
    try
    {
      // EF Requires a scope so we are creating one per execution here
      using var scope = _serviceScopeFactoryLocator.CreateScope();
      var repository =
        scope.ServiceProvider
          .GetService<IRepository>();
      var jsonFile = scope.ServiceProvider.GetService<IJsonFileReader>();
      var garages = repository.List<Garage>();
      
      var cyclicGaragesHeatTimes =
        await jsonFile.GetCyclicHeatDaysObject<GaragesJsonObject>(_settings.cyclicHeatDaysJsonFilePath);

      var customHeatRequests = repository.List<HeatRequest>();
      
      var todaysHeatTimes = FindNearestHeatTime.Find(cyclicGaragesHeatTimes, customHeatRequests);

      this.CheckIfShouldBeOff(todaysHeatTimes, garages, repository);

      var listOfGarageTemperatures = await this.GetListOfGarageTemperatures(garages);

      var listOfStartHeatTimes = new WhenToStartHeatingCalculation().CalculateForMultipleGarages(listOfGarageTemperatures, todaysHeatTimes);

      this.SetOnHeaters(listOfStartHeatTimes, garages, repository);
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      _logger.LogError(ex, $"{nameof(HeatingService)}.{nameof(ExecuteAsync)} threw an exception.");
      // TODO: Decide if you want to re-throw which will crash the worker service
      //throw;
    }
  }

  private async Task<List<GarageTemperatureDto>> GetListOfGarageTemperatures(List<Garage> garages)
  {
    List<GarageTemperatureDto> listOfGarageTemperatureDtos = new();
    
    for (int i = 0; i < garages.Count; i++)
    {
      var response = await Client.GetAsync($"http://{garages[i].Ip}/Temperature/");
      var contentString = await response.Content.ReadAsStringAsync();
      var json = JsonConvert.DeserializeObject<TemperatureDto>(contentString);
      listOfGarageTemperatureDtos.Add(new GarageTemperatureDto(){Id=i + 1,Temperature = json.Temperature});
    }

    return listOfGarageTemperatureDtos;
  }

  private void CheckIfShouldBeOff(List<GarageHeatingTime> todaysHeatTimes, List<Garage> garages, IRepository repository)
  {
    for (int i = 0; i < todaysHeatTimes.Count; i++)
    {
      if (!todaysHeatTimes[i].HeatTime.HasValue)
      {
        ChangeHeaterStatus("false", garages[i].Ip);
        _logger.LogInformation($"Setted heater OFF in garage {garages[i].Id} running at: {DateTimeOffset.Now}");
        repository.Add(new HeatingLog() { Date = DateTime.UtcNow, Info = $"Ended heating garage {garages[i].Id}" });
        continue;
      }
      if (DateTime.Now.TimeOfDay.TotalSeconds > todaysHeatTimes[i].HeatTime.Value.TimeOfDay.TotalSeconds)
      {
        ChangeHeaterStatus("false", garages[i].Ip);
        _logger.LogInformation($"Setted heater OFF in garage {garages[i].Id} running at: {DateTimeOffset.Now}");
        repository.Add(new HeatingLog() { Date = DateTime.UtcNow, Info = $"Ended heating garage {garages[i].Id}" });
      }
    }
  }

  private void SetOnHeaters(List<GarageStartHeatTime> startHeatTimes, List<Garage> garages, IRepository repository)
  {
    foreach (var garageStartHeatTime in startHeatTimes)
    {
      if (!garageStartHeatTime.StartHeatTime.HasValue) continue;
      if (DateTime.Now.Date.Equals(garageStartHeatTime.StartHeatTime.Value.Date) && DateTime.Now.TimeOfDay.TotalSeconds > garageStartHeatTime.StartHeatTime.Value.TimeOfDay.TotalSeconds)
      {
        var ip = garages.Find((garage) => garage.Id == garageStartHeatTime.Id).Ip;
        if (!String.IsNullOrEmpty(ip))
        {
          this.ChangeHeaterStatus("true", ip);
          _logger.LogInformation($"Setted heater ON in garage {garageStartHeatTime.Id} running at: {DateTimeOffset.Now}");
          repository.Add(new HeatingLog() { Date = DateTime.UtcNow, Info = $"Starting heating garage {garageStartHeatTime.Id}" });
        }
      }
    }

    
  }
  
  private async void ChangeHeaterStatus(string onOff, string ip)
  {
    var values = new Dictionary<string, string>
    {
      { "heat", $"{onOff}" }
    };

    var content = new FormUrlEncodedContent(values);
    await Client.PutAsync($"http://{ip}/Heater/", content);
  }
  
}

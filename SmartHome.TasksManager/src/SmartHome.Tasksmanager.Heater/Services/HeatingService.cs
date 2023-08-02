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
using SmartHome.TasksManager.Heater.Settings;

namespace SmartHome.TasksManager.Heater.Services;

public class HeatingService : IHeatingService
{
  private readonly ILoggerAdapter<HeatingService> _logger;
  private readonly IServiceLocator _serviceScopeFactoryLocator;
  private readonly HeatingSettings _settings;
  private List<GarageHeatingTime> heatTimes = new();
  private static readonly HttpClient Client = new HttpClient();

  public HeatingService(ILoggerAdapter<HeatingService> logger, HeatingSettings settings,
    IServiceLocator serviceScopeFactoryLocator)
  {
    _logger = logger;
    _settings = settings;
    _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
  }

  public async Task ExecuteAsync()
  {
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
      
      var todaysHeatTimes = MapJSONHeatTimesToDict.GetHeatTimeFromCyclicHeatDays(cyclicGaragesHeatTimes);
      
     //TODO: change this to find the nearest heat on timespan
      
      var listOfGarageTemperatures = await this.GetListOfGarageTemperatures(garages);
      // read from the queue
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      _logger.LogError(ex, $"{nameof(EntryPointService)}.{nameof(ExecuteAsync)} threw an exception.");
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

  private void CheckIfShouldBeOff(List<GarageHeatingTime> todaysHeatTimes, List<Garage> garages)
  {
    for (int i = 0; i < todaysHeatTimes.Count; i++)
    {
      if (!todaysHeatTimes[i].HeatTime.HasValue)
      {
        ChangeHeaterStatus("false", garages[i].Ip);
        return;
      }
      if (DateTime.Now.TimeOfDay.TotalSeconds > todaysHeatTimes[i].HeatTime.Value.TotalSeconds)
      {
        ChangeHeaterStatus("false", garages[i].Ip);
        return;
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

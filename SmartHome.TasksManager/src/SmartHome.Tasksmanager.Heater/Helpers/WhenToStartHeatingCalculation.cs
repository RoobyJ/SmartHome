using System;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using SmartHome.TasksManager.Core.Dtos;
using SmartHome.TasksManager.Core.Models;
using SmartHome.TasksManager.Heater.Models;

namespace SmartHome.TasksManager.Heater.Helpers;

public class WhenToStartHeatingCalculation
{

  public List<GarageStartHeatTime> CalculateForMultipleGarages(List<GarageTemperatureDto> listOfGarageTemperatureDtos,  List<GarageHeatingTime> listOfGarageEndHeatTimes)
  {
    List<GarageStartHeatTime> listOfGarageStartHeatTimes = new();

    for (int i = 0; i < listOfGarageTemperatureDtos.Count; i++ )
    {
      var startHeatTime = TimeToStartHeating(listOfGarageEndHeatTimes[i].HeatTime, listOfGarageTemperatureDtos[i].Temperature);
      if (startHeatTime == null) continue;
      listOfGarageStartHeatTimes.Add(new GarageStartHeatTime() {Id = i + 1, StartHeatTime = startHeatTime});
    }
    
    return listOfGarageStartHeatTimes;
  }
  
  private double CalculateOnHeatTime(float temp) {
    // linear formula 
    var result = temp - 14.1061;
    result = result / (-0.161569);
    if (result < 0) result *= -1;
    
    return result; // the result is in minutes
  }

  private TimeSpan? TimeToStartHeating(TimeSpan? endHeatTime, float temp)
  {
    if (endHeatTime == null) return null;
    var offsetTimeSpan = TimeSpan.FromMinutes(CalculateOnHeatTime(temp));
    return endHeatTime.Value.Subtract(offsetTimeSpan);
  }
}

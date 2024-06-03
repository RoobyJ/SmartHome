using System;
using System.Collections.Generic;
using SmartHome.Core.DTOs;
using SmartHome.Core.Models;
using SmartHome.Heater.Models;

namespace SmartHome.Core.Helpers;

public class StartHeatingTimeCalculator
{
  public List<GarageStartHeatTime> CalculateForMultipleGarages(List<GarageTemperatureDto> listOfGarageTemperatureDtos,
    List<GarageHeatingTime> listOfGarageEndHeatTimes)
  {
    List<GarageStartHeatTime> listOfGarageStartHeatTimes = new();

    for (var i = 0; i < listOfGarageEndHeatTimes.Count; i++)
    {
      var heatTime = listOfGarageEndHeatTimes[i].HeatTime;
      if (heatTime != null)
      {
        var startHeatTime = TimeToStartHeating(heatTime.Value.TimeOfDay,
          listOfGarageTemperatureDtos[i].Temperature);
        if (startHeatTime == null)
        {
          continue;
        }

        var startHeatingDate = heatTime.Value.TimeOfDay.TotalSeconds < startHeatTime.Value.TotalSeconds
          ? heatTime.Value.AddDays(-1) + startHeatTime
          : heatTime.Value.Date + startHeatTime;
        listOfGarageStartHeatTimes.Add(new GarageStartHeatTime { Id = i + 1, StartHeatTime = startHeatingDate });
      }
    }

    return listOfGarageStartHeatTimes;
  }

  private static double CalculateOnHeatTime(float temp)
  {
    // linear formula 
    return ((Math.Log(temp) + 1) * 35) + 125;
  }

  private static TimeSpan? TimeToStartHeating(TimeSpan? endHeatTime, float temp)
  {
    if (endHeatTime == null)
    {
      return null;
    }

    var offsetTimeSpan = TimeSpan.FromMinutes(CalculateOnHeatTime(temp));
    return endHeatTime.Value.Subtract(offsetTimeSpan);
  }
}

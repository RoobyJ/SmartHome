using System;
using System.Collections.Generic;
using Core.Dtos;
using Core.Models;

namespace Core.Helpers;

public class StartHeatingTimeCalculator
{
  public static List<GarageStartHeatTime> CalculateForMultipleGarages(List<GarageTemperatureDto> listOfGarageTemperatureDtos,
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
        listOfGarageStartHeatTimes.Add(new GarageStartHeatTime
        {
          GarageId = i + 1,
          StartHeatTime = startHeatingDate,
          IsCyclic = listOfGarageEndHeatTimes[i].IsCyclic,
          HeatTaskId = listOfGarageEndHeatTimes[i].HeatTaskId
        });
      }
    }

    return listOfGarageStartHeatTimes;
  }

  private static double CalculateOnHeatTime(float temp)
  {
    return 10 * (5 * (1 + 5 * Math.Pow(Math.E, (-0.02 * temp))) - 11) * 0.37;
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

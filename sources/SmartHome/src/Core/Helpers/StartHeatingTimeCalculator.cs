using System;
using System.Collections.Generic;
using System.Linq;
using Core.Dtos;
using Core.Models;

namespace Core.Helpers;

public class StartHeatingTimeCalculator
{
  public static List<GarageStartHeatTime> CalculateForMultipleGarages(List<GarageTemperatureDto> listOfGarageTemperatureDtos,
    List<GarageHeatingTime> listOfGarageEndHeatTimes)
  {
    List<GarageStartHeatTime> listOfGarageStartHeatTimes = new();

    foreach (var garageEndHeatTime in listOfGarageEndHeatTimes)
    {
      var heatTime = garageEndHeatTime.HeatTime;
      if (heatTime != null)
      {
        var garageTemperatureDto = listOfGarageTemperatureDtos.FirstOrDefault(x => x.Id == garageEndHeatTime.Id);
        var startHeatTime = TimeToStartHeating(heatTime.Value.TimeOfDay, garageTemperatureDto?.Temperature);
        if (startHeatTime == null)
        {
          continue;
        }

        var startHeatingDate = heatTime.Value.TimeOfDay.TotalSeconds < startHeatTime.Value.TotalSeconds
          ? heatTime.Value.AddDays(-1) + startHeatTime
          : heatTime.Value.Date + startHeatTime;
        listOfGarageStartHeatTimes.Add(new GarageStartHeatTime
        {
          GarageId = garageEndHeatTime.Id,
          StartHeatTime = startHeatingDate,
          IsCyclic = garageEndHeatTime.IsCyclic,
          HeatTaskId = garageEndHeatTime.HeatTaskId
        });
      }
    }

    return listOfGarageStartHeatTimes;
  }

  private static double CalculateOnHeatTime(float temp)
  {
    return 10 * (5 * (1 + 5 * Math.Pow(Math.E, (-0.02 * temp))) - 11) * 0.37;
  }

  private static TimeSpan? TimeToStartHeating(TimeSpan? endHeatTime, float? temp)
  {
    if (endHeatTime == null || temp == null)
    {
      return null;
    }

    var offsetTimeSpan = TimeSpan.FromMinutes(CalculateOnHeatTime(temp.Value));
    return endHeatTime.Value.Subtract(offsetTimeSpan);
  }
}

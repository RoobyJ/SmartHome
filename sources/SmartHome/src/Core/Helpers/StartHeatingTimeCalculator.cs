using System;
using System.Collections.Generic;
using System.Linq;
using Core.Dtos;
using Core.Models;

namespace Core.Helpers;

public class StartHeatingTimeCalculator
{
  public static List<HeatTask> CalculateForMultipleGarages(
    List<GarageTemperatureDto> listOfGarageTemperatureDtos,
    List<HeatTask> listOfGarageEndHeatTasks)
  {
    var listOfGarageStartHeatTimes = new List<HeatTask>();

    foreach (var garageEndHeatTime in listOfGarageEndHeatTasks)
    {
      var endHeatTime = garageEndHeatTime.EndTime;

      var garageTemperatureDto = listOfGarageTemperatureDtos.FirstOrDefault(x => x.Id == garageEndHeatTime.GarageId);
      var startHeatTime = TimeToStartHeating(endHeatTime.TimeOfDay, garageTemperatureDto?.Temperature);
      if (startHeatTime == null) continue;

      var startHeatingDate = endHeatTime.TimeOfDay.TotalSeconds < startHeatTime.Value.TotalSeconds
        ? endHeatTime.AddDays(-1) + startHeatTime
        : endHeatTime.Date + startHeatTime;
      garageEndHeatTime.StartTime = startHeatingDate;
      listOfGarageStartHeatTimes.Add(garageEndHeatTime);
    }

    return listOfGarageStartHeatTimes;
  }

  #region private methods
  private static double CalculateOnHeatTime(float temp)
  {
    // TODO: check if this function can return negative values
    return 10 * (5 * (1 + 5 * Math.Pow(Math.E, (-0.02 * temp))) - 11) * 0.37;
  }

  private static TimeSpan? TimeToStartHeating(TimeSpan? endHeatTime, float? temp)
  {
    if (endHeatTime == null || temp == null) return null;

    var offsetTimeSpan = TimeSpan.FromMinutes(CalculateOnHeatTime(temp.Value));
    return endHeatTime.Value.Subtract(offsetTimeSpan);
  }
  #endregion
}

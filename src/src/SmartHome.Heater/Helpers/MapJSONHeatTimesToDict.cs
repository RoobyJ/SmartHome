using System;
using System.Collections.Generic;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Heater.Helpers;

public class MapJSONHeatTimesToDict
{
  public static List<GarageHeatingTimespan> GetHeatTimeFromCyclicHeatDays(GaragesJsonObject cyclicGaragesHeatTimes)
  {
    List<GarageHeatingTimespan> listOfHeatTimes = new();
    var dayOfWeek = DateTime.Today.DayOfWeek.ToString();
    
    for (int i = 0; i < cyclicGaragesHeatTimes.Garages.Length; i++)
    {
      switch (dayOfWeek)
      {
        case "Monday":
          listOfHeatTimes.Add(new GarageHeatingTimespan()
          {
            Id = i + 1, HeatTime = cyclicGaragesHeatTimes.Garages[i].Monday
          });
          break;
        case "Tuesday":
          listOfHeatTimes.Add(new GarageHeatingTimespan()
          {
            Id = i + 1, HeatTime = cyclicGaragesHeatTimes.Garages[i].Tuesday
          });
          break;
        case "Wednesday":
          listOfHeatTimes.Add(new GarageHeatingTimespan()
          {
            Id = i + 1, HeatTime = cyclicGaragesHeatTimes.Garages[i].Wednesday
          });
          break;
        case "Thursday":
          listOfHeatTimes.Add(new GarageHeatingTimespan()
          {
            Id = i + 1, HeatTime = cyclicGaragesHeatTimes.Garages[i].Thursday
          });
          break;
        case "Friday":
          listOfHeatTimes.Add(new GarageHeatingTimespan()
          {
            Id = i + 1, HeatTime = cyclicGaragesHeatTimes.Garages[i].Friday
          });
          break;
        case "Saturday":
          listOfHeatTimes.Add(new GarageHeatingTimespan()
          {
            Id = i + 1, HeatTime = cyclicGaragesHeatTimes.Garages[i].Saturday
          });
          break;
        case "Sunday":
          listOfHeatTimes.Add(new GarageHeatingTimespan()
          {
            Id = i + 1, HeatTime = cyclicGaragesHeatTimes.Garages[i].Sunday
          });
          break;
        default:
          break;
      }
    }

    return listOfHeatTimes;
  }
}

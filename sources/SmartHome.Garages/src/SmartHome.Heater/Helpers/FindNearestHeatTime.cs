using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Heater.Helpers;

public class FindNearestHeatTime
{
  public static List<GarageHeatingTime> Find(
    List<HeatRequest> customHeatRequests)
  {
    List<GarageHeatingTime> listOfGarageHeatingTimes = new();
    var todayDay = (int)DateTime.Today.DayOfWeek;
    

    foreach (var customHeatRequest in customHeatRequests)
    {
      if (listOfGarageHeatingTimes.Exists(request => request.Id == customHeatRequest.GarageId))
      {
        var heatTime = listOfGarageHeatingTimes.Find(request => request.Id == customHeatRequest.GarageId).HeatTime;
        if (heatTime != null && 0 < DateTime.Compare(
              heatTime.Value,
              customHeatRequest.HeatRequest1))
        {
          var heatTimeIndex = listOfGarageHeatingTimes.FindIndex(request => request.Id == customHeatRequest.GarageId);
          listOfGarageHeatingTimes[heatTimeIndex] = new GarageHeatingTime
          {
            Id = customHeatRequest.GarageId, HeatTime = customHeatRequest.HeatRequest1
          };
        }
      }
      else
      {
        listOfGarageHeatingTimes.Add(new GarageHeatingTime
        {
          Id = customHeatRequest.GarageId, HeatTime = customHeatRequest.HeatRequest1
        });
      }
    }


    return listOfGarageHeatingTimes;
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.TasksManager.Core.Entities;
using SmartHome.TasksManager.Core.Models;
using SmartHome.TasksManager.Heater.Entities;

namespace SmartHome.TasksManager.Heater.Helpers;

public class FindNearestHeatTime
{
  public static List<GarageHeatingTime> Find(GaragesJsonObject cyclicGaragesHeatTimes,
    List<HeatRequest> customHeatRequests)
  {
    List<GarageHeatingTime> listOfGarageHeatingTimes = new();
    var todayDay = (int)DateTime.Today.DayOfWeek;

    for (int i = 0; i < cyclicGaragesHeatTimes.Garages.Length; i++)
    {
      if (cyclicGaragesHeatTimes.Garages[i].ToList()[todayDay].HasValue)
      {
        if (cyclicGaragesHeatTimes.Garages[i].ToList()[todayDay].Value.TotalSeconds -
            DateTime.Today.TimeOfDay.TotalSeconds < 0)
        {
          if (i != cyclicGaragesHeatTimes.Garages.Length)
          {
            var newDate = DateTime.Today.AddDays(1);
            var timeStamp = cyclicGaragesHeatTimes.Garages[i].ToList()[todayDay + 1];
            listOfGarageHeatingTimes.Add(new GarageHeatingTime { Id = i, HeatTime = newDate + timeStamp });
          }
        }
        else
        {
          var newDate = DateTime.Today;
          var timeStamp = cyclicGaragesHeatTimes.Garages[i].ToList()[todayDay];
          listOfGarageHeatingTimes.Add(new GarageHeatingTime { Id = i, HeatTime = newDate + timeStamp });
        }
      }
    }

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

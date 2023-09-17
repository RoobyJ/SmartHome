using System;
using System.Collections.Generic;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Core.Helpers;

public class HeatingServiceHelper
{

  public static DateTime? CheckWhichIsCloser(List<TimeSpan?> cyclicHeatRequest, HeatRequest customHeatRequest)
  {
    var todayDay = (int)DateTime.Today.DayOfWeek;
    
    if (customHeatRequest.HeatRequest1.Date.Day.Equals(DateTime.Now.Day) && cyclicHeatRequest?[todayDay] != null)
    {
      // Check for today comparation
      if (customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds &&
          cyclicHeatRequest[todayDay].Value.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds)
      {
        if (customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds < cyclicHeatRequest[todayDay].Value.TotalSeconds)
        {
          return customHeatRequest.HeatRequest1;
        }

        return DateTime.Now.Date + cyclicHeatRequest[todayDay].Value;
      }
    }
    else if (cyclicHeatRequest?[todayDay] != null && !customHeatRequest.HeatRequest1.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      // today
      if (DateTime.Now.TimeOfDay.TotalSeconds < cyclicHeatRequest[todayDay].Value.TotalSeconds)
        return DateTime.Now.Date + cyclicHeatRequest[todayDay].Value;
    }
    else if (cyclicHeatRequest?[todayDay] == null && customHeatRequest.HeatRequest1.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      if (DateTime.Now.TimeOfDay.TotalSeconds < customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds)
        return DateTime.Now.Date + customHeatRequest.HeatRequest1.TimeOfDay;
    }
    else if (cyclicHeatRequest != null && (customHeatRequest.HeatRequest1.Day.Equals(DateTime.Now.AddDays(1).Day) ||
                                           cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1] != null))
    {
      // Check for tomorrow
      if (!(customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds) ||
          !(cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value.TotalSeconds >
            DateTime.Now.TimeOfDay.TotalSeconds))
      {
        return null;
      }

      if (customHeatRequest.HeatRequest1.TimeOfDay.TotalSeconds >
          cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value.TotalSeconds)
      {
        return customHeatRequest.HeatRequest1;
      }

      return DateTime.Now.Date + cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value;
    }

    return null;
  }
}

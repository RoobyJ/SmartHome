using System;
using System.Collections.Generic;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Helpers;

public class HeatingServiceHelper
{
  public static DateTime? CheckWhichIsCloser(List<TimeSpan?> cyclicHeatRequest, HeatTask customHeatRequest)
  {
    var todayDay = (int)DateTime.Today.DayOfWeek;

    if (customHeatRequest.HeatRequest.Date.Day.Equals(DateTime.Now.Day) && cyclicHeatRequest?[todayDay] != null)
    {
      // Check for today comparation
      if (customHeatRequest.HeatRequest.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds &&
          cyclicHeatRequest[todayDay].Value.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds)
      {
        if (customHeatRequest.HeatRequest.TimeOfDay.TotalSeconds < cyclicHeatRequest[todayDay].Value.TotalSeconds)
        {
          return customHeatRequest.HeatRequest;
        }

        return DateTime.Now.Date + cyclicHeatRequest[todayDay].Value;
      }
    }
    else if (cyclicHeatRequest?[todayDay] != null && !customHeatRequest.HeatRequest.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      // today
      if (DateTime.Now.TimeOfDay.TotalSeconds < cyclicHeatRequest[todayDay].Value.TotalSeconds)
      {
        return DateTime.Now.Date + cyclicHeatRequest[todayDay].Value;
      }
    }
    else if (cyclicHeatRequest?[todayDay] == null && customHeatRequest.HeatRequest.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      if (DateTime.Now.TimeOfDay.TotalSeconds < customHeatRequest.HeatRequest.TimeOfDay.TotalSeconds)
      {
        return DateTime.Now.Date + customHeatRequest.HeatRequest.TimeOfDay;
      }
    }
    else if (cyclicHeatRequest != null && (customHeatRequest.HeatRequest.Day.Equals(DateTime.Now.AddDays(1).Day) ||
                                           cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1] != null))
    {
      // Check for tomorrow
      if (!(customHeatRequest.HeatRequest.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds) ||
          !(cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value.TotalSeconds >
            DateTime.Now.TimeOfDay.TotalSeconds))
      {
        return null;
      }

      if (customHeatRequest.HeatRequest.TimeOfDay.TotalSeconds >
          cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value.TotalSeconds)
      {
        return customHeatRequest.HeatRequest;
      }

      return DateTime.Now.Date + cyclicHeatRequest[todayDay == 6 ? 0 : todayDay + 1].Value;
    }

    return null;
  }
}

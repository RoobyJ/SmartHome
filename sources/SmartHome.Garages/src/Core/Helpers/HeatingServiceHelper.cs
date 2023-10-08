using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Helpers;

public abstract class HeatingServiceHelper
{
  public static DateTime? CheckWhichIsCloser(CyclicHeatTask cyclicHeatTask, HeatTask customHeatRequest)
  {
    var todayDay = (int)DateTime.Today.DayOfWeek;

    if (customHeatRequest.HeatTask1.Date.Day.Equals(DateTime.Now.Day) &&
        cyclicHeatTask.CyclicHeatTaskDaysInWeeks.FirstOrDefault(i => i.DayId == todayDay) != null)
    {
      // Check for today comparation
      if (customHeatRequest.HeatTask1.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds &&
          cyclicHeatTask.Time.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds)
      {
        if (customHeatRequest.HeatTask1.TimeOfDay.TotalSeconds < cyclicHeatTask.Time.TotalSeconds)
        {
          return customHeatRequest.HeatTask1;
        }

        return DateTime.Now.Date + cyclicHeatTask.Time;
      }
    }
    else if (!customHeatRequest.HeatTask1.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      // today
      if (DateTime.Now.TimeOfDay.TotalSeconds < cyclicHeatTask.Time.TotalSeconds)
      {
        return DateTime.Now.Date + cyclicHeatTask.Time;
      }
    }
    else if (customHeatRequest.HeatTask1.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      if (DateTime.Now.TimeOfDay.TotalSeconds < customHeatRequest.HeatTask1.TimeOfDay.TotalSeconds)
      {
        return DateTime.Now.Date + customHeatRequest.HeatTask1.TimeOfDay;
      }
    }
    else if ((customHeatRequest.HeatTask1.Day.Equals(DateTime.Now.AddDays(1).Day) ||
              CheckIfNextDayIsHeatTask(cyclicHeatTask, todayDay)
             ))
    {
      // Check for tomorrow
      if (!(customHeatRequest.HeatTask1.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds) ||
          !(cyclicHeatTask.Time.TotalSeconds >
            DateTime.Now.TimeOfDay.TotalSeconds))
      {
        return null;
      }

      if (customHeatRequest.HeatTask1.TimeOfDay.TotalSeconds >
          cyclicHeatTask.Time.TotalSeconds)
      {
        return customHeatRequest.HeatTask1;
      }

      return DateTime.Now.Date + cyclicHeatTask.Time;
    }

    return null;
  }

  #region private methods

  private static bool CheckIfNextDayIsHeatTask(CyclicHeatTask cyclicHeatTask, int todayDay)
  {
    return cyclicHeatTask.CyclicHeatTaskDaysInWeeks.FirstOrDefault(i =>
      (todayDay == 6 ? i.DayId == 0 : i.DayId == todayDay + 1)) != null;
  }

  #endregion
}

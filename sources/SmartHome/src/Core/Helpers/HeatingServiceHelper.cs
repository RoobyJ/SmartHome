using System;
using System.Linq;
using Core.Entities;
using Core.Models;

namespace Core.Helpers;

public abstract class HeatingServiceHelper
{
  public static CheckClosestDateResult CheckWhichIsCloser(CyclicHeatTask cyclicHeatTask, HeatTask customHeatRequest)
  {
    var todayDay = (int)DateTime.Today.DayOfWeek + 1;

    if (customHeatRequest.Date.Date.Day.Equals(DateTime.Now.Day) &&
        cyclicHeatTask.CyclicHeatTaskDays.FirstOrDefault(i => i.Day == todayDay) != null)
    {
      // Check for today comparison
      if (customHeatRequest.Date.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds &&
          cyclicHeatTask.Time.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds)
      {
        if (customHeatRequest.Date.TimeOfDay.TotalSeconds < cyclicHeatTask.Time.TotalSeconds)
        {
          return new CheckClosestDateResult
          {
            IsCyclic = false,
            ClosestDate = customHeatRequest.Date,
            HeatTaskId = customHeatRequest.Id
          };
        }
        
        return new CheckClosestDateResult
        {
          IsCyclic = true,
          ClosestDate = DateTime.Now.Date + cyclicHeatTask.Time,
          HeatTaskId = cyclicHeatTask.Id
        };
      }
    }
    else if (!customHeatRequest.Date.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      // today
      if (DateTime.Now.TimeOfDay.TotalSeconds < cyclicHeatTask.Time.TotalSeconds)
      {
        return new CheckClosestDateResult
        {
          IsCyclic = true,
          ClosestDate = DateTime.Now.Date + cyclicHeatTask.Time,
          HeatTaskId = cyclicHeatTask.Id
        };
      }
    }
    else if (customHeatRequest.Date.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      if (DateTime.Now.TimeOfDay.TotalSeconds < customHeatRequest.Date.TimeOfDay.TotalSeconds)
      {
        return new CheckClosestDateResult
        {
          IsCyclic = false,
          ClosestDate = customHeatRequest.Date,
          HeatTaskId = customHeatRequest.Id
        };
      }
    }
    else if (customHeatRequest.Date.Day.Equals(DateTime.Now.AddDays(1).Day) ||
             CheckIfNextDayIsHeatTask(cyclicHeatTask, todayDay))
    {
      // Check for tomorrow
      if (!(customHeatRequest.Date.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds) ||
          !(cyclicHeatTask.Time.TotalSeconds >
            DateTime.Now.TimeOfDay.TotalSeconds))
      {
        return new CheckClosestDateResult
        {
          IsCyclic = false,
          ClosestDate = null,
          HeatTaskId = 0
        };
      }

      if (customHeatRequest.Date.TimeOfDay.TotalSeconds >
          cyclicHeatTask.Time.TotalSeconds)
      {
        return new CheckClosestDateResult
        {
          IsCyclic = false,
          ClosestDate = customHeatRequest.Date,
          HeatTaskId = customHeatRequest.Id
        };
      }

      return new CheckClosestDateResult
      {
        IsCyclic = true,
        ClosestDate = DateTime.Now.Date + cyclicHeatTask.Time,
        HeatTaskId = cyclicHeatTask.Id
      };
    }

    return new CheckClosestDateResult
    {
      IsCyclic = false,
      ClosestDate = null,
      HeatTaskId = 0
    };
  }

  #region private methods

  private static bool CheckIfNextDayIsHeatTask(CyclicHeatTask cyclicHeatTask, int todayDay)
  {
    return cyclicHeatTask.CyclicHeatTaskDays.FirstOrDefault(i =>
      todayDay == 6 ? i.Day == 0 : i.Day == todayDay + 1) != null;
  }

  #endregion
}

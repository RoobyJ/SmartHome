using System;
using System.Linq;
using Core.Entities;
using Core.Models;

namespace Core.Helpers;

public abstract class HeatingServiceHelper
{
  public static CheckClosestDateResult CheckWhichIsCloser(CyclicHeatTaskEntity cyclicHeatTaskEntity, HeatTaskEntity customHeatRequest)
  {
    var todayDay = (int)DateTime.Today.DayOfWeek + 1;

    if (customHeatRequest.Date.Date.Day.Equals(DateTime.Now.Day) &&
        cyclicHeatTaskEntity.CyclicHeatTaskDays.FirstOrDefault(i => i.Day == todayDay) != null)
    {
      // Check for today comparison
      if (customHeatRequest.Date.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds &&
          cyclicHeatTaskEntity.Time.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds)
      {
        if (customHeatRequest.Date.TimeOfDay.TotalSeconds < cyclicHeatTaskEntity.Time.TotalSeconds)
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
          ClosestDate = DateTime.Now.Date + cyclicHeatTaskEntity.Time,
          HeatTaskId = cyclicHeatTaskEntity.Id
        };
      }
    }
    else if (!customHeatRequest.Date.Date.Day.Equals(DateTime.Now.Day)
            )
    {
      // today
      if (DateTime.Now.TimeOfDay.TotalSeconds < cyclicHeatTaskEntity.Time.TotalSeconds)
      {
        return new CheckClosestDateResult
        {
          IsCyclic = true,
          ClosestDate = DateTime.Now.Date + cyclicHeatTaskEntity.Time,
          HeatTaskId = cyclicHeatTaskEntity.Id
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
             CheckIfNextDayIsHeatTask(cyclicHeatTaskEntity, todayDay))
    {
      // Check for tomorrow
      if (!(customHeatRequest.Date.TimeOfDay.TotalSeconds > DateTime.Now.TimeOfDay.TotalSeconds) ||
          !(cyclicHeatTaskEntity.Time.TotalSeconds >
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
          cyclicHeatTaskEntity.Time.TotalSeconds)
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
        ClosestDate = DateTime.Now.Date + cyclicHeatTaskEntity.Time,
        HeatTaskId = cyclicHeatTaskEntity.Id
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

  private static bool CheckIfNextDayIsHeatTask(CyclicHeatTaskEntity cyclicHeatTaskEntity, int todayDay)
  {
    return cyclicHeatTaskEntity.CyclicHeatTaskDays.FirstOrDefault(i =>
      todayDay == 6 ? i.Day == 0 : i.Day == todayDay + 1) != null;
  }

  #endregion
}

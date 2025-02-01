using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Core.Common;

namespace Core.Entities;

public sealed class CyclicHeatTask
{
  public int GarageId { get; set; }

  public TimeSpan Time { get; set; }

  public bool Active { get; set; }

  public ICollection<CyclicHeatTaskDay> CyclicHeatTaskDays { get; set; } = new List<CyclicHeatTaskDay>();

  public Garage Garage { get; set; } = null!;
  public int Id { get; set; }

  public DateTime GetClosestDateTimeFromCyclicHeatTask()
  {
    var currentDay = new DateOnly();
    var listOfHeatDays = this.CyclicHeatTaskDays.Select(i => i.Day).ToList();
    int? closestDay = null;
    foreach (var heatDay in listOfHeatDays)
    {
      closestDay ??= heatDay;
      if (heatDay - (int)currentDay.DayOfWeek < closestDay - (int)currentDay.DayOfWeek) closestDay = heatDay;
    }

    var daysCountDifference = closestDay!.Value - (int)currentDay.DayOfWeek;
    var date = currentDay.AddDays(daysCountDifference);
    return date.ToDateTime(TimeOnly.FromTimeSpan(this.Time));
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Core.Common;

namespace Core.Entities;

public sealed class CyclicHeatTaskEntity
{
  public int GarageId { get; set; }

  public TimeSpan Time { get; set; }

  public bool Active { get; set; }

  public ICollection<CyclicHeatTaskDayEntity> CyclicHeatTaskDays { get; set; } = new List<CyclicHeatTaskDayEntity>();

  public GarageEntity GarageEntity { get; set; } = null!;
  public int Id { get; set; }

  public DateTime GetClosestDateTimeFromCyclicHeatTask(DateTime? now = null)
  {
    var reference = now ?? DateTime.Now;
    var currentDayOfWeek = (int)reference.DayOfWeek;
    var listOfHeatDays = this.CyclicHeatTaskDays.Select(i => i.Day).ToList();

    DateTime? closest = null;
    foreach (var heatDay in listOfHeatDays)
    {
      var daysUntilTarget = heatDay - currentDayOfWeek;
      if (daysUntilTarget < 0) daysUntilTarget += 7;

      var candidate = reference.Date.AddDays(daysUntilTarget) + this.Time;
      if (candidate <= reference) candidate = candidate.AddDays(7);

      if (closest == null || candidate < closest) closest = candidate;
    }

    return closest ?? reference;
  }
}

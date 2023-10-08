using System;
using System.Collections.Generic;

namespace SmartHome.Core.Dtos;

public class CreateCyclicHeatTaskDto
{
  public TimeSpan Time { get; set; }
  public ICollection<DayOfWeek> DaysInWeekSelected { get; set; } = default!;
}

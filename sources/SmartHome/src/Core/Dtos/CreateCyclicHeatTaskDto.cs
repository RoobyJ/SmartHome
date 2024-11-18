using System;
using System.Collections.Generic;

namespace Core.Dtos;

public class CreateCyclicHeatTaskDto
{
  public TimeSpan Time { get; set; }
  public ICollection<DayOfWeek> DaysInWeekSelected { get; set; } = default!;
}

using System;
using System.Collections.Generic;

namespace Core.Dtos;

public class UpdateCyclicHeatTaskDto
{
  public int Id { get; init; }
  public TimeSpan Time { get; set; }
  public ICollection<DayOfWeek> DaysInWeekSelected { get; set; } = default!;
}

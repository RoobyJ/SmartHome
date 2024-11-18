using System.Collections.Generic;

namespace Core.Dtos;

public class CyclicHeatTaskDto : HeatTaskBase
{
  public string Time { get; set; } = null!;
  public ICollection<int> DaysInWeekSelected { get; set; } = null!;
}

using System.Collections.Generic;

namespace SmartHome.Core.Dtos;

public class CyclicHeatTaskDto
{
  public int Id { get; set; }
  public string Time { get; set; }
  public ICollection<int> DaysInWeekSelected { get; set; }
}

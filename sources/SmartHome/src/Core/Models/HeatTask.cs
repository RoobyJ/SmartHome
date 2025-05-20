using System;

namespace Core.Models;

public class HeatTask
{
  public int HeatTaskId { get; init; }
  public int GarageId { get; init; }
  public bool IsCyclic { get; init; }
  public DateTime? StartTime { get; set; }
  public DateTime EndTime { get; init; }
  public bool IsCurrentlyHeating { get; set; }
}

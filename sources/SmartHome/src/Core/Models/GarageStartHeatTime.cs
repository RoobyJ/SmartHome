using System;

namespace Core.Models;

public class GarageStartHeatTime
{
  public int GarageId { get; set; }
  public int HeatTaskId { get; set; }
  public bool IsCyclic { get; set; }
  public DateTime? StartHeatTime { get; set; }
}

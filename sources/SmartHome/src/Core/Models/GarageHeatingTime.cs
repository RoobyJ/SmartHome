using System;

namespace Core.Models;

public class GarageHeatingTime
{
  public int Id { get; init; }
  public int HeatTaskId { get; set; }
  public bool IsCyclic { get; set; }
  public DateTime StartTime { get; set; }
}

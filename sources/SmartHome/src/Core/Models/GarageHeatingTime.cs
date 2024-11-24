using System;

namespace Core.Models;

public class GarageHeatingTime
{
  public int Id { get; set; }
  public int HeatTaskId { get; set; }
  public bool IsCyclic { get; set; }
  public DateTime? HeatTime { get; set; }
}

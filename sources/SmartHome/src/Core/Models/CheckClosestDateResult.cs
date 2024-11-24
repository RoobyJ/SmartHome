using System;

namespace Core.Models;

public class CheckClosestDateResult
{
  public DateTime? ClosestDate { get; set; }
  public bool IsCyclic { get; set; }
  public int HeatTaskId { get; set; }
}

using System;

namespace SmartHome.TasksManager.Heater.Models;

public class GarageStartHeatTime
{
  public int Id { get; set; }
  
  public TimeSpan? StartHeatTime { get; set; }
}

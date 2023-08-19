using System;

namespace SmartHome.TasksManager.Core.Models;

public class GarageHeatingTimespan
{
  public int Id { get; set; }
  
  public TimeSpan? HeatTime { get; set; }
}

using System;

namespace SmartHome.Core.Models;

public class GarageHeatingTimespan
{
  public int Id { get; set; }

  public TimeSpan? HeatTime { get; set; }
}

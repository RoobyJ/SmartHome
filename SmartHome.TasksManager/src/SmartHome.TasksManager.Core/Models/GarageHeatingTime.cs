﻿using System;

namespace SmartHome.TasksManager.Core.Models;

public class GarageHeatingTime
{
  public int Id { get; set; }
  public TimeSpan? HeatTime { get; set; }
}

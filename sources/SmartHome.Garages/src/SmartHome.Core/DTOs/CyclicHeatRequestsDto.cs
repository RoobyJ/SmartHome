using System;

namespace SmartHome.Core.DTOs;

public class CyclicHeatRequestsDto
{
  public TimeSpan Sunday { get; set; }
  public TimeSpan Monday { get; set; }
  public TimeSpan Tuesday { get; set; }
  public TimeSpan Wednesday { get; set; }
  public TimeSpan Thursday { get; set; }
  public TimeSpan Friday { get; set; }
  public TimeSpan Saturday { get; set; }
}

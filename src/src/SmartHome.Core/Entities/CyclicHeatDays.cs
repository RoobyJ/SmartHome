using System;
using System.Collections.Generic;
using System.Net;

namespace SmartHome.Core.Entities;

public class CyclicHeatDays
{
  public TimeSpan? Sunday { get; set; }

  public TimeSpan? Monday { get; set; }

  public TimeSpan? Tuesday { get; set; }

  public TimeSpan? Wednesday { get; set; }

  public TimeSpan? Thursday { get; set; }

  public TimeSpan? Friday { get; set; }

  public TimeSpan? Saturday { get; set; }

  public List<TimeSpan?> ToList()
  {
    List<TimeSpan?> listOfHeatTimes = new()
    {
      this.Monday,
      this.Tuesday,
      this.Wednesday,
      this.Thursday,
      this.Friday,
      this.Saturday,
      this.Sunday
    };

    return listOfHeatTimes;
  }
}

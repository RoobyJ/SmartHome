using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.Core.Common;

namespace Core.Entities;

public class CyclicHeatTaskDay
{
  public int Day { get; set; }

  public int CyclicHeatTaskId { get; set; }

  public virtual CyclicHeatTask CyclicHeatTask { get; set; } = null!;
  public int Id { get; set; }
}

using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public class HeatTask
{
  public DateTime Date { get; set; }

  public int GarageId { get; set; }

  public bool Active { get; set; }

  public virtual Garage Garage { get; set; } = null!;
  public int Id { get; set; }
}

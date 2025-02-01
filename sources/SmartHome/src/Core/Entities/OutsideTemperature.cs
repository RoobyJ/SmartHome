using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public sealed class OutsideTemperature
{
  public DateTime Date { get; set; }

  public float Temperature { get; set; }

  public int GarageId { get; set; }

  public Garage Garage { get; set; } = null!;
  public int Id { get; set; }
}

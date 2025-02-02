using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public sealed class OutsideTemperatureEntity
{
  public DateTime Date { get; set; }

  public float Temperature { get; set; }

  public int GarageId { get; set; }

  public GarageEntity GarageEntity { get; set; } = null!;
  public int Id { get; set; }
}

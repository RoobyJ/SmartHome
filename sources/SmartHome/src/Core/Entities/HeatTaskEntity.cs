using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public class HeatTaskEntity
{
  public int Id { get; set; }
  public DateTime Date { get; set; }

  public int GarageId { get; set; }

  public bool Active { get; set; }

  public virtual GarageEntity GarageEntity { get; set; } = null!;
}

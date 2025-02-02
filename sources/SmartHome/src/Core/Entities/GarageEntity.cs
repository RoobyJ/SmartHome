using System.Collections.Generic;
using SmartHome.Core.Common;

namespace Core.Entities;

public class GarageEntity
{
  public string Name { get; set; } = null!;

  public string Ip { get; set; } = null!;

  public virtual ICollection<CyclicHeatTaskEntity> CyclicHeatTasks { get; set; } = new List<CyclicHeatTaskEntity>();

  public virtual ICollection<HeatTaskEntity> HeatTasks { get; set; } = new List<HeatTaskEntity>();

  public virtual ICollection<OutsideTemperatureEntity> OutsideTemperatures { get; set; } = new List<OutsideTemperatureEntity>();
  public int Id { get; set; }
}

using System.Collections.Generic;
using SmartHome.Core.Common;

namespace Core.Entities;

public class Garage : IEntity
{
  public string Name { get; set; } = null!;

  public string Ip { get; set; } = null!;

  public virtual ICollection<CyclicHeatTask> CyclicHeatTasks { get; set; } = new List<CyclicHeatTask>();

  public virtual ICollection<HeatTask> HeatTasks { get; set; } = new List<HeatTask>();

  public virtual ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
  public int Id { get; set; }
}

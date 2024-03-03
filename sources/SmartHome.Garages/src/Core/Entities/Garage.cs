using System.Collections.Generic;
using SmartHome.Core.Common;

namespace Core.Entities;

public partial class Garage : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Ip { get; set; }

    public virtual ICollection<CyclicHeatTask> CyclicHeatTasks { get; set; } = new List<CyclicHeatTask>();

    public virtual ICollection<HeatTask> HeatTasks { get; set; } = new List<HeatTask>();

    public virtual ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
}

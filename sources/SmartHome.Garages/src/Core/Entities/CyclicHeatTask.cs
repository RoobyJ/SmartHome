using System;
using System.Collections.Generic;
using SmartHome.Core.Common;

namespace Core.Entities;

public partial class CyclicHeatTask : IEntity
{
    public int Id { get; set; }

    public int GarageId { get; set; }

    public TimeSpan Time { get; set; }
    
    public bool Active { get; set; }

    public virtual ICollection<CyclicHeatTaskDay> CyclicHeatTaskDays { get; set; } = new List<CyclicHeatTaskDay>();

    public virtual Garage Garage { get; set; }
}

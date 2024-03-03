using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public partial class HeatTask : IEntity
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int GarageId { get; set; }
    
    public bool Active { get; set; }

    public virtual Garage Garage { get; set; }
}

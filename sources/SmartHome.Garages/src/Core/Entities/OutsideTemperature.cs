using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public partial class OutsideTemperature : IEntity
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public float Temperature { get; set; }

    public int GarageId { get; set; }

    public virtual Garage Garage { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SmartHome.webapi.Entities;

public partial class HeatRequest
{
    public int id { get; set; }

    public DateTime heatRequest1 { get; set; }

    public int garageId { get; set; }

    public virtual Garage garage { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SmartHome.webapi.Entities;

public partial class Garage
{
    public int id { get; set; }

    public virtual ICollection<HeatRequest> HeatRequests { get; set; } = new List<HeatRequest>();

    public virtual ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
}

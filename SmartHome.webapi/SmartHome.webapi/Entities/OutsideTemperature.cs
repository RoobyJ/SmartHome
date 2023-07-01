using System;
using System.Collections.Generic;

namespace SmartHome.webapi.Entities;

public partial class OutsideTemperature
{
    public int id { get; set; }

    public DateTime date { get; set; }

    public int temperature { get; set; }

    public int garageId { get; set; }

    public virtual Garage garage { get; set; } = null!;
}

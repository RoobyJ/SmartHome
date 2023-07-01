using System;
using System.Collections.Generic;

namespace SmartHome.webapi.Entities;

public partial class HeatingLog
{
    public int id { get; set; }

    public DateTime date { get; set; }

    public long? heatingTime { get; set; }
}

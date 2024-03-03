using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public partial class HeatLog : IEntity
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Info { get; set; }
}

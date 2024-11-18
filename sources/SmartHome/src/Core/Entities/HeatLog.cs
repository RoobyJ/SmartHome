using System;
using SmartHome.Core.Common;

namespace Core.Entities;

public class HeatLog : IEntity
{
  public DateTime Date { get; set; }

  public string? Info { get; set; }
  public int Id { get; set; }
}

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("HeatingLogs", Schema = "Garages")]
public class HeatingLog : IEntity
{
  public DateTime Date { get; set; }

  public string? Info { get; set; }

  [Key] public int Id { get; set; }
}

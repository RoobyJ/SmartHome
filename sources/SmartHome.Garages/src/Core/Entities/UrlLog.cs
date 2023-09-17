using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("UrlLogs", Schema = "Garages")]
public class UrlLog : IEntity
{
  public string Url { get; set; } = null!;

  public DateTime Date { get; set; }

  public int StatusCode { get; set; }

  public int RequestId { get; set; }

  [Key] public int Id { get; set; }
}

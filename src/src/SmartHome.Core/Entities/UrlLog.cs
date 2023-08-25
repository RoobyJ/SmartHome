using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Entities;

[Table("UrlLogs", Schema = "Heater")]
public partial class UrlLog : BaseEntity
{
  public string Url { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    public int StatusCode { get; set; }

    public string RequestId { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.webapi.Entities;

[Table("UrlLogs", Schema = "Heater")]
public partial class UrlLog
{
    [Key]
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public DateOnly Datetime { get; set; }

    public int StatusCode { get; set; }

    public string RequestId { get; set; } = null!;
}

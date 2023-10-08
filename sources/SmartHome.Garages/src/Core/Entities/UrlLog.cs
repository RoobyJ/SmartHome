using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("UrlLogs", Schema = "Garages")]
public class UrlLog: IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Url { get; set; }

    public DateTime Date { get; set; }

    public int StatusCode { get; set; }

    public int RequestId { get; set; }
}

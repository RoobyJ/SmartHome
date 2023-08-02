using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.webapi.Entities;

[Table("HeatingLogs", Schema = "Heater")]
public partial class HeatingLog
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    public long? HeatingTime { get; set; }
}

#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.TasksManager.Core.Entities;

namespace SmartHome.TasksManager.Heater.Entities;

[Table("HeatingLogs", Schema = "Heater")]
public partial class HeatingLog : BaseEntity
{
  public DateTime Date { get; set; }

    public string? Info { get; set; }
}

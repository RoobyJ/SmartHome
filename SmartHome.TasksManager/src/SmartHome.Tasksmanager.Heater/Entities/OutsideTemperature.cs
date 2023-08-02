﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.TasksManager.Core.Entities;
using SmartHome.webapi.Entities;

namespace SmartHome.TasksManager.Heater.Entities;

[Table("OutsideTemperatures", Schema = "Heater")]
public partial class OutsideTemperature : BaseEntity
{
  [Column(TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    public int Temperature { get; set; }

    public int GarageId { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("OutsideTemperatures")]
    public virtual Garage Garage { get; set; } = null!;
}

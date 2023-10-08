using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("HeatTasks", Schema = "Garages")]
public sealed class HeatTask: IEntity
{
    [Key]
    public int Id { get; set; }

    [Column("HeatTask")]
    public DateTime HeatTask1 { get; set; }

    public int GarageId { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("HeatTasks")]
    public Garage Garage { get; set; }
}

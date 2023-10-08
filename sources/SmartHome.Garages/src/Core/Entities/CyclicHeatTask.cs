using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("CyclicHeatTasks", Schema = "Garages")]
public sealed class CyclicHeatTask: IEntity
{
    [Key]
    public int Id { get; set; }

    public int GarageId { get; set; }

    public TimeSpan Time { get; set; }

    [InverseProperty("CyclicHeatTaskTime")]
    public ICollection<CyclicHeatTaskDaysInWeek> CyclicHeatTaskDaysInWeeks { get; set; } = new List<CyclicHeatTaskDaysInWeek>();

    [ForeignKey("GarageId")]
    [InverseProperty("CyclicHeatTasks")]
    public Garage Garage { get; set; }
}

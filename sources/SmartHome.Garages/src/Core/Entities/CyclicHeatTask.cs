using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("CyclicHeatTask", Schema = "Garages")]
public partial class CyclicHeatTask: IEntity
{
    [Key]
    public int Id { get; set; }

    public int GarageId { get; set; }

    public TimeOnly Time { get; set; }

    [InverseProperty("CyclicHeatTaskTime")]
    public virtual ICollection<CyclicHeatTaskDaysInWeek> CyclicHeatTaskDaysInWeeks { get; set; } = new List<CyclicHeatTaskDaysInWeek>();

    [ForeignKey("GarageId")]
    [InverseProperty("CyclicHeatTasks")]
    public virtual Garage Garage { get; set; }
}

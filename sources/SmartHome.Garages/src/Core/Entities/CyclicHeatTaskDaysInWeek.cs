using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("CyclicHeatTaskDaysInWeek", Schema = "Garages")]
public sealed class CyclicHeatTaskDaysInWeek: IEntity
{
    [Key]
    public int Id { get; set; }

    public int DayId { get; set; }

    public int CyclicHeatTaskTimeId { get; set; }

    [ForeignKey("CyclicHeatTaskTimeId")]
    [InverseProperty("CyclicHeatTaskDaysInWeeks")]
    public CyclicHeatTask CyclicHeatTaskTime { get; set; }

    [ForeignKey("DayId")]
    [InverseProperty("CyclicHeatTaskDaysInWeeks")]
    public DaysInWeek Day { get; set; }
}

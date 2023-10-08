using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("DaysInWeek", Schema = "Garages")]
public sealed class DaysInWeek: IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [InverseProperty("Day")]
    public ICollection<CyclicHeatTaskDaysInWeek> CyclicHeatTaskDaysInWeeks { get; set; } = new List<CyclicHeatTaskDaysInWeek>();
}

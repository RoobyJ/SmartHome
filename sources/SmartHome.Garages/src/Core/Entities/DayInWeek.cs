using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("DayInWeek", Schema = "Garages")]
public sealed class DayInWeek : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [InverseProperty("Day")]
    public ICollection<CyclicHeatTaskDaysInWeek> CyclicHeatTaskDaysInWeeks { get; set; } = new List<CyclicHeatTaskDaysInWeek>();
}

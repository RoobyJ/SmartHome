using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("CyclicHeatTaskDaysInWeek", Schema = "Garages")]
public sealed class CyclicHeatTaskDaysInWeek : IEntity
{
  public int DayId { get; set; }

  public int CyclicHeatTaskId { get; set; }

  [ForeignKey("CyclicHeatTaskId")]
  [InverseProperty("CyclicHeatTaskDaysInWeeks")]
  public CyclicHeatTask CyclicHeatTask { get; set; }

  [ForeignKey("DayId")]
  [InverseProperty("CyclicHeatTaskDaysInWeeks")]
  public DayInWeek Day { get; set; }

  [Key] public int Id { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;


namespace SmartHome.Core.Entities;

[Table("CyclicHeatRequests", Schema = "Garages")]
public partial class CyclicHeatRequest : IEntity
{
  [Key] public int Id { get; set; }

  public int GarageId { get; set; }

  public TimeSpan? Monday { get; set; }

  public TimeSpan? Tuesday { get; set; }

  public TimeSpan? Wednesday { get; set; }

  public TimeSpan? Thursday { get; set; }

  public TimeSpan? Friday { get; set; }

  public TimeSpan? Saturday { get; set; }

  public TimeSpan? Sunday { get; set; }

  [ForeignKey("GarageId")]
  [InverseProperty("CyclicHeatRequests")]
  public virtual Garage Garage { get; set; } = null!;

  public List<TimeSpan?> ToList()
  {
    List<TimeSpan?> listOfHeatTimes = new()
    {
      this.Sunday,
      this.Monday,
      this.Tuesday,
      this.Wednesday,
      this.Thursday,
      this.Friday,
      this.Saturday
    };

    return listOfHeatTimes;
  }
}

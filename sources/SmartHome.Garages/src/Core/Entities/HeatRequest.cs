using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("HeatRequests", Schema = "Garages")]
public class HeatRequest : IEntity
{
  [Column("HeatRequest")] public DateTime HeatRequest1 { get; set; }

  public int GarageId { get; set; }

  [ForeignKey("GarageId")]
  [InverseProperty("HeatRequests")]
  public virtual Garage Garage { get; set; } = null!;

  [Key] public int Id { get; set; }
}

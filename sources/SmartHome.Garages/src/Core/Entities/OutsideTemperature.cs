using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("OutsideTemperatures", Schema = "Garages")]
public class OutsideTemperature : IEntity
{
  public DateTime Date { get; set; }

  public int Temperature { get; set; }

  public int GarageId { get; set; }

  [ForeignKey("GarageId")]
  [InverseProperty("OutsideTemperatures")]
  public virtual Garage Garage { get; set; } = null!;

  [Key] public int Id { get; set; }
}

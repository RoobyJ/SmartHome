using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("OutsideTemperature", Schema = "Garages")]
public sealed class OutsideTemperature : IEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public float Temperature { get; set; }

    public int GarageId { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("OutsideTemperatures")]
    public Garage Garage { get; set; }
}

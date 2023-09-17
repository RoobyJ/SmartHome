using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Entities;

[Table("OutsideTemperatures", Schema = "Garages")]
public partial class OutsideTemperature : IEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Temperature { get; set; }

    public int GarageId { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("OutsideTemperatures")]
    public virtual Garage Garage { get; set; } = null!;
}

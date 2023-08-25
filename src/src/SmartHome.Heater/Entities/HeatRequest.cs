using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;

namespace SmartHome.Heater.Entities;

[Table("HeatRequests", Schema = "Heater")]
public partial class HeatRequest : BaseEntity
{
  [Column("HeatRequest")]
    public DateTime HeatRequest1 { get; set; }

    public int GarageId { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("HeatRequests")]
    public virtual Garage Garage { get; set; } = null!;
}

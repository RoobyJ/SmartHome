using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.webapi.Entities;

[Table("HeatRequests", Schema = "Heater")]
public partial class HeatRequest
{
    [Key]
    public int Id { get; set; }

    [Column("HeatRequest")]
    public DateTime HeatRequest1 { get; set; }

    public int GarageId { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("HeatRequests")]
    public virtual Garage Garage { get; set; } = null!;
}

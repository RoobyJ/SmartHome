using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("OutsideTemperatures", Schema = "Garages")]
public sealed class OutsideTemperature: IEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Temperature { get; set; }

    public int GarageId { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("OutsideTemperatures")]
    public Garage Garage { get; set; }
}

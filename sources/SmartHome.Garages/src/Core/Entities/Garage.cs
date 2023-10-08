using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("Garages", Schema = "Garages")]
public sealed class Garage: IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Ip { get; set; }

    [InverseProperty("Garage")]
    public ICollection<CyclicHeatTask> CyclicHeatTasks { get; set; } = new List<CyclicHeatTask>();

    [InverseProperty("Garage")]
    public ICollection<HeatTask> HeatTasks { get; set; } = new List<HeatTask>();

    [InverseProperty("Garage")]
    public ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
}

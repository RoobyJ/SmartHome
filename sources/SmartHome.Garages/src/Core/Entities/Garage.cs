using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("Garages", Schema = "Garages")]
public partial class Garage: IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Ip { get; set; }

    [InverseProperty("Garage")]
    public virtual ICollection<CyclicHeatTask> CyclicHeatTasks { get; set; } = new List<CyclicHeatTask>();

    [InverseProperty("Garage")]
    public virtual ICollection<HeatTask> HeatTasks { get; set; } = new List<HeatTask>();

    [InverseProperty("Garage")]
    public virtual ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
}

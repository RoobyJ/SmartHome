using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;

namespace SmartHome.Heater.Entities;

[Table("Garages", Schema = "Heater")]
public partial class Garage : BaseEntity
{
  public string Name { get; set; } = null!;

    public string Ip { get; set; } = null!;

    [InverseProperty("Garage")]
    public virtual ICollection<HeatRequest> HeatRequests { get; set; } = new List<HeatRequest>();

    [InverseProperty("Garage")]
    public virtual ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
}

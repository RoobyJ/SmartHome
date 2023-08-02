using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.TasksManager.Core.Entities;

namespace SmartHome.TasksManager.Heater.Entities;

[Table("Garages", Schema = "Heater")]
public partial class Garage : BaseEntity
{
  public string Ip { get; set; } = null!;

    [InverseProperty("Garage")]
    public virtual ICollection<HeatRequest> HeatRequests { get; set; } = new List<HeatRequest>();

    [InverseProperty("Garage")]
    public virtual ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
}

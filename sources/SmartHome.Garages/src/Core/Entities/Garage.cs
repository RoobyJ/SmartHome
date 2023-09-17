using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("Garages", Schema = "Garages")]
public class Garage : IEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Ip { get; set; } = null!;

    [InverseProperty("Garage")]
    public virtual ICollection<CyclicHeatRequest> CyclicHeatRequests { get; set; } = new List<CyclicHeatRequest>();

    [InverseProperty("Garage")]
    public virtual ICollection<HeatRequest> HeatRequests { get; set; } = new List<HeatRequest>();

    [InverseProperty("Garage")]
    public virtual ICollection<OutsideTemperature> OutsideTemperatures { get; set; } = new List<OutsideTemperature>();
}

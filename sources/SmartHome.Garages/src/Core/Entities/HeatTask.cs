using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("HeatTask", Schema = "Garages")]
public sealed class HeatTask : IEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int GarageId { get; set; }
    
    public bool Active { get; set; }

    [ForeignKey("GarageId")]
    [InverseProperty("HeatTasks")]
    public Garage Garage { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("HeatLog", Schema = "Garages")]
public class HeatLog : IEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Info { get; set; }
}

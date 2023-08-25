using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Entities;

public abstract class BaseEntity
{
  [Key] public int Id { get; set; }
}

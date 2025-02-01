using System;

namespace Core.Models;

public record GarageHeaterStatus
{
  public int GarageId { get; set; }
  public DateTime EndTime { get; set; }
}

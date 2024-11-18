namespace SmartHome.Core.Models;

public record GarageHeaterStatus
{
  public int Id { get; set; }
  public bool HeatingStatus { get; set; }
}

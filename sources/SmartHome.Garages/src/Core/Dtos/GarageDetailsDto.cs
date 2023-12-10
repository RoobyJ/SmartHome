namespace SmartHome.Core.Dtos;

public class GarageDetailsDto
{
  public int Id { get; set; }

  public string Name { get; set; } = default!;

  public bool? HeaterStatus { get; set; }

  public float? Temperature { get; set; }
}

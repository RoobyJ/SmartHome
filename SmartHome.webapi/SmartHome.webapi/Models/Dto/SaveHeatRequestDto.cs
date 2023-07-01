namespace SmartHome.webapi.Models;

public class SaveHeatRequestDto
{
    public DateTime TimeToHeat { get; set; }
    public int Cyclic { get; set; }
}
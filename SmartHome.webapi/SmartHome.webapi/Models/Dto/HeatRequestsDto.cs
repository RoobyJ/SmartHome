namespace SmartHome.webapi.Models;

public class HeatRequestDto
{
    public int Id { get; set; }

    public DateTime Time { get; set; }
    
    public int garageId { get; set; }
}
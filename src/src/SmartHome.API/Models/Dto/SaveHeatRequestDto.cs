namespace SmartHome.webapi.Models;

public class SaveHeatRequestDto
{
    public DateTime EndHeatTime { get; set; }
    public int Cyclic { get; set; }
}

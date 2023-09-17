using System;

namespace SmartHome.Core.DTOs;

public class SaveHeatRequestDto
{
    public DateTime TimeToHeat { get; set; }
    public int Cyclic { get; set; }
}

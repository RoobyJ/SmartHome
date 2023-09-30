using System;

namespace SmartHome.Core.DTOs;

public class SaveHeatRequestDto
{
    public DateTime EndHeatTime { get; set; }
    public int Cyclic { get; set; }
}

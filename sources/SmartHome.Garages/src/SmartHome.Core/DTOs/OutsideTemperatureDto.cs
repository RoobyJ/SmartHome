using System;

namespace SmartHome.Core.DTOs;

public class OutsideTemperatureDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Temperature { get; set; }

    public int GarageId { get; set; }

}

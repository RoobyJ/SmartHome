using SmartHome.Core.Models;

namespace SmartHome.Core.DTOs;

public class CyclicHeatRequestsDto
{
  public int GarageId { get; set; }
  public CyclicHeatRequests Requests { get; set; }
}

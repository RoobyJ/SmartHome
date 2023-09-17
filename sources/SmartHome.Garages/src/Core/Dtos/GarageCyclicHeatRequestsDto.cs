using SmartHome.Core.Models;

namespace SmartHome.Core.Dtos;

public class GarageCyclicHeatRequestsDto
{
  public int GarageId { get; set; }

  public CyclicHeatRequests Requests { get; set; }
}

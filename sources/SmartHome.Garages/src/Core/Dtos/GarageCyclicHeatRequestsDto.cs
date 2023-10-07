using System.Collections.Generic;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Core.Dtos;

public class GarageCyclicHeatRequestsDto
{
  public CyclicHeatRequest Requests { get; set; }
}

using SmartHome.Core.Dtos;
using SmartHome.Core.DTOs;
using SmartHome.Core.Entities;

namespace SmartHome.webapi.Mappers;

public abstract class GarageConverters
{
  public static GarageDetailsDto GarageToGarageDetailsDto(Garage garage, GarageHeaterStatusDto heaterStatusResponse)
  {
    return new GarageDetailsDto()
    {
      Id = garage.Id, Name = garage.Name, HeaterStatus = heaterStatusResponse.HeaterStatus
    };
  }
}

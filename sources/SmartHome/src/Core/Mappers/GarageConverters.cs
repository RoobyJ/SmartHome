using System.Linq;
using Core.Dtos;
using Core.Entities;

namespace Core.Mappers;

public static class GarageConverters
{
  public static CyclicHeatTaskDto CyclicHeatTaskToDto(this CyclicHeatTaskEntity heatTaskEntity)
  {
    return new CyclicHeatTaskDto
    {
      Id = heatTaskEntity.Id,
      Time = heatTaskEntity.Time.ToString(),
      DaysInWeekSelected = heatTaskEntity.CyclicHeatTaskDays.Select(i => i.Day).ToList(),
      IsActive = heatTaskEntity.Active,
      GarageId = heatTaskEntity.GarageId
    };
  }

  public static HeatTaskDto HeatTaskToDto(this HeatTaskEntity heatTaskEntity)
  {
    return new HeatTaskDto
    {
      Id = heatTaskEntity.Id, GarageId = heatTaskEntity.GarageId, IsActive = heatTaskEntity.Active, Date = heatTaskEntity.Date,
    };
  }

  public static GarageDetailsDto GarageToGarageDetailsDto(this GarageEntity garageEntity,
    GarageHeaterStatusDto? heaterStatusResponse, TemperatureDto? temperatureResponse)
  {
    return new GarageDetailsDto
    {
      Id = garageEntity.Id,
      Name = garageEntity.Name,
      HeaterStatus = heaterStatusResponse?.HeatingStatus,
      Temperature = temperatureResponse?.Temperature,
    };
  }
}

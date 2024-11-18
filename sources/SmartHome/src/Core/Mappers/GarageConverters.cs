using System.Linq;
using Core.Dtos;
using Core.Entities;

namespace Core.Mappers;

public static class GarageConverters
{
  public static CyclicHeatTaskDto CyclicHeatTaskToDto(this CyclicHeatTask heatTask)
  {
    return new CyclicHeatTaskDto
    {
      Id = heatTask.Id,
      Time = heatTask.Time.ToString(),
      DaysInWeekSelected = heatTask.CyclicHeatTaskDays.Select(i => i.Day).ToList(),
      IsActive = heatTask.Active,
      GarageId = heatTask.GarageId
    };
  }

  public static HeatTaskDto HeatTaskToDto(this HeatTask heatTask)
  {
    return new HeatTaskDto
    {
      Id = heatTask.Id, GarageId = heatTask.GarageId, IsActive = heatTask.Active, Date = heatTask.Date,
    };
  }

  public static GarageDetailsDto GarageToGarageDetailsDto(this Garage garage,
    GarageHeaterStatusDto? heaterStatusResponse, TemperatureDto? temperatureResponse)
  {
    return new GarageDetailsDto
    {
      Id = garage.Id,
      Name = garage.Name,
      HeaterStatus = heaterStatusResponse?.HeatingStatus,
      Temperature = temperatureResponse?.Temperature,
    };
  }
}

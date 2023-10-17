using System.Linq;
using SmartHome.Core.Dtos;
using SmartHome.Core.DTos;
using SmartHome.Core.DTOs;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Mappers;

public abstract class GarageConverters
{
  public static CyclicHeatTaskDto CyclicHeatTaskToDto(CyclicHeatTask heatTask)
  {
    return new CyclicHeatTaskDto()
    {
      Id = heatTask.Id,
      Time = heatTask.Time.ToString(),
      DaysInWeekSelected = heatTask.CyclicHeatTaskDaysInWeeks.Select(i => i.DayId).ToList()
    };
  }

  public static GarageDetailsDto GarageToGarageDetailsDto(Garage garage, GarageHeaterStatusDto heaterStatusResponse,
    TemperatureDto temperatureResponse)
  {
    return new GarageDetailsDto()
    {
      Id = garage.Id,
      Name = garage.Name,
      HeaterStatus = heaterStatusResponse.HeaterStatus,
      Temperature = temperatureResponse.Temperature
    };
  }
}

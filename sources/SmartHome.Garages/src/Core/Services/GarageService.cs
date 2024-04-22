using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Interfaces;
using SmartHome.Core.Dtos;
using Core.Entities;
using Core.Helpers;
using Core.Mappers;

namespace Core.Services;

public class GarageService(
  IGarageRepository garageRepository,
  IOutsideTemperatureRepository outsideTemperatureRepository,
  IGarageClient garageClient)
  : IGarageService
{
  public async Task<ICollection<GarageDetailsDto>> GetGarages(CancellationToken ct)
  {
    var garages = await garageRepository.GetGarages(ct);
    var result = new List<GarageDetailsDto>();

    foreach (var garage in garages)
    {
      var heaterStatus = await garageClient.GetHeaterStatus(garage.Ip, ct);
      var temperature = await garageClient.GetGarageTemperature(garage.Ip, ct);
      result.Add(GarageConverters.GarageToGarageDetailsDto(garage, heaterStatus, temperature));
    }

    return result;
  }

  public async Task<ICollection<OutsideTemperature>> GetTemperatures(int id, int days, CancellationToken ct)
  {
    return await outsideTemperatureRepository.GetTemperatures(id, days, ct);
  }

  public async Task<Garage?> GetGarageById(int id, CancellationToken ct)
  {
    return await garageRepository.GetGarage(id, ct);
  }

  public async Task<bool?> GetGarageHeaterStatus(int garageId, CancellationToken ct)
  {
    var garage = await garageRepository.GetGarage(garageId, ct);
    var result = await garageClient.GetHeaterStatus(garage!.Ip, ct);
    return result?.HeaterStatus ?? null;
  }
}

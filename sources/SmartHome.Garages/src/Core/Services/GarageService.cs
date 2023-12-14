using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Mappers;

namespace SmartHome.Core.Services;

public class GarageService : IGarageService
{
  private readonly IGarageRepository _garageRepository;
  private readonly IOutsideTemperatureRepository<OutsideTemperature> _outsideTemperatureRepository;
  private readonly IGarageClient garageClient;

  public GarageService(IGarageRepository garageRepository, IOutsideTemperatureRepository<OutsideTemperature>
    outsideTemperatureRepository, IGarageClient garageClient)
  {
    _garageRepository = garageRepository;
    _outsideTemperatureRepository = outsideTemperatureRepository;
    this.garageClient = garageClient;
  }

  public async Task<ICollection<GarageDetailsDto>> GetGarages(CancellationToken cancellationToken)
  {
    var garages = await _garageRepository.Get(new GarageQueryOptions()).ToListAsync(cancellationToken);
    var result = new List<GarageDetailsDto>();

    foreach (var garage in garages)
    {
      var heaterStatus = await garageClient.GetHeaterStatus(garage.Ip, cancellationToken);
      var temperature = await garageClient.GetGarageTemperature(garage.Ip, cancellationToken);
      result.Add(GarageConverters.GarageToGarageDetailsDto(garage, heaterStatus, temperature));
    }

    return result;
  }

  public async Task<List<OutsideTemperature>> GetTemperatures(int id, int days, CancellationToken ct)
  {
    return await _outsideTemperatureRepository.Get(new OutsideTemperatureQueryOptions())
      .Where(i => i.GarageId == id).Take(days * 288).ToListAsync(ct);
  }

  public async Task<Garage> GetGarageById(int id, CancellationToken ct)
  {
    return await _garageRepository.Get(new GarageQueryOptions()).Where(i => i.Id == id).FirstAsync(ct);
  }
}

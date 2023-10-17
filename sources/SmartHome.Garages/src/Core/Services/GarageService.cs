using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Mappers;

namespace SmartHome.Core.Services;

public class GarageService : IGarageService
{
  private readonly IGarageRepository _garageRepository;
  private readonly IOutsideTemperatureRepository<OutsideTemperature> _outsideTemperatureRepository;
  private readonly ILogger<GarageService> _logger;

  public GarageService(ILogger<GarageService> logger, IGarageRepository garageRepository, IOutsideTemperatureRepository<OutsideTemperature>
    outsideTemperatureRepository)
  {
    _logger = logger;
    _garageRepository = garageRepository;
    _outsideTemperatureRepository = outsideTemperatureRepository;
  }
  
  public async Task<ICollection<GarageDetailsDto>> GetGarages(CancellationToken cancellationToken)
  {
    var garages = await this._garageRepository.Get(new GarageQueryOptions()).ToListAsync(cancellationToken);
    var result = new List<GarageDetailsDto>();

    foreach (var garage in garages)
    {
      try
      {
        var heaterStatus = await GarageClient.GetHeaterStatus(garage.Ip);
        var temperature = await GarageClient.GetGarageTemperature(garage.Ip);
        result.Add(GarageConverters.GarageToGarageDetailsDto(garage, heaterStatus, temperature));
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"{nameof(GarageService)}.{nameof(GetGarages)} threw an exception.");
      }
    }

    return result;
  }

  public async Task<List<OutsideTemperature>> GetTemperatures(int id, int days, CancellationToken ct)
  {
    return await this._outsideTemperatureRepository.Get(new OutsideTemperatureQueryOptions())
      .Where(i => i.GarageId == id)
      .Where(i => i.Date.Second > DateTime.Now.AddDays(-1*days).Second).ToListAsync(ct);
  }

  public async Task<Garage> GetGarageById(int id, CancellationToken ct)
  {
    return await this._garageRepository.Get(new GarageQueryOptions()).Where(i => i.Id == id).FirstAsync(ct);
  }
}

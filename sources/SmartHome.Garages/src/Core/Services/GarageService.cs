using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.webapi.Mappers;

namespace SmartHome.Core.Services;

public class GarageService : IGarageService
{
  private readonly IGarageRepository _garageRepository;
  private readonly ILogger<GarageService> _logger;
  
  public GarageService(ILogger<GarageService> logger, IGarageRepository garageRepository)
  {
    _logger = logger;
    _garageRepository = garageRepository;
  }

  public Task SaveHeatTimeRequest(int id, HeatRequestDto heatRequest)
  {
    throw new System.NotImplementedException();
  }

  public Task<List<HeatRequest>> GetHeatTimeRequests(int id)
  {
    throw new System.NotImplementedException();
  }

  public async Task<ICollection<GarageDetailsDto>> GetGarages(CancellationToken cancellationToken)
  {
    var garages = await this._garageRepository.Get(new GarageQueryOptions()).ToListAsync(cancellationToken);
    var result = new List<GarageDetailsDto>();
    
    foreach (var garage in garages)
    {
      var response = await GarageClient.GetHeaterStatus(garage.Ip);
      result.Add(GarageConverters.GarageToGarageDetailsDto(garage, response));
    }
    
    return result;
  }

  public Task<List<OutsideTemperature>> GetTemperatures(int id)
  {
    throw new System.NotImplementedException();
  }

  public Task<Garage> GetGarageById(int id)
  {
    throw new System.NotImplementedException();
  }

  public Task CreateOrUpdateCyclicHeatRequests(int id, CyclicHeatRequestsDto requestsDto)
  {
    throw new System.NotImplementedException();
  }

  public Task<CyclicHeatRequest> GetCyclicHeatRequests(int id)
  {
    throw new System.NotImplementedException();
  }
}

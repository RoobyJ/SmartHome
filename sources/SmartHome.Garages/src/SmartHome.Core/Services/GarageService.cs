using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.DTOs;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Core.Services;

public class GarageService
{
  private readonly ILogger<GarageService> _logger;
  private readonly IGarageRepository _context;
  private readonly IServiceLocator _serviceScopeFactoryLocator;

  public GarageService(ILogger<GarageService> logger, IGarageRepository context,
    IServiceLocator serviceScopeFactoryLocator)
  {
    _logger = logger;
    _context = context;
    _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
  }

  public async Task SaveHeatTimeRequest(int id, HeatRequestDto request, CancellationToken cancellationToken)
  {
    var repository = GetRepository<IHeatingRequestRepository>();

    var heatRequest = new HeatRequest() { GarageId = id, HeatRequest1 = request.Time };

    await repository.AddAsync(heatRequest, cancellationToken);
  }

  public async Task<List<HeatRequest>> GetHeatTimeRequests(int id, CancellationToken cancellationToken)
  {
    var repository = GetRepository<IHeatingRequestRepository>();

    var heatTimeRequests = await repository.Get(new HeatingRequestQueryOptions() { AsNoTracking = true })
      .Where(i => i.GarageId == id).ToListAsync(cancellationToken);

    return heatTimeRequests;
  }

  public async Task<List<Garage>> GetGarages()
  {
    var repository = GetRepository<IGarageRepository>();

    var garages = await repository.Get(new GarageQueryOptions() { AsNoTracking = true }).ToListAsync();

    return garages;
  }

  public async Task<List<OutsideTemperature>> GetTemperatures(int id, CancellationToken cancellationToken)
  {
    var repository = GetRepository<IOutsideTemperatureRepository>();

    var temperatures = await repository.Get(new OutsideTemperatureQueryOptions() { AsNoTracking = true })
      .Where(i => i.GarageId == id).ToListAsync(cancellationToken);

    return temperatures;
  }

  public async Task<Garage> GetGarageById(int id, CancellationToken cancellationToken)
  {
    var repository = GetRepository<IGarageRepository>();

    var garage = await repository.Get(new GarageQueryOptions() { AsNoTracking = true }).FirstAsync(i => i.Id == id, cancellationToken);

    return garage;
  }

  public async Task CreateOrUpdateCyclicRequests(int id, CyclicHeatRequests requests, CancellationToken cancellationToken)
  {
    var repository = GetRepository<ICyclicHeatingRequestRepository>();

    var newCyclicRequests = new CyclicHeatRequest()
    {
      GarageId = id,
      Monday = requests.Monday,
      Tuesday = requests.Tuesday,
      Wednesday = requests.Wednesday,
      Thursday = requests.Thursday,
      Friday = requests.Friday,
      Saturday = requests.Saturday,
      Sunday = requests.Sunday
    };
    
    await repository.AddAsync(newCyclicRequests, cancellationToken);
  }

  #region private methods

  private T GetRepository<T>()
  {
    var scope = _serviceScopeFactoryLocator.CreateScope();
    return scope.ServiceProvider.GetService<T>();
  }

  #endregion
}

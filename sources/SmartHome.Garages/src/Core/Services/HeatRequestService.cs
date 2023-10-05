using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Interfaces;

namespace SmartHome.Core.Services;

public class HeatRequestService : IHeatRequestService
{
  private readonly ICyclicHeatingRequestRepository<CyclicHeatRequest> _cyclicHeatingRequestRepository;
  private readonly IHeatRequestRepository<HeatRequest> _heatRequestRepository;

  public HeatRequestService(ICyclicHeatingRequestRepository<CyclicHeatRequest> cyclicHeatingRequestRepository,
    IHeatRequestRepository<HeatRequest> heatRequestRepository)
  {
    _cyclicHeatingRequestRepository = cyclicHeatingRequestRepository;
    _heatRequestRepository = heatRequestRepository;
  }

  public async Task SaveHeatTimeRequest(int id, HeatRequestDto heatRequest, CancellationToken ct)
  {
    var heatTimeRequest = new HeatRequest() { GarageId = id, HeatRequest1 = heatRequest.Date };

    await this._heatRequestRepository.AddAsync(heatTimeRequest, ct);
    await this._heatRequestRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task<List<HeatRequest>> GetHeatTimeRequests(int id, CancellationToken ct)
  {
    return await this._heatRequestRepository.Get(new HeatRequestQueryOptions()).Where(i => i.GarageId == id)
      .ToListAsync(ct);
  }

  public async Task DeleteHeatTimeRequest(int garageId, int requestId, CancellationToken ct)
  {
    var request = await this._heatRequestRepository.Get(new HeatRequestQueryOptions())
      .Where(i => i.GarageId == garageId && i.Id == requestId).FirstAsync(ct);
    await this._heatRequestRepository.DeleteAsync(request, ct);
    await this._heatRequestRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task CreateCyclicHeatRequest(int id, CyclicHeatRequestsDto request, CancellationToken ct)
  {
    var entity = new CyclicHeatRequest()
    {
      GarageId = id,
      Monday = request.Monday,
      Tuesday = request.Tuesday,
      Wednesday = request.Wednesday,
      Thursday = request.Thursday,
      Friday = request.Friday,
      Saturday = request.Saturday,
      Sunday = request.Sunday
    };

    await this._cyclicHeatingRequestRepository.AddAsync(entity, ct);
    await this._cyclicHeatingRequestRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task UpdateCyclicHeatRequest(int id, CyclicHeatRequestsDto request, CancellationToken ct)
  {
    var entity = new CyclicHeatRequest()
    {
      GarageId = id,
      Monday = request.Monday,
      Tuesday = request.Tuesday,
      Wednesday = request.Wednesday,
      Thursday = request.Thursday,
      Friday = request.Friday,
      Saturday = request.Saturday,
      Sunday = request.Sunday
    };

    await this._cyclicHeatingRequestRepository.UpdateAsync(entity, ct);
    await this._cyclicHeatingRequestRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task<CyclicHeatRequest> GetCyclicHeatRequests(int id, CancellationToken ct)
  {
    return await this._cyclicHeatingRequestRepository.Get(new CyclicHeatingRequestQueryOptions())
      .Where(i => i.GarageId == id).FirstAsync(ct);
  }
}

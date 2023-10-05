using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Interfaces;

public interface IHeatRequestService
{
  public Task SaveHeatTimeRequest(int id, HeatRequestDto heatRequest, CancellationToken ct);
  public Task<List<HeatRequest>> GetHeatTimeRequests(int id, CancellationToken ct);
  public Task CreateCyclicHeatRequest(int id, CyclicHeatRequestsDto requestsDto, CancellationToken ct);
  public Task UpdateCyclicHeatRequest(int id, CyclicHeatRequestsDto requestsDto, CancellationToken ct);
  public Task<CyclicHeatRequest> GetCyclicHeatRequests(int id, CancellationToken ct);
  public Task DeleteHeatTimeRequest(int garageId, int requestId, CancellationToken ct);
}

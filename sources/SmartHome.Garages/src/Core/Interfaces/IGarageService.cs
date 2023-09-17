using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Interfaces;

public interface IGarageService
{
  public Task SaveHeatTimeRequest(int id, HeatRequestDto heatRequest);
  public Task<List<HeatRequest>> GetHeatTimeRequests(int id);
  public Task<List<Garage>> GetGarages();
  public Task<List<OutsideTemperature>> GetTemperatures(int id);
  public Task<Garage> GetGarageById(int id);
  public Task CreateOrUpdateCyclicHeatRequests(int id, CyclicHeatRequestsDto requestsDto);
  public Task<CyclicHeatRequest> GetCyclicHeatRequests(int id);
}

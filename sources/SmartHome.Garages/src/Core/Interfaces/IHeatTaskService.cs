using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;

namespace Core.Interfaces;

public interface IHeatTaskService
{
  public Task SaveHeatTimeTask(int id, CreateHeatTaskDto heatTask, CancellationToken ct);
  public Task<ICollection<HeatTask>> GetHeatTimeTasks(int id, CancellationToken ct);
  public Task UpdateHeatTask(int id, HeatTaskDto tasksDto, CancellationToken ct);
  public Task CreateCyclicHeatTask(int id, CreateCyclicHeatTaskDto taskDto, CancellationToken ct);
  public Task UpdateCyclicHeatTask(int id, UpdateCyclicHeatTaskDto requestsDto, CancellationToken ct);
  public Task<ICollection<CyclicHeatTask>> GetCyclicHeatTasks(int id, CancellationToken ct);
  public Task DeleteHeatTimeTask(int garageId, int requestId, CancellationToken ct);
  public Task DeleteCyclicHeatTask(int garageId, int requestId, CancellationToken ct);
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces;

public interface IHeatTaskService
{
  Task SaveHeatTimeTask(int id, CreateHeatTaskDto heatTask, CancellationToken ct);
  Task<ICollection<HeatTaskEntity>> GetHeatTimeTasks(int id, CancellationToken ct);
  Task UpdateHeatTask(int id, HeatTaskDto tasksDto, CancellationToken ct);
  Task CreateCyclicHeatTask(int id, CreateCyclicHeatTaskDto taskDto, CancellationToken ct);
  Task UpdateCyclicHeatTask(int id, UpdateCyclicHeatTaskDto requestsDto, CancellationToken ct);
  Task<ICollection<CyclicHeatTaskEntity>> GetCyclicHeatTasks(int id, CancellationToken ct);
  Task DeleteHeatTimeTask(int garageId, int requestId, CancellationToken ct);
  Task DeleteCyclicHeatTask(int garageId, int requestId, CancellationToken ct);
  Task SetHeatTaskActive(SetHeatTaskActiveDto data, CancellationToken ct);
}

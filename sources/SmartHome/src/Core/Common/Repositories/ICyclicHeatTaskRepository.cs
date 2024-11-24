using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface ICyclicHeatTaskRepository
{
  Task<ICollection<CyclicHeatTask>> GetCyclicHeatTasks(int garageId, CancellationToken ct);
  Task<CyclicHeatTask> GetCyclicHeatTask(int taskId, CancellationToken ct);
  Task<ICollection<CyclicHeatTask>> GetActiveCyclicHeatTasks(int garageId, CancellationToken ct);
  Task AddCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);

  Task UpdateCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);

  Task DeleteCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);
  Task SetHeatTaskActive(int id, bool active, CancellationToken ct = default);
}

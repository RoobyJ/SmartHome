using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface ICyclicHeatTaskRepository
{
  Task<ICollection<CyclicHeatTaskEntity>> GetCyclicHeatTasks(int garageId, CancellationToken ct);
  Task<CyclicHeatTaskEntity> GetCyclicHeatTask(int taskId, CancellationToken ct);
  Task<ICollection<CyclicHeatTaskEntity>> GetActiveCyclicHeatTasks(int garageId, CancellationToken ct);
  Task AddCyclicHeatTask(CyclicHeatTaskEntity entity, CancellationToken ct = default);

  Task UpdateCyclicHeatTask(CyclicHeatTaskEntity entity, CancellationToken ct = default);

  Task DeleteCyclicHeatTask(CyclicHeatTaskEntity entity, CancellationToken ct = default);
  Task SetHeatTaskActive(int id, bool active, CancellationToken ct = default);
  void ClearTrackedEntities();
}

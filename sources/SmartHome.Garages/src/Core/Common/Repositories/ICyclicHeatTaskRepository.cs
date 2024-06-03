using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface ICyclicHeatTaskRepository
{
  Task<ICollection<CyclicHeatTask>> GetCyclicHeatTasks(int garageId, CancellationToken ct);
  Task<CyclicHeatTask> GetCyclicHeatTask(int taskId, CancellationToken ct);
  Task<HeatTask> GetHeatTask(int taskId, CancellationToken ct);
  Task AddCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);

  Task UpdateCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);

  Task DeleteCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);
}

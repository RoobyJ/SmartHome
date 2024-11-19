using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IHeatTaskRepository
{
  Task<HeatTask> GetHeatTask(int garageId, CancellationToken ct);
  Task<ICollection<HeatTask>> GetHeatTasks(int garageId, CancellationToken ct);
  Task UpdateHeatTask(HeatTask heatTask, CancellationToken ct);
  Task AddHeatTask(HeatTask heatTask, CancellationToken ct);
  Task DeleteHeatTask(int heatTaskId, int garageId, CancellationToken ct);
  Task SetHeatTaskActive(int id, bool active, CancellationToken ct = default);
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IHeatTaskRepository
{
  Task<HeatTaskEntity> GetHeatTask(int garageId, CancellationToken ct);
  Task<ICollection<HeatTaskEntity>> GetHeatTasks(int garageId, CancellationToken ct);
  HeatTaskEntity? GetClosestActiveHeatTaskForGarageId(int garageId);
  Task UpdateHeatTask(HeatTaskEntity heatTaskEntity, CancellationToken ct);
  Task AddHeatTask(HeatTaskEntity heatTaskEntity, CancellationToken ct);
  Task DeleteHeatTask(int heatTaskId, int garageId, CancellationToken ct);
  Task SetHeatTaskActive(int id, bool active, CancellationToken ct = default);
}

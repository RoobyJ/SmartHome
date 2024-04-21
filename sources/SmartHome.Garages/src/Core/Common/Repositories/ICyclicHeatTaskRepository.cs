using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;

namespace Core.Common.Repositories;

public interface ICyclicHeatTaskRepository
{
  Task<ICollection<CyclicHeatTask>> GetCyclicHeatTasks(int garageId, CancellationToken ct);
  Task<CyclicHeatTask> GetCyclicHeatTask(int garageId, int id, CancellationToken ct);
  Task AddCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);

  Task UpdateCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);

  Task DeleteCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default);
}

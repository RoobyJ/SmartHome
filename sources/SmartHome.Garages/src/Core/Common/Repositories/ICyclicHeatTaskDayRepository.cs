using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface ICyclicHeatTaskDayRepository
{
  Task DeleteCyclicHeatTaskDays(ICollection<CyclicHeatTaskDay> entities, CancellationToken ct);
}

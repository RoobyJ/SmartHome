using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;

namespace Core.Common.Repositories;

public interface ICyclicHeatTaskDayRepository
{
  Task DeleteCyclicHeatTaskDays(ICollection<CyclicHeatTaskDay> entities, CancellationToken ct);
}

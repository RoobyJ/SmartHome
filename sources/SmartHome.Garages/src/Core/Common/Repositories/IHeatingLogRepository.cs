using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IHeatingLogRepository
{
  Task<ICollection<HeatLog>> GetHeatLogs(CancellationToken ct);

  Task AddHeatLog(HeatLog heatLog, CancellationToken ct);
}

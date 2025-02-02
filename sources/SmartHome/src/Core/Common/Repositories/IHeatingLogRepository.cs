using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IHeatingLogRepository
{
  Task<ICollection<HeatLogEntity>> GetHeatLogs(CancellationToken ct);

  Task AddHeatLog(HeatLogEntity heatLogEntity, CancellationToken ct);
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Repositories;

internal class HeatLogRepository(SmartHomeDbContext dbContext) : IHeatingLogRepository
{
  public async Task<ICollection<HeatLog>> GetHeatLogs(CancellationToken ct)
  {
    return await dbContext.HeatLogs.ToListAsync(ct);
  }

  public async Task AddHeatLog(HeatLog heatLog, CancellationToken ct)
  {
    await dbContext.HeatLogs.AddAsync(heatLog, ct);
    await dbContext.SaveChangesAsync(ct);
  }
}

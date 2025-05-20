using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class HeatLogRepository(SmartHomeDbContext dbContext) : IHeatingLogRepository
{
  public async Task<ICollection<HeatLogEntity>> GetHeatLogs(CancellationToken ct)
  {
    return await dbContext.HeatLogs.ToListAsync(ct);
  }

  public async Task AddHeatLog(HeatLogEntity heatLogEntity, CancellationToken ct)
  {
    await dbContext.HeatLogs.AddAsync(heatLogEntity, ct);
    await dbContext.SaveChangesAsync(ct);
  }
}

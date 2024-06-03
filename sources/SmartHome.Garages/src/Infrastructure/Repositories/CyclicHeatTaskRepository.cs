using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class CyclicHeatTaskRepository(SmartHomeDbContext dbContext) : ICyclicHeatTaskRepository
{
  public async Task<ICollection<CyclicHeatTask>> GetCyclicHeatTasks(int garageId, CancellationToken ct)
  {
    return await dbContext.CyclicHeatTasks.Include(i => i.CyclicHeatTaskDays).Where(i => i.GarageId == garageId)
      .ToListAsync(ct);
  }

  public async Task<CyclicHeatTask> GetCyclicHeatTask(int garageId, int id, CancellationToken ct)
  {
    return await dbContext.CyclicHeatTasks.FirstAsync(i => i.GarageId == garageId, ct);
  }

  public async Task AddCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default)
  {
    await dbContext.CyclicHeatTasks.AddAsync(entity, ct);
    await dbContext.SaveChangesAsync(ct);
  }

  public async Task UpdateCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default)
  {
    dbContext.CyclicHeatTasks.Update(entity);
    await dbContext.SaveChangesAsync(ct);
  }

  public async Task DeleteCyclicHeatTask(CyclicHeatTask entity, CancellationToken ct = default)
  {
    dbContext.CyclicHeatTasks.Remove(entity);
    await dbContext.SaveChangesAsync(ct);
  }
}

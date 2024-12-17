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

  public async Task<CyclicHeatTask> GetCyclicHeatTask(int id, CancellationToken ct)
  {
    return await dbContext.CyclicHeatTasks.FirstAsync(i => i.Id == id, ct);
  }

  public async Task<ICollection<CyclicHeatTask>> GetActiveCyclicHeatTasks(int garageId, CancellationToken ct)
  {
    return await dbContext.CyclicHeatTasks.Where(i => i.GarageId == garageId && i.Active).ToListAsync(ct);
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

  public async Task SetHeatTaskActive(int id, bool active, CancellationToken ct = default)
  {
    var entity = await dbContext.CyclicHeatTasks.FirstAsync(i => i.Id == id, ct);
    entity.Active = active;
    await dbContext.SaveChangesAsync(ct);
  }

  public void ClearTrackedEntities()
  {
    dbContext.ChangeTracker.Clear();
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class HeatTaskRepository(SmartHomeDbContext dbContext) : IHeatTaskRepository
{
  public async Task<ICollection<HeatTaskEntity>> GetHeatTasks(int garageId, CancellationToken ct)
  {
    return await dbContext.HeatTasks.Where(i => i.GarageId == garageId).ToListAsync(ct);
  }

  public HeatTaskEntity? GetClosestActiveHeatTaskForGarageId(int garageId)
  {
    var currentDateTime = DateTime.Now;
    return dbContext.HeatTasks.Where(i => i.GarageId == garageId && i.Active).MinBy(i => currentDateTime - i.Date);
  }

  public async Task<HeatTaskEntity> GetHeatTask(int taskId, CancellationToken ct)
  {
    return await dbContext.HeatTasks.Where(i => i.Id == taskId).FirstAsync(ct);
  }

  public async Task UpdateHeatTask(HeatTaskEntity heatTaskEntity, CancellationToken ct)
  {
    dbContext.HeatTasks.Update(heatTaskEntity);
    await dbContext.SaveChangesAsync(ct);
  }

  public async Task AddHeatTask(HeatTaskEntity heatTaskEntity, CancellationToken ct)
  {
    await dbContext.HeatTasks.AddAsync(heatTaskEntity, ct);
    await dbContext.SaveChangesAsync(ct);
  }

  public async Task DeleteHeatTask(int heatTaskId, int garageId, CancellationToken ct)
  {
    var heatTask = await dbContext.HeatTasks.FirstAsync(i => i.Id == heatTaskId && i.GarageId == garageId, ct);
    dbContext.HeatTasks.Remove(heatTask);
    await dbContext.SaveChangesAsync(ct);
  }

  public async Task SetHeatTaskActive(int id, bool active, CancellationToken ct = default)
  {
    var entity = await dbContext.HeatTasks.FirstAsync(i => i.Id == id, ct);
    entity.Active = active;
    await dbContext.SaveChangesAsync(ct);
  }
}

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
  public async Task<ICollection<HeatTask>> GetHeatTasks(int garageId, CancellationToken ct)
  {
    return await dbContext.HeatTasks.Where(i => i.GarageId == garageId).ToListAsync(ct);
  }

  public async Task<HeatTask> GetHeatTask(int taskId, CancellationToken ct)
  {
    return await dbContext.HeatTasks.Where(i => i.Id == taskId).FirstAsync(ct);
  }

  public async Task UpdateHeatTask(HeatTask heatTask, CancellationToken ct)
  {
    dbContext.HeatTasks.Update(heatTask);
    await dbContext.SaveChangesAsync(ct);
  }

  public async Task AddHeatTask(HeatTask heatTask, CancellationToken ct)
  {
    await dbContext.HeatTasks.AddAsync(heatTask, ct);
    await dbContext.SaveChangesAsync(ct);
  }

  public async Task DeleteHeatTask(int heatTaskId, int garageId, CancellationToken ct)
  {
    var heatTask = await dbContext.HeatTasks.FirstAsync(i => i.Id == heatTaskId && i.GarageId == garageId, ct);
    dbContext.HeatTasks.Remove(heatTask);
    await dbContext.SaveChangesAsync(ct);
  }
}

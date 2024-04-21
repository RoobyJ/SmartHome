using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Repositories;

internal class CyclicHeatTaskDayRepository(SmartHomeDbContext dbContext) : ICyclicHeatTaskDayRepository
{
  public async Task DeleteCyclicHeatTaskDays(ICollection<CyclicHeatTaskDay> entities, CancellationToken ct)
  {
    foreach (var entity in entities)
    {
      dbContext.CyclicHeatTaskDays.Remove(entity);
    }
    await dbContext.SaveChangesAsync(ct);
  }
}

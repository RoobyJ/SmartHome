using System.Linq;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Repositories;

internal class CyclicHeatTaskRepository : EfRepository<CyclicHeatTask>, ICyclicHeatTaskRepository<CyclicHeatTask>
{
  public CyclicHeatTaskRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<CyclicHeatTask> Get(CyclicHeatingTaskQueryOptions queryOptions)
  {
    var query = GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    if (queryOptions.IncludeCyclicHeatTaskDays)
    {
      query = query.Include(i => i.CyclicHeatTaskDays);
    }

    return query;
  }
}

using System.Linq;
using Core.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;
using SmartHome.Infrastructure.Persistence;

namespace SmartHome.Infrastructure.Data;

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
      query = query.Include(i => i.CyclicHeatTaskDaysInWeeks);
    }

    return query;
  }
}

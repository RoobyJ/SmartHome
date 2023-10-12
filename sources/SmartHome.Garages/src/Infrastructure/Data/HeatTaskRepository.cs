using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;
using SmartHome.Infrastructure.Persistence;

namespace SmartHome.Infrastructure.Data;

internal class HeatTaskRepository : EfRepository<HeatTask>, IHeatTaskRepository<HeatTask>
{
  public HeatTaskRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<HeatTask> Get(HeatRequestQueryOptions queryOptions)
  {
    var query = GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }

  public override async Task UpdateAsync(HeatTask entity, CancellationToken ct = default)
  {
    await base.UpdateAsync(entity, ct);
  }
}

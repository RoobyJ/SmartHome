using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Repositories;

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

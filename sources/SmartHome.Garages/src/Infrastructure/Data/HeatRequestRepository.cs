using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;

internal class HeatRequestRepository : EfRepository<HeatRequest>, IHeatRequestRepository<HeatRequest>
{
  public HeatRequestRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<HeatRequest> Get(HeatRequestQueryOptions queryOptions)
  {
    var query = GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }

  public override async Task UpdateAsync(HeatRequest entity, CancellationToken ct = default)
  {
    await base.UpdateAsync(entity, ct);
  }
}

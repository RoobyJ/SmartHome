using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;

internal class HeatLogRepository : EfRepository<HeatLog>, IHeatingLogRepository
{
  public HeatLogRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<HeatLog> Get(GarageQueryOptions queryOptions)
  {
    var query = GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

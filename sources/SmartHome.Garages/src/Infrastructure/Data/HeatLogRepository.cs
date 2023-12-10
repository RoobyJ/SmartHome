using System.Linq;
using Core.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;
using SmartHome.Infrastructure.Persistence;

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

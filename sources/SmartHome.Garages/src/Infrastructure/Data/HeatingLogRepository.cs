using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;

internal class HeatingLogRepository : EfRepository<HeatingLog>, IHeatingLogRepository
{
  public HeatingLogRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<HeatingLog> Get(GarageQueryOptions queryOptions)
  {
    var query = GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

using System.Linq;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;

namespace Infrastructure.Data;

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

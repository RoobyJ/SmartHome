using System.Linq;
using Core.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;
using SmartHome.Infrastructure.Persistence;

namespace SmartHome.Infrastructure.Data;

internal class GarageRepository : EfRepository<Garage>, IGarageRepository
{
  public GarageRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<Garage> Get(GarageQueryOptions queryOptions)
  {
    var query = GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

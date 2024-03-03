using System.Linq;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Repositories;

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

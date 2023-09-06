using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;

internal class CyclicHeatingRequestRepository : EfRepository<CyclicHeatRequest>, ICyclicHeatingRequestRepository
{
  public CyclicHeatingRequestRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<CyclicHeatRequest> Get(GarageQueryOptions queryOptions)
  {
    var query = this.GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

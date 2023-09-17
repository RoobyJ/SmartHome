using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;

internal class HeatingRequestRepository : EfRepository<HeatRequest>, IHeatingRequestRepository
{
  public HeatingRequestRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<HeatRequest> Get(HeatingRequestQueryOptions queryOptions)
  {
    var query = this.GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

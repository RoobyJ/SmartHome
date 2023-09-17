using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;

internal class GarageRepository : EfRepository<Garage>, IGarageRepository
{
  public GarageRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<Garage> Get(GarageQueryOptions queryOptions)
  {
    var query = this.GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

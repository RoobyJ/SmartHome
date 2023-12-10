using System.Linq;
using Core.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;
using SmartHome.Infrastructure.Persistence;

namespace SmartHome.Infrastructure.Data;

internal class OutsideTemperaturesRepository : EfRepository<OutsideTemperature>, IOutsideTemperatureRepository<OutsideTemperature>
{
  public OutsideTemperaturesRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<OutsideTemperature> Get(OutsideTemperatureQueryOptions queryOptions)
  {
    var query = GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

using System.Linq;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;

namespace Infrastructure.Data;

internal class OutsideTemperaturesRepository : EfRepository<OutsideTemperature>,
  IOutsideTemperatureRepository<OutsideTemperature>
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

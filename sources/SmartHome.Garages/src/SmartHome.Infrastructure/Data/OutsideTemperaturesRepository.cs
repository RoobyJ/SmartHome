using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;


internal class OutsideTemperaturesRepository : EfRepository<OutsideTemperature>, IOutsideTemperatureRepository
{
  public OutsideTemperaturesRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
  }

  public IQueryable<OutsideTemperature> Get(OutsideTemperatureQueryOptions queryOptions)
  {
    var query = this.GetAll();

    if (queryOptions.AsNoTracking)
    {
      query = query.AsNoTracking();
    }

    return query;
  }
}

using System.Linq;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class OutsideTemperatureQueryOptions
{
  public bool AsNoTracking { get; set; } = true;
}

public interface IOutsideTemperatureRepository : IRepository
{
  IQueryable<OutsideTemperature> Get(OutsideTemperatureQueryOptions queryOptions);
}

using System.Linq;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class CyclicHeatingRequestQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface ICyclicHeatingRequestRepository : IRepository
{
  IQueryable<CyclicHeatRequest> Get(GarageQueryOptions queryOptions);
  
}

using System.Linq;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class HeatingRequestQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface IHeatingRequestRepository : IRepository
{
  IQueryable<HeatRequest> Get(GarageQueryOptions queryOptions);
}

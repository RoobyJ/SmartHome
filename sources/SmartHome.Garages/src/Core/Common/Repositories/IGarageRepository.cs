using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class GarageQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface IGarageRepository
{
  IQueryable<Garage> Get(GarageQueryOptions queryOptions);

  Task AddAsync(Garage entity, CancellationToken cancellationToken = default);
}

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public class GarageQueryOptions
{
  public bool AsNoTracking { get; set; } = true;
}

public interface IGarageRepository
{
  IQueryable<Garage> Get(GarageQueryOptions queryOptions = default);

  Task AddAsync(Garage entity, CancellationToken cancellationToken = default);
}

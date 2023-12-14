using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;

namespace Core.Common.Repositories;

public class CyclicHeatingTaskQueryOptions
{
  public bool AsNoTracking { get; init; } = true;
  public bool IncludeCyclicHeatTaskDays { get; init; }
}

public interface ICyclicHeatTaskRepository<TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<TEntity> Get(CyclicHeatingTaskQueryOptions queryOptions = default);

  Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

  Task UpdateAsync(TEntity entity, CancellationToken ct = default);

  Task DeleteAsync(TEntity entity, CancellationToken ct = default);
}

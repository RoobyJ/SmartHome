using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Common.Repositories;

public class HeatRequestQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface IHeatTaskRepository<TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<TEntity> Get(HeatRequestQueryOptions queryOptions);

  Task AddAsync(TEntity entity, CancellationToken ct = default);

  Task UpdateAsync(TEntity entity, CancellationToken ct = default);

  Task DeleteAsync(TEntity entity, CancellationToken ct = default);
}

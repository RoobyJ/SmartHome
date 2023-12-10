using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;

namespace Core.Common.Repositories;

public class HeatRequestQueryOptions
{
  public bool AsNoTracking { get; set; } = true;
}

public interface IHeatTaskRepository<TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<TEntity> Get(HeatRequestQueryOptions queryOptions = default);

  Task AddAsync(TEntity entity, CancellationToken ct = default);
  
  Task UpdateAsync(TEntity entity, CancellationToken ct = default);

  Task DeleteAsync(TEntity entity, CancellationToken ct = default);
}

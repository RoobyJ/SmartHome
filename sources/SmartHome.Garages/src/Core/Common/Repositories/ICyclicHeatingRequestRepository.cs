using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class CyclicHeatingRequestQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface ICyclicHeatingRequestRepository<TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<TEntity> Get(CyclicHeatingRequestQueryOptions queryOptions);

  Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
  
  Task UpdateAsync(TEntity entity, CancellationToken ct = default);
}

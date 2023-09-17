using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Common.Repositories;

public interface IRepository
{
  IUnitOfWork UnitOfWork { get; }
}

public interface IRepository<TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<TEntity> GetAll();
  
  Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
  
  Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
  
  Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}

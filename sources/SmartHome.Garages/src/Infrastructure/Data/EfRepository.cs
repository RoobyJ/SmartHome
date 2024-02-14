using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;

namespace Infrastructure.Data;

internal class EfRepository : IRepository
{
  public EfRepository(IUnitOfWork dbContext)
  {
    UnitOfWork = dbContext;
  }

  public IUnitOfWork UnitOfWork { get; }
}

internal class EfRepository<T> : EfRepository, IRepository<T> where T : class, IEntity
{
  private readonly SmartHomeDbContext _dbContext;

  public EfRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  private DbSet<T> DbSet => _dbContext.Set<T>();

  public virtual IQueryable<T> GetAll()
  {
    return _dbContext.Set<T>();
  }

  public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    await DbSet.AddAsync(entity, cancellationToken);
  }

  public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    return Task.CompletedTask;
  }

  public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    DbSet.Remove(entity);
    return Task.CompletedTask;
  }
}

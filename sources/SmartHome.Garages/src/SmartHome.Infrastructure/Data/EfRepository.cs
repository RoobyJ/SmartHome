using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;

namespace SmartHome.Infrastructure.Data;

internal class EfRepository : IRepository
{
  public EfRepository(IUnitOfWork dbContext)
  {
    this.UnitOfWork = dbContext;
  }

  public IUnitOfWork UnitOfWork { get; }
}

internal class EfRepository<T>: EfRepository, IRepository<T> where T : class, IEntity
{
  private readonly SmartHomeDbContext _dbContext;

  public EfRepository(SmartHomeDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }
  
  protected DbSet<T> DbSet => this._dbContext.Set<T>();

  public virtual IQueryable<T> GetAll()
  {
    return this._dbContext.Set<T>();
  }

  public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    await this.DbSet.AddAsync(entity, cancellationToken);
  }

  public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    
    return Task.CompletedTask;
  }

  public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    this.DbSet.Remove(entity);
    return Task.CompletedTask;
  }
}

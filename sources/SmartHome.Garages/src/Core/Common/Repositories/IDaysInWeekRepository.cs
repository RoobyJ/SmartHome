using System.Linq;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;

namespace Core.Common.Repositories;

public class DaysInWeekQueryOptions
{
  public bool AsNoTracking { get; init; } = true;
}

public interface IDaysInWeekRepository<out TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<TEntity> Get(DaysInWeekQueryOptions queryOptions = default);
}

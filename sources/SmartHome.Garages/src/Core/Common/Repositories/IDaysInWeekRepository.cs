using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Common.Repositories;

public class DaysInWeekQueryOptions
{
  public bool AsNoTracking { get; init; }
}

public interface IDaysInWeekRepository<out TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<TEntity> Get(DaysInWeekQueryOptions queryOptions);
}

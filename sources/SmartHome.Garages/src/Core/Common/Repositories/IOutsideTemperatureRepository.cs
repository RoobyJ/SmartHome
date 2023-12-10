using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace Core.Common.Repositories;

public class OutsideTemperatureQueryOptions
{
  public bool AsNoTracking { get; set; } = true;
}

public interface IOutsideTemperatureRepository<in TEntity> : IRepository where TEntity : IEntity
{
  IQueryable<OutsideTemperature> Get(OutsideTemperatureQueryOptions queryOptions = default);
  
  Task AddAsync(TEntity entity, CancellationToken ct = default);
}

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class HeatingLogQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface IHeatingLogRepository : IRepository
{
  IQueryable<HeatLog> Get(GarageQueryOptions queryOptions);

  Task AddAsync(HeatLog entity, CancellationToken cancellationToken = default);
}

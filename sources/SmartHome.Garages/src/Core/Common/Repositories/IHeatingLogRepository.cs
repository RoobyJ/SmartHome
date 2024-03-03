using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Common.Repositories;
using Core.Entities;

namespace Core.Common.Repositories;

public class HeatingLogQueryOptions
{
  public bool AsNoTracking { get; set; } = true;
}

public interface IHeatingLogRepository : IRepository
{
  IQueryable<HeatLog> Get(GarageQueryOptions queryOptions = default);

  Task AddAsync(HeatLog entity, CancellationToken cancellationToken = default);
}

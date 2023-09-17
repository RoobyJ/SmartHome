using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class HeatingRequestQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface IHeatingRequestRepository : IRepository
{
  IQueryable<HeatRequest> Get(HeatingRequestQueryOptions queryOptions);
  
  Task AddAsync(HeatRequest entity, CancellationToken cancellationToken = default);
}

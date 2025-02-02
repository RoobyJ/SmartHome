using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IGarageRepository
{
  Task<GarageEntity?> GetGarage(int garageId, CancellationToken ct);
  Task<ICollection<GarageEntity>> GetGarages(CancellationToken ct);
  Task AddGarage(GarageEntity garageEntity, CancellationToken cancellationToken);
  Task<IEnumerable<string>> GetGaragesIpsByIds(IEnumerable<int> garages, CancellationToken ct);
}

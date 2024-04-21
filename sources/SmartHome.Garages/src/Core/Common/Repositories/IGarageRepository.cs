using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IGarageRepository
{
  Task<Garage?> GetGarage(int garageId, CancellationToken ct);
  Task<ICollection<Garage>> GetGarages(CancellationToken ct);
  Task AddGarage(Garage garage, CancellationToken cancellationToken);
}

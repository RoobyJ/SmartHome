using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IOutsideTemperatureRepository
{
  Task<ICollection<OutsideTemperature>> GetTemperatures(int garageId, int days, CancellationToken ct);
  Task AddTemperatures(ICollection<OutsideTemperature> temperatures, CancellationToken ct);
}

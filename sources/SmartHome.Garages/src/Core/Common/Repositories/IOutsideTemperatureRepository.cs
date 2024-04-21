using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Common;
using SmartHome.Core.Common.Repositories;
using Core.Entities;

namespace Core.Common.Repositories;

public interface IOutsideTemperatureRepository
{
  Task<ICollection<OutsideTemperature>> GetTemperatures(int garageId, int days, CancellationToken ct);
  Task AddTemperatures(ICollection<OutsideTemperature> temperatures, CancellationToken ct);
}

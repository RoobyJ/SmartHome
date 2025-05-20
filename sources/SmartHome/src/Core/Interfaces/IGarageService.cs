using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces;

public interface IGarageService
{
  public Task<ICollection<GarageDetailsDto>> GetGarages(CancellationToken ct);
  public Task<ICollection<OutsideTemperatureEntity>> GetTemperatures(int id, int days, CancellationToken ct);
  public Task<GarageEntity?> GetGarageById(int id, CancellationToken ct);
  public Task<bool?> GetGarageHeaterStatus(int garageId, CancellationToken ct);
}

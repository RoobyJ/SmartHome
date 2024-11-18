using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class GarageRepository(SmartHomeDbContext dbContext) : IGarageRepository
{
  public async Task<Garage?> GetGarage(int garageId, CancellationToken ct)
  {
    return await dbContext.Garages.FirstOrDefaultAsync(i => i.Id == garageId, ct);
  }

  public async Task<ICollection<Garage>> GetGarages(CancellationToken ct)
  {
    return await dbContext.Garages.ToListAsync(ct);
  }

  public async Task AddGarage(Garage garage, CancellationToken ct)
  {
    await dbContext.Garages.AddAsync(garage, ct);
    await dbContext.SaveChangesAsync(ct);
  }
}

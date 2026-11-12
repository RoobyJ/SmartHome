using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class GarageRepository(SmartHomeDbContext dbContext) : IGarageRepository
{
  public async Task<GarageEntity?> GetGarage(int garageId, CancellationToken ct)
  {
    return await dbContext.Garages.FirstOrDefaultAsync(i => i.Id == garageId, ct);
  }

  public async Task<ICollection<GarageEntity>> GetGarages(CancellationToken ct)
  {
    return await dbContext.Garages.ToListAsync(ct);
  }

  public async Task AddGarage(GarageEntity garageEntity, CancellationToken ct)
  {
    await dbContext.Garages.AddAsync(garageEntity, ct);
    await dbContext.SaveChangesAsync(ct);
  }
}

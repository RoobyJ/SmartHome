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

internal class OutsideTemperaturesRepository(SmartHomeDbContext dbContext) : IOutsideTemperatureRepository
{
  public async Task<ICollection<OutsideTemperature>> GetTemperatures(int garageId, int days, CancellationToken ct)
  {
    return await dbContext.OutsideTemperatures
      .Where(i => i.GarageId == garageId && i.Date.Ticks > new DateTime().AddDays(-days).Ticks).ToListAsync(ct);
  }

  public async Task AddTemperatures(ICollection<OutsideTemperature> temperatures, CancellationToken ct)
  {
    await dbContext.OutsideTemperatures.AddRangeAsync(temperatures, ct);
    await dbContext.SaveChangesAsync(ct);
  }
}

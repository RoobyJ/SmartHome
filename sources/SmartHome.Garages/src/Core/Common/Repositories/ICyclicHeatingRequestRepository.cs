﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Common.Repositories;

public class CyclicHeatingRequestQueryOptions
{
  public bool AsNoTracking { get; set; }
}

public interface ICyclicHeatingRequestRepository : IRepository
{
  IQueryable<CyclicHeatRequest> Get(CyclicHeatingRequestQueryOptions queryOptions);

  Task AddAsync(CyclicHeatRequest entity, CancellationToken cancellationToken = default);
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Interfaces;

namespace SmartHome.Core.Services;

public class HeatTaskService : IHeatTaskService
{
  private readonly ICyclicHeatTaskRepository<CyclicHeatTask> _cyclicHeatTaskRepository;
  private readonly IHeatTaskRepository<HeatTask> _heatTaskRepository;

  public HeatTaskService(ICyclicHeatTaskRepository<CyclicHeatTask> cyclicHeatTaskRepository,
    IHeatTaskRepository<HeatTask> heatTaskRepository)
  {
    _cyclicHeatTaskRepository = cyclicHeatTaskRepository;
    _heatTaskRepository = heatTaskRepository;
  }

  public async Task SaveHeatTimeTask(int id, HeatRequestDto heatRequest, CancellationToken ct)
  {
    var heatTimeRequest = new HeatTask { GarageId = id, Date = heatRequest.Date };

    await _heatTaskRepository.AddAsync(heatTimeRequest, ct);
    await _heatTaskRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task<ICollection<HeatTask>> GetHeatTimeTasks(int id, CancellationToken ct)
  {
    return await _heatTaskRepository.Get(new HeatRequestQueryOptions()).Where(i => i.GarageId == id)
      .ToListAsync(ct);
  }

  public async Task UpdateHeatTask(int id, HeatRequestDto request, CancellationToken ct)
  {
    var heatRequest = await _heatTaskRepository.Get(new HeatRequestQueryOptions()).Where(i => i.GarageId == id)
      .FirstAsync(ct);
    //TODO: this method and the cyclic one must be corrected, its not working

    await _heatTaskRepository.UpdateAsync(heatRequest, ct);
    await _heatTaskRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task DeleteHeatTimeTask(int garageId, int requestId, CancellationToken ct)
  {
    var request = await _heatTaskRepository.Get(new HeatRequestQueryOptions())
      .Where(i => i.GarageId == garageId && i.Id == requestId).FirstAsync(ct);
    await _heatTaskRepository.DeleteAsync(request, ct);
    await _heatTaskRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task CreateCyclicHeatTask(int id, CreateCyclicHeatTaskDto task, CancellationToken ct)
  {
    var cyclicHeatTaskEntity = new CyclicHeatTask { GarageId = id, Time = task.Time };

    await _cyclicHeatTaskRepository.AddAsync(cyclicHeatTaskEntity, ct);

    cyclicHeatTaskEntity.CyclicHeatTaskDaysInWeeks = task.DaysInWeekSelected.Select(i =>
      new CyclicHeatTaskDaysInWeek { DayId = (int)i, CyclicHeatTaskId = cyclicHeatTaskEntity.Id }).ToList();

    await _cyclicHeatTaskRepository.AddAsync(cyclicHeatTaskEntity, ct);
    await _cyclicHeatTaskRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task UpdateCyclicHeatTask(int id, UpdateCyclicHeatTaskDto task, CancellationToken ct)
  {
    var cyclicHeatTaskEntity = new CyclicHeatTask { Id = task.Id, GarageId = id, Time = task.Time };

    await _cyclicHeatTaskRepository.AddAsync(cyclicHeatTaskEntity, ct);

    cyclicHeatTaskEntity.CyclicHeatTaskDaysInWeeks = task.DaysInWeekSelected.Select(i =>
      new CyclicHeatTaskDaysInWeek { DayId = (int)i, CyclicHeatTaskId = cyclicHeatTaskEntity.Id }).ToList();

    await _cyclicHeatTaskRepository.AddAsync(cyclicHeatTaskEntity, ct);
    await _cyclicHeatTaskRepository.UnitOfWork.SaveChangesAsync(ct);
  }

  public async Task<ICollection<CyclicHeatTask>> GetCyclicHeatTasks(int id, CancellationToken ct)
  {
    return await _cyclicHeatTaskRepository.Get(new CyclicHeatingTaskQueryOptions
      {
        AsNoTracking = true, IncludeCyclicHeatTaskDays = true
      })
      .Where(i => i.GarageId == id).ToListAsync(ct);
  }

  public async Task DeleteCyclicHeatTask(int id, int taskId, CancellationToken ct)
  {
    var request = await _cyclicHeatTaskRepository.Get(new CyclicHeatingTaskQueryOptions
      {
        AsNoTracking = true, IncludeCyclicHeatTaskDays = true
      })
      .Where(i => i.GarageId == id && i.Id == taskId).FirstAsync(ct);
    await _cyclicHeatTaskRepository.DeleteAsync(request, ct);
    await _cyclicHeatTaskRepository.UnitOfWork.SaveChangesAsync(ct);
  }
}

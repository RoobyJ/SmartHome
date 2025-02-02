using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;

namespace Core.Services;

public class HeatTaskService(
  ICyclicHeatTaskRepository cyclicHeatTaskRepository,
  ICyclicHeatTaskDayRepository cyclicHeatTaskDayRepository,
  IHeatTaskRepository heatTaskRepository)
  : IHeatTaskService
{
  public async Task SaveHeatTimeTask(int id, CreateHeatTaskDto heatTask, CancellationToken ct)
  {
    var heatTaskRequest = new HeatTaskEntity { GarageId = id, Date = heatTask.Date, Active = true};
    await heatTaskRepository.AddHeatTask(heatTaskRequest, ct);
  }

  public async Task<ICollection<HeatTaskEntity>> GetHeatTimeTasks(int id, CancellationToken ct)
  {
    return await heatTaskRepository.GetHeatTasks(id, ct);
  }

  public async Task UpdateHeatTask(int garageId, HeatTaskDto task, CancellationToken ct)
  {
    var heatTask = await heatTaskRepository.GetHeatTask(garageId, ct);

    if (heatTask == null)
    {
      throw new Exception("Such heat task doesn't exist");
    }

    heatTask.Date = task.Date;

    await heatTaskRepository.UpdateHeatTask(heatTask, ct);
  }

  public async Task DeleteHeatTimeTask(int garageId, int heatTaskId, CancellationToken ct)
  {
    await heatTaskRepository.DeleteHeatTask(heatTaskId, garageId, ct);
  }

  public async Task CreateCyclicHeatTask(int id, CreateCyclicHeatTaskDto task, CancellationToken ct)
  {
    var cyclicHeatTaskEntity = new CyclicHeatTaskEntity { GarageId = id, Time = task.Time, Active = true};

    cyclicHeatTaskEntity.CyclicHeatTaskDays = task.DaysInWeekSelected.Select(i =>
      new CyclicHeatTaskDayEntity { Day = (int)i, CyclicHeatTaskEntity = cyclicHeatTaskEntity }).ToList();

    await cyclicHeatTaskRepository.AddCyclicHeatTask(cyclicHeatTaskEntity, ct);
  }

  public async Task UpdateCyclicHeatTask(int garageId, UpdateCyclicHeatTaskDto task, CancellationToken ct)
  {
    var cyclicHeatTask = await cyclicHeatTaskRepository.GetCyclicHeatTask(task.Id, ct);
    await cyclicHeatTaskDayRepository.DeleteCyclicHeatTaskDays(cyclicHeatTask.CyclicHeatTaskDays, ct);
    cyclicHeatTaskRepository.ClearTrackedEntities();
    var entity = new CyclicHeatTaskEntity
    {
      Id = task.Id,
      GarageId = garageId,
      Time = task.Time,
      CyclicHeatTaskDays = task.DaysInWeekSelected
        .Select(i => new CyclicHeatTaskDayEntity { Day = (int)i, CyclicHeatTaskId = task.Id }).ToList()
    };
    await cyclicHeatTaskRepository.UpdateCyclicHeatTask(entity, ct);
  }

  public async Task<ICollection<CyclicHeatTaskEntity>> GetCyclicHeatTasks(int id, CancellationToken ct)
  {
    return await cyclicHeatTaskRepository.GetCyclicHeatTasks(id, ct);
  }

  public async Task DeleteCyclicHeatTask(int garageId, int taskId, CancellationToken ct)
  {
    var task = await cyclicHeatTaskRepository.GetCyclicHeatTask(taskId, ct);
    await cyclicHeatTaskRepository.DeleteCyclicHeatTask(task, ct);
  }
  
  public async Task SetHeatTaskActive(SetHeatTaskActiveDto data, CancellationToken ct)
  {
    if (data.IsCyclic)
    {
      await cyclicHeatTaskRepository.SetHeatTaskActive(data.Id, data.Active, ct);
    }
    else
    {
      await heatTaskRepository.SetHeatTaskActive(data.Id, data.Active, ct);
    }
  }
}

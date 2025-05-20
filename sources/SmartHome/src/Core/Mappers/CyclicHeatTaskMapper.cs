using System;
using Core.Entities;
using Core.Models;

namespace Core.Mappers;

public static class CyclicHeatTaskMapper
{
  public static HeatTask ToHeatTask(this CyclicHeatTaskEntity entity, DateTime startDateTime)
  {
    return new HeatTask
    {
      GarageId = entity.GarageId, HeatTaskId = entity.Id, IsCyclic = true, EndTime = startDateTime
    };
  }
}

using Core.Entities;
using Core.Models;

namespace Core.Mappers;

public static class HeatTaskMapper
{
  public static HeatTask ToHeatTask(this HeatTaskEntity entity)
  {
    return new HeatTask { GarageId = entity.GarageId, HeatTaskId = entity.Id, EndTime = entity.Date, IsCyclic = false};
  }
}

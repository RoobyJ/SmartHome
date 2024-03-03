using SmartHome.Core.Common;

namespace Core.Entities;

public partial class CyclicHeatTaskDay : IEntity
{
    public int Id { get; set; }

    public int Day { get; set; }

    public int CyclicHeatTaskId { get; set; }

    public virtual CyclicHeatTask CyclicHeatTask { get; set; }
}

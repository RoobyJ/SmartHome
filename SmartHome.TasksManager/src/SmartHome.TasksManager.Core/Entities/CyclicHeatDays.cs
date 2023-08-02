using System;

namespace SmartHome.TasksManager.Core.Entities;

public class CyclicHeatDays
{
    public TimeSpan? Monday { get; set; }
    
    public TimeSpan? Tuesday { get; set; }
    
    public TimeSpan? Wednesday { get; set; }
    
    public TimeSpan? Thursday { get; set; }
    
    public TimeSpan? Friday { get; set; }
    
    public TimeSpan? Saturday { get; set; }
    
    public TimeSpan? Sunday { get; set; }
}

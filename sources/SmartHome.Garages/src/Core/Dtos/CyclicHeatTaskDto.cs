using System;
using System.Collections;
using System.Collections.Generic;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Core.Dtos;

public class CyclicHeatRequestsDto
{
public int Id { get; set; }
public int GarageId { get; set; }
public TimeOnly Time { get; set; }
public ICollection<DayOfWeek> DaysInWeekSelected { get; set; }
}

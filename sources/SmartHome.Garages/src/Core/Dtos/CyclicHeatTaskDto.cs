using System;
using System.Collections;
using System.Collections.Generic;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Core.Dtos;

public class CyclicHeatTaskDto
{
public int Id { get; set; }
public string Time { get; set; }
public ICollection<int> DaysInWeekSelected { get; set; }
}

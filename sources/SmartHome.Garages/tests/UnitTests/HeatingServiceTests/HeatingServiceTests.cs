using System;
using System.Collections.Generic;
using SmartHome.Core.DTOs;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Models;

namespace UnitTests.HeatingServiceTests;

public class TestingDataWhichIsCloser
{
  public DateTime ExampleBase { get; init; }
  public DateTime ExpectedResult { get; set; }
  public HeatTask CustomHeatTask { get; init; }
  public CyclicHeatTask CyclicHeatTask { get; set; }
}

public class HeatingServiceTests
{
  [Fact]
  public void CheckWhichIsCloserHeatTime_ForSameDayRequests()
  {
    var exampleBase = DateTime.Now;
    var expectedResult = exampleBase.AddHours(1);
    var cyclicHeatTask = new CyclicHeatTask
    {
      Id = 1,
      Time = DateTime.Now.AddHours(1).TimeOfDay,
      GarageId = 1,
      CyclicHeatTaskDaysInWeeks = new List<CyclicHeatTaskDaysInWeek>()
      {
        new CyclicHeatTaskDaysInWeek() { Id = 1, CyclicHeatTaskId = 1, DayId = 0 },
        new CyclicHeatTaskDaysInWeek() { Id = 1, CyclicHeatTaskId = 1, DayId = 1 },
      }
    };

    var customHeatRequest = new HeatTask { Id = 1, Date = expectedResult, GarageId = 1 };
    var result = HeatingServiceHelper.CheckWhichIsCloser(cyclicHeatTask, customHeatRequest
    );

    Assert.True(result.Equals(expectedResult));
  }

  [Fact]
  public void CheckWhichIsCloserHeatTime_ForDifferentDaysRequests()
  {
    var testingData = GetTestingDataForFindingWhichIsCloser(1, 26);


    var result = HeatingServiceHelper.CheckWhichIsCloser(testingData.CyclicHeatTask, testingData.CustomHeatTask
    );

    Assert.False(result.Equals(testingData.ExpectedResult));
  }

  [Fact]
  public void CheckStartHeatTime()
  {
    var temperatures =
      new List<GarageTemperatureDto> { new() { Id = 1, Temperature = 20 } };
    var heatTimes =
      new List<GarageHeatingTime> { new() { Id = 1, HeatTime = DateTime.Now.AddHours(5) } };
    var expectedResult =
      new List<GarageHeatingTime> { new() { Id = 1, HeatTime = DateTime.Now.AddHours(4).AddMinutes(24) } };

    var result = new StartHeatingTimeCalculator().CalculateForMultipleGarages(temperatures, heatTimes);

    if (!result[0].StartHeatTime.HasValue)
    {
      Assert.Fail("No startHeat time");
    }

    if (!expectedResult[0].HeatTime.HasValue)
    {
      Assert.Fail("No expected time");
    }

    Assert.True(Math.Abs(result[0].StartHeatTime.Value.TimeOfDay.TotalMinutes -
                         expectedResult[0].HeatTime.Value.TimeOfDay.TotalMinutes) < 10);
  }

  private static TestingDataWhichIsCloser GetTestingDataForFindingWhichIsCloser(int extraHoursCyclic, int extraHoursExpected)
  {
    return new TestingDataWhichIsCloser
    {
      ExampleBase = DateTime.Now,
      ExpectedResult = DateTime.Now.AddHours(extraHoursExpected),
      CustomHeatTask =
        new HeatTask { Id = 1, Date = DateTime.Now.AddHours(extraHoursExpected), GarageId = 1 },
      CyclicHeatTask = new CyclicHeatTask
      {
        Id = 1,
        Time = DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay,
        GarageId = 1,
        CyclicHeatTaskDaysInWeeks = new List<CyclicHeatTaskDaysInWeek>()
        {
          new CyclicHeatTaskDaysInWeek() {Id = 1, CyclicHeatTaskId = 1, DayId = 0},
          new CyclicHeatTaskDaysInWeek() {Id = 1, CyclicHeatTaskId = 1, DayId = 1},
        }
      }
    };
  }
}

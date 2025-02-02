using System;
using System.Collections.Generic;
using Core.Dtos;
using Core.Entities;
using Core.Helpers;
using Core.Models;

namespace UnitTests.HeatingServiceTests;

public class TestingDataWhichIsCloser
{
  public DateTime ExampleBase { get; init; }
  public DateTime ExpectedResult { get; set; }
  public HeatTaskEntity CustomHeatTaskEntity { get; init; }
  public CyclicHeatTaskEntity CyclicHeatTaskEntity { get; set; }
}

public class HeatingServiceTests
{
  [Fact]
  public void CheckWhichIsCloserHeatTime_ForSameDayRequests()
  {
    var exampleBase = DateTime.Now;
    var expectedResult = exampleBase.AddHours(1);
    var cyclicHeatTask = new CyclicHeatTaskEntity
    {
      Id = 1,
      Time = DateTime.Now.AddHours(1).TimeOfDay,
      GarageId = 1,
      CyclicHeatTaskDays = new List<CyclicHeatTaskDayEntity>
      {
        new() { Id = 1, CyclicHeatTaskId = 1, Day = 0 }, new() { Id = 1, CyclicHeatTaskId = 1, Day = 1 }
      }
    };

    var customHeatRequest = new HeatTaskEntity { Id = 1, Date = expectedResult, GarageId = 1 };
    var result = HeatingServiceHelper.CheckWhichIsCloser(cyclicHeatTask, customHeatRequest
    );

    Assert.True(result.ClosestDate.Equals(expectedResult));
  }

  [Fact]
  public void CheckWhichIsCloserHeatTime_ForDifferentDaysRequests()
  {
    var testingData = GetTestingDataForFindingWhichIsCloser(1, 26);


    var result = HeatingServiceHelper.CheckWhichIsCloser(testingData.CyclicHeatTaskEntity, testingData.CustomHeatTaskEntity
    );

    Assert.False(result.Equals(testingData.ExpectedResult));
  }

  [Fact]
  public void Check_For_CyclicHeatTask_OnDifferentDayRequests()
  {
    // Arange
    var task = new CyclicHeatTaskEntity
    {
      Id = 1,
      GarageId = 1,
      Time = new TimeSpan(12, 0, 0),
      Active = true,
      CyclicHeatTaskDays = new List<CyclicHeatTaskDayEntity>
      {
        new() { Id = 1, CyclicHeatTaskId = 1, Day = (int)DateTime.Now.DayOfWeek + 3 }
      }
    };

    // Act
    var result = task.GetClosestDateTimeFromCyclicHeatTask();

    
    // Assert
    var currentDate = DateOnly.FromDateTime(new DateTime());
    var assertVal = DateOnly.FromDateTime(result);
    Assert.False(assertVal.Equals(currentDate));
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

    var result = StartHeatingTimeCalculator.CalculateForMultipleGarages(temperatures, heatTimes);

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

  private static TestingDataWhichIsCloser GetTestingDataForFindingWhichIsCloser(int extraHoursCyclic,
    int extraHoursExpected)
  {
    return new TestingDataWhichIsCloser
    {
      ExampleBase = DateTime.Now,
      ExpectedResult = DateTime.Now.AddHours(extraHoursExpected),
      CustomHeatTaskEntity =
        new HeatTaskEntity { Id = 1, Date = DateTime.Now.AddHours(extraHoursExpected), GarageId = 1 },
      CyclicHeatTaskEntity = new CyclicHeatTaskEntity
      {
        Id = 1,
        Time = DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay,
        GarageId = 1,
        CyclicHeatTaskDays = new List<CyclicHeatTaskDayEntity>
        {
          new() { Id = 1, CyclicHeatTaskId = 1, Day = 0 }, new() { Id = 1, CyclicHeatTaskId = 1, Day = 1 }
        }
      }
    };
  }
}

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
  public HeatRequest CustomHeatRequest { get; init; }
  public List<TimeSpan?> CyclicHeatTimes { get; set; }
}

public class HeatingServiceTests
{
  [Fact]
  public void CheckWhichIsCloserHeatTime_ForSameDayRequests()
  {
    var exampleBase = DateTime.Now;
    var expectedResult = exampleBase.AddHours(1);
    var cyclicHeatTimes = new List<TimeSpan?>
    {
      exampleBase.AddHours(2).TimeOfDay, // sunday
      exampleBase.AddHours(2).TimeOfDay, // monday
      exampleBase.AddHours(2).TimeOfDay, // ...
      exampleBase.AddHours(2).TimeOfDay,
      exampleBase.AddHours(2).TimeOfDay,
      exampleBase.AddHours(2).TimeOfDay,
      exampleBase.AddHours(2).TimeOfDay // saturday
    };

    var customHeatRequest = new HeatRequest { Id = 1, HeatRequest1 = expectedResult, GarageId = 1 };
    var result = HeatingServiceHelper.CheckWhichIsCloser(cyclicHeatTimes, customHeatRequest
    );

    Assert.True(result.Equals(expectedResult));
  }

  [Fact]
  public void CheckWhichIsCloserHeatTime_ForDifferentDaysRequests()
  {
    var testingData = GetTestingDataForFindingWhichIsCloser(1, 26);


    var result = HeatingServiceHelper.CheckWhichIsCloser(testingData.CyclicHeatTimes, testingData.CustomHeatRequest
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

  private TestingDataWhichIsCloser GetTestingDataForFindingWhichIsCloser(int extraHoursCyclic, int extraHoursExpected)
  {
    return new TestingDataWhichIsCloser
    {
      ExampleBase = DateTime.Now,
      ExpectedResult = DateTime.Now.AddHours(extraHoursExpected),
      CustomHeatRequest =
        new HeatRequest { Id = 1, HeatRequest1 = DateTime.Now.AddHours(extraHoursExpected), GarageId = 1 },
      CyclicHeatTimes = new List<TimeSpan?>
      {
        DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay, // sunday
        DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay, // monday
        DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay, // ...
        DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay,
        DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay,
        DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay,
        DateTime.Now.AddHours(extraHoursCyclic).TimeOfDay // saturday
      }
    };
  }
}

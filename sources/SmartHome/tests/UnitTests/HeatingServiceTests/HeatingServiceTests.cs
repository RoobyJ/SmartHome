using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Dtos;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Models;
using Core.Services;
using Moq;

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
  private readonly Mock<ILoggerAdapter<HeatingService>> loggerMock = new();
  private readonly Mock<IGarageRepository> garageRepositoryMock = new();
  private readonly Mock<IHeatTaskRepository> heatTaskRepositoryMock = new();
  private readonly Mock<ICyclicHeatTaskRepository> cyclicHeatTaskRepositoryMock = new();
  private readonly Mock<IOutsideTemperatureRepository> outsideTemperatureRepositoryMock = new();
  private readonly Mock<IHeatingLogRepository> heatingLogRepositoryMock = new();
  private readonly Mock<IGarageClient> garageClientMock = new();
  private readonly Mock<IDateTimeProvider> dateTimeProviderMock = new();

  public HeatingServiceTests()
  {
    ResetStaticTracker();
  }

  private static void ResetStaticTracker()
  {
    var field = typeof(HeatingService).GetField("garageHeatingState",
      BindingFlags.Static | BindingFlags.NonPublic);
    var dictionary = field?.GetValue(null) as System.Collections.IDictionary;
    dictionary?.Clear();
  }

  private HeatingService CreateService()
  {
    return new HeatingService(
      loggerMock.Object,
      garageRepositoryMock.Object,
      heatTaskRepositoryMock.Object,
      cyclicHeatTaskRepositoryMock.Object,
      outsideTemperatureRepositoryMock.Object,
      heatingLogRepositoryMock.Object,
      garageClientMock.Object,
      dateTimeProviderMock.Object);
  }

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
        new() { Id = 1, CyclicHeatTaskId = 1, Day = ((int)DateTime.Now.DayOfWeek + 3) % 7 }
      }
    };

    // Act
    var result = task.GetClosestDateTimeFromCyclicHeatTask();


    // Assert
    var currentDate = DateOnly.FromDateTime(DateTime.Now);
    var assertVal = DateOnly.FromDateTime(result);
    Assert.False(assertVal.Equals(currentDate));
  }

  [Fact]
  public void CheckStartHeatTime()
  {
    var temperatures =
      new List<GarageTemperatureDto> { new() { Id = 1, Temperature = 20 } };
    var heatTimes =
      new List<HeatTask> { new() { HeatTaskId = 1, GarageId = 1, EndTime = DateTime.Now.AddHours(5) } };
    var expectedResult =
      new List<HeatTask> { new() { HeatTaskId = 1, StartTime = DateTime.Now.AddHours(4).AddMinutes(24) } };

    var result = StartHeatingTimeCalculator.CalculateForMultipleGarages(temperatures, heatTimes);

    if (!result[0].StartTime.HasValue)
    {
      Assert.Fail("No startHeat time");
    }

    if (!expectedResult[0].StartTime.HasValue)
    {
      Assert.Fail("No expected time");
    }

    Assert.True(Math.Abs(result[0].StartTime.Value.TimeOfDay.TotalMinutes -
                         expectedResult[0].StartTime.Value.TimeOfDay.TotalMinutes) < 10);
  }

  [Fact]
  public async Task ExecuteAsync_CustomHeatTaskActive_TurnsHeaterOn()
  {
    // Arrange
    var now = new DateTime(2026, 6, 11, 12, 0, 0);
    var garage = new GarageEntity { Id = 1, Name = "Garage 1", Ip = "192.168.1.10" };
    dateTimeProviderMock.Setup(d => d.Now).Returns(now);
    garageRepositoryMock.Setup(r => r.GetGarages(It.IsAny<CancellationToken>())).ReturnsAsync([garage]);
    garageClientMock.Setup(c => c.GetGarageTemperature(garage.Ip, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new TemperatureDto { Temperature = 20 });
    heatTaskRepositoryMock.Setup(r => r.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, now, It.IsAny<CancellationToken>()))
      .ReturnsAsync([new HeatTaskEntity { Id = 1, GarageId = garage.Id, Date = now.AddMinutes(30), Active = true }]);
    cyclicHeatTaskRepositoryMock.Setup(r => r.GetActiveCyclicHeatTasks(garage.Id, It.IsAny<CancellationToken>()))
      .ReturnsAsync([]);

    var service = CreateService();

    // Act
    await service.ExecuteAsync(CancellationToken.None);

    // Assert
    garageClientMock.Verify(c => c.ChangeHeaterStatus("ON", garage.Ip, It.IsAny<CancellationToken>()), Times.Once);
    garageClientMock.Verify(c => c.ChangeHeaterStatus("OFF", garage.Ip, It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact]
  public async Task ExecuteAsync_NoActiveTask_TurnsHeaterOff()
  {
    // Arrange
    var now = new DateTime(2026, 6, 11, 12, 0, 0);
    var garage = new GarageEntity { Id = 1, Name = "Garage 1", Ip = "192.168.1.10" };
    dateTimeProviderMock.Setup(d => d.Now).Returns(now);
    garageRepositoryMock.Setup(r => r.GetGarages(It.IsAny<CancellationToken>())).ReturnsAsync([garage]);
    garageClientMock.Setup(c => c.GetGarageTemperature(garage.Ip, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new TemperatureDto { Temperature = 20 });
    heatTaskRepositoryMock.Setup(r => r.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, now, It.IsAny<CancellationToken>()))
      .ReturnsAsync([]);
    cyclicHeatTaskRepositoryMock.Setup(r => r.GetActiveCyclicHeatTasks(garage.Id, It.IsAny<CancellationToken>()))
      .ReturnsAsync([]);

    var dictionary = typeof(HeatingService).GetField("garageHeatingState",
      BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null) as
      System.Collections.Concurrent.ConcurrentDictionary<int, bool>;
    dictionary!.TryAdd(garage.Id, true);

    var service = CreateService();

    // Act
    await service.ExecuteAsync(CancellationToken.None);

    // Assert
    garageClientMock.Verify(c => c.ChangeHeaterStatus("OFF", garage.Ip, It.IsAny<CancellationToken>()), Times.Once);
    garageClientMock.Verify(c => c.ChangeHeaterStatus("ON", garage.Ip, It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact]
  public async Task ExecuteAsync_CyclicHeatTaskTodayActive_TurnsHeaterOn()
  {
    // Arrange
    var now = new DateTime(2026, 6, 11, 12, 0, 0);
    var garage = new GarageEntity { Id = 1, Name = "Garage 1", Ip = "192.168.1.10" };
    dateTimeProviderMock.Setup(d => d.Now).Returns(now);
    garageRepositoryMock.Setup(r => r.GetGarages(It.IsAny<CancellationToken>())).ReturnsAsync([garage]);
    garageClientMock.Setup(c => c.GetGarageTemperature(garage.Ip, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new TemperatureDto { Temperature = 20 });
    heatTaskRepositoryMock.Setup(r => r.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, now, It.IsAny<CancellationToken>()))
      .ReturnsAsync([]);
    cyclicHeatTaskRepositoryMock.Setup(r => r.GetActiveCyclicHeatTasks(garage.Id, It.IsAny<CancellationToken>()))
      .ReturnsAsync([
        new CyclicHeatTaskEntity
        {
          Id = 1,
          GarageId = garage.Id,
          Time = now.AddMinutes(30).TimeOfDay,
          Active = true,
          CyclicHeatTaskDays = [new CyclicHeatTaskDayEntity { Day = (int)now.DayOfWeek }]
        }
      ]);

    var service = CreateService();

    // Act
    await service.ExecuteAsync(CancellationToken.None);

    // Assert
    garageClientMock.Verify(c => c.ChangeHeaterStatus("ON", garage.Ip, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task ExecuteAsync_CyclicHeatTaskAlreadyPassed_TurnsHeaterOff()
  {
    // Arrange
    var now = new DateTime(2026, 6, 11, 12, 0, 0);
    var garage = new GarageEntity { Id = 1, Name = "Garage 1", Ip = "192.168.1.10" };
    dateTimeProviderMock.Setup(d => d.Now).Returns(now);
    garageRepositoryMock.Setup(r => r.GetGarages(It.IsAny<CancellationToken>())).ReturnsAsync([garage]);
    garageClientMock.Setup(c => c.GetGarageTemperature(garage.Ip, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new TemperatureDto { Temperature = 20 });
    heatTaskRepositoryMock.Setup(r => r.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, now, It.IsAny<CancellationToken>()))
      .ReturnsAsync([]);
    cyclicHeatTaskRepositoryMock.Setup(r => r.GetActiveCyclicHeatTasks(garage.Id, It.IsAny<CancellationToken>()))
      .ReturnsAsync([
        new CyclicHeatTaskEntity
        {
          Id = 1,
          GarageId = garage.Id,
          Time = now.AddHours(-1).TimeOfDay,
          Active = true,
          CyclicHeatTaskDays = [new CyclicHeatTaskDayEntity { Day = (int)now.DayOfWeek }]
        }
      ]);

    var dictionary = typeof(HeatingService).GetField("garageHeatingState",
      BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null) as
      System.Collections.Concurrent.ConcurrentDictionary<int, bool>;
    dictionary!.TryAdd(garage.Id, true);

    var service = CreateService();

    // Act
    await service.ExecuteAsync(CancellationToken.None);

    // Assert
    garageClientMock.Verify(c => c.ChangeHeaterStatus("OFF", garage.Ip, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task ExecuteAsync_StateAlreadyMatches_DoesNotCallClient()
  {
    // Arrange
    var now = new DateTime(2026, 6, 11, 12, 0, 0);
    var garage = new GarageEntity { Id = 1, Name = "Garage 1", Ip = "192.168.1.10" };
    dateTimeProviderMock.Setup(d => d.Now).Returns(now);
    garageRepositoryMock.Setup(r => r.GetGarages(It.IsAny<CancellationToken>())).ReturnsAsync([garage]);
    garageClientMock.Setup(c => c.GetGarageTemperature(garage.Ip, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new TemperatureDto { Temperature = 20 });
    heatTaskRepositoryMock.Setup(r => r.GetActiveHeatTaskForGarageIdFromFuture(garage.Id, now, It.IsAny<CancellationToken>()))
      .ReturnsAsync([]);
    cyclicHeatTaskRepositoryMock.Setup(r => r.GetActiveCyclicHeatTasks(garage.Id, It.IsAny<CancellationToken>()))
      .ReturnsAsync([]);

    var dictionary = typeof(HeatingService).GetField("garageHeatingState",
      BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null) as
      System.Collections.Concurrent.ConcurrentDictionary<int, bool>;
    dictionary!.TryAdd(garage.Id, false);

    var service = CreateService();

    // Act
    await service.ExecuteAsync(CancellationToken.None);

    // Assert
    garageClientMock.Verify(c => c.ChangeHeaterStatus(It.IsAny<string>(), garage.Ip, It.IsAny<CancellationToken>()), Times.Never);
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

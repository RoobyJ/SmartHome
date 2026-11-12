using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Repositories;
using Core.Dtos;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
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

  private readonly HeatingState state = new();
  private readonly List<HeatTaskEntity> customTasks = [];
  private readonly List<CyclicHeatTaskEntity> cyclicTasks = [];

  private static readonly DateTime Now = new(2026, 6, 11, 12, 0, 0);

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
      dateTimeProviderMock.Object,
      state);
  }

  private static GarageEntity Garage(int id) => new() { Id = id, Name = $"Garage {id}", Ip = $"192.168.1.{id}" };

  /// <summary>Sets up garages with a temperature, heater status and no tasks by default.</summary>
  private void SetupGarages(DateTime now, bool heaterOn, float? temperature, params GarageEntity[] garages)
  {
    dateTimeProviderMock.Setup(d => d.Now).Returns(now);
    garageRepositoryMock.Setup(r => r.GetGarages(It.IsAny<CancellationToken>())).ReturnsAsync(garages);
    heatTaskRepositoryMock
      .Setup(r => r.GetActiveHeatTasksFromFuture(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(() => customTasks);
    cyclicHeatTaskRepositoryMock.Setup(r => r.GetActiveCyclicHeatTasks(It.IsAny<CancellationToken>()))
      .ReturnsAsync(() => cyclicTasks);
    foreach (var garage in garages)
    {
      SetTemperature(garage, temperature);
      SetHeaterStatus(garage, heaterOn);
    }
  }

  private void SetTemperature(GarageEntity garage, float? temperature)
  {
    garageClientMock.Setup(c => c.GetGarageTemperature(garage.Ip, It.IsAny<CancellationToken>()))
      .ReturnsAsync(temperature == null ? null : new TemperatureDto { Temperature = temperature.Value });
  }

  private void SetHeaterStatus(GarageEntity garage, bool? on)
  {
    garageClientMock.Setup(c => c.GetHeaterStatus(garage.Ip, It.IsAny<CancellationToken>()))
      .ReturnsAsync(on == null ? null : new GarageHeaterStatusDto { HeatingStatus = on.Value });
  }

  private void SetCustomTask(GarageEntity garage, DateTime end)
  {
    customTasks.Clear();
    customTasks.Add(new HeatTaskEntity { Id = 1, GarageId = garage.Id, Date = end, Active = true });
  }

  private void SetCyclicTask(GarageEntity garage, TimeSpan time, int day)
  {
    cyclicTasks.Clear();
    cyclicTasks.Add(new CyclicHeatTaskEntity
    {
      Id = 1,
      GarageId = garage.Id,
      Time = time,
      Active = true,
      CyclicHeatTaskDays = [new CyclicHeatTaskDayEntity { Day = day }]
    });
  }

  private void VerifySent(GarageEntity garage, string status, Times times)
  {
    garageClientMock.Verify(c => c.ChangeHeaterStatus(status, garage.Ip, It.IsAny<CancellationToken>()), times);
  }

  private void VerifyNoErrors()
  {
    loggerMock.Verify(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
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

  // Finding #10: fixed dates instead of DateTime.Now, including a window that crosses midnight.
  [Theory]
  [InlineData(12, 0)]
  [InlineData(0, 10)]
  public void CheckStartHeatTime(int endHour, int endMinute)
  {
    var end = new DateTime(2026, 6, 11, endHour, endMinute, 0);

    var start = StartHeatingTimeCalculator.CalculateStartTime(end, 20);

    Assert.NotNull(start);
    Assert.InRange((end - start.Value).TotalMinutes, 39, 41); // ~39.8 min at 20 C
  }

  [Fact]
  public void CalculateStartTime_UnknownOrNaNTemperature_ReturnsNull()
  {
    var end = new DateTime(2026, 6, 11, 12, 0, 0);

    Assert.Null(StartHeatingTimeCalculator.CalculateStartTime(end, null));
    Assert.Null(StartHeatingTimeCalculator.CalculateStartTime(end, float.NaN));
  }

  // Finding #8: a hot reading must not produce a start time hours before the end.
  [Theory]
  [InlineData(85)]
  [InlineData(200)]
  public void CalculateStartTime_VeryHot_NoPreheat(float temperature)
  {
    var end = new DateTime(2026, 6, 11, 12, 30, 0);

    Assert.Equal(end, StartHeatingTimeCalculator.CalculateStartTime(end, temperature));
  }

  // Finding #8: a very cold reading is capped at MaxLeadTime.
  [Fact]
  public void CalculateStartTime_VeryCold_ClampedToMaxLead()
  {
    var end = new DateTime(2026, 6, 11, 6, 0, 0);

    Assert.Equal(end - StartHeatingTimeCalculator.MaxLeadTime, StartHeatingTimeCalculator.CalculateStartTime(end, -127));
  }

  // Finding #8: sensor fault values (DS18B20 -127 / 85) are ignored: no heating decision and not stored.
  [Theory]
  [InlineData(-127)]
  [InlineData(85)]
  public async Task ExecuteAsync_ImplausibleTemperature_IgnoredAndNotSaved(float temperature)
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: false, temperature: temperature, garage);
    SetCustomTask(garage, Now.AddHours(3)); // -127 would otherwise start heating now

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "ON", Times.Never());
    outsideTemperatureRepositoryMock.Verify(
      r => r.AddTemperatures(It.IsAny<List<OutsideTemperatureEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  // Finding #12: device reads for all garages run concurrently.
  [Fact]
  public async Task ExecuteAsync_ReadsAllGaragesConcurrently()
  {
    var g1 = Garage(1);
    var g2 = Garage(2);
    SetupGarages(Now, heaterOn: false, temperature: 20, g1, g2);
    var g1Release = new TaskCompletionSource<TemperatureDto>();
    var g2Started = new TaskCompletionSource();
    garageClientMock.Setup(c => c.GetGarageTemperature(g1.Ip, It.IsAny<CancellationToken>()))
      .Returns(() => g1Release.Task);
    garageClientMock.Setup(c => c.GetGarageTemperature(g2.Ip, It.IsAny<CancellationToken>()))
      .Returns(() =>
      {
        g2Started.SetResult();
        return Task.FromResult(new TemperatureDto { Temperature = 20 });
      });

    var run = CreateService().ExecuteAsync(CancellationToken.None);
    var winner = await Task.WhenAny(g2Started.Task, Task.Delay(2000));
    g1Release.SetResult(new TemperatureDto { Temperature = 20 });
    await run;

    Assert.Same(g2Started.Task, winner);
  }

  [Fact]
  public async Task ExecuteAsync_CustomHeatTaskActive_TurnsHeaterOn()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);
    SetCustomTask(garage, Now.AddMinutes(30));

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "ON", Times.Once());
    VerifySent(garage, "OFF", Times.Never());
    VerifyNoErrors();
  }

  [Fact]
  public async Task ExecuteAsync_NoActiveTask_TurnsHeaterOff()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: true, temperature: 20, garage);

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Once());
    VerifySent(garage, "ON", Times.Never());
    VerifyNoErrors();
  }

  [Fact]
  public async Task ExecuteAsync_CyclicHeatTaskTodayActive_TurnsHeaterOn()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);
    SetCyclicTask(garage, Now.AddMinutes(30).TimeOfDay, (int)Now.DayOfWeek);

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "ON", Times.Once());
    VerifyNoErrors();
  }

  [Fact]
  public async Task ExecuteAsync_CyclicHeatTaskAlreadyPassed_TurnsHeaterOff()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: true, temperature: 20, garage);
    SetCyclicTask(garage, Now.AddHours(-1).TimeOfDay, (int)Now.DayOfWeek);

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Once());
    VerifyNoErrors();
  }

  [Fact]
  public async Task ExecuteAsync_StateAlreadyMatches_DoesNotCallClient()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);

    await CreateService().ExecuteAsync(CancellationToken.None);

    garageClientMock.Verify(c => c.ChangeHeaterStatus(It.IsAny<string>(), garage.Ip, It.IsAny<CancellationToken>()),
      Times.Never);
    VerifyNoErrors();
  }

  // Finding #1: one garage's failing temperature read must not block another garage.
  [Fact]
  public async Task ExecuteAsync_OtherGarageTemperatureReadThrows_StillTurnsHeaterOff()
  {
    var broken = Garage(1);
    var heating = Garage(2);
    SetupGarages(Now, heaterOn: true, temperature: 20, broken, heating);
    garageClientMock.Setup(c => c.GetGarageTemperature(broken.Ip, It.IsAny<CancellationToken>()))
      .ThrowsAsync(new HttpRequestException("No route to host"));

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(heating, "OFF", Times.Once());
  }

  // Finding #1: a DB error when saving temperatures must not block switching.
  [Fact]
  public async Task ExecuteAsync_SavingTemperaturesThrows_StillTurnsHeaterOff()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: true, temperature: 20, garage);
    outsideTemperatureRepositoryMock
      .Setup(r => r.AddTemperatures(It.IsAny<List<OutsideTemperatureEntity>>(), It.IsAny<CancellationToken>()))
      .ThrowsAsync(new InvalidOperationException("db down"));

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Once());
  }

  // Finding #2: a heater found ON on the device (UI toggle, worker restart) with no task is switched OFF,
  // even though this process never switched it ON.
  [Fact]
  public async Task ExecuteAsync_HeaterOnOnDeviceWithFreshState_TurnsHeaterOff()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: true, temperature: null, garage);

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Once());
  }

  // Finding #2: a controller that rebooted with the relay OFF mid-window gets ON re-sent.
  [Fact]
  public async Task ExecuteAsync_DeviceLostOnStateMidWindow_ResendsOn()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);
    SetCustomTask(garage, Now.AddMinutes(30));
    state.LastCommanded[garage.Id] = true;

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "ON", Times.Once());
  }

  // Finding #2: if the device status cannot be read, the desired state is still enforced.
  [Fact]
  public async Task ExecuteAsync_HeaterStatusUnreadable_SendsDesiredStateAnyway()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);
    SetHeaterStatus(garage, null);

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Once());
  }

  // Finding #4: once a window has started, a missing reading does not switch the heater OFF before the end time.
  [Fact]
  public async Task ExecuteAsync_TemperatureMissingMidWindow_KeepsHeaterOn()
  {
    var garage = Garage(1);
    var end = Now.AddMinutes(30);
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);
    SetCustomTask(garage, end);
    await CreateService().ExecuteAsync(CancellationToken.None);
    VerifySent(garage, "ON", Times.Once());

    dateTimeProviderMock.Setup(d => d.Now).Returns(Now.AddSeconds(30));
    SetTemperature(garage, null);
    SetHeaterStatus(garage, true);
    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Never());
  }

  // Finding #4: a temperature rise that moves the computed start later does not switch the heater OFF.
  [Fact]
  public async Task ExecuteAsync_TemperatureRisesMidWindow_KeepsHeaterOn()
  {
    var garage = Garage(1);
    var end = Now.AddMinutes(35); // 20 C -> ~40 min lead, so the window has started; 60 C -> ~6 min
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);
    SetCustomTask(garage, end);
    await CreateService().ExecuteAsync(CancellationToken.None);
    VerifySent(garage, "ON", Times.Once());

    dateTimeProviderMock.Setup(d => d.Now).Returns(Now.AddSeconds(30));
    SetTemperature(garage, 60);
    SetHeaterStatus(garage, true);
    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Never());
  }

  // Finding #4: the latch ends with the window.
  [Fact]
  public async Task ExecuteAsync_LatchedWindowEnded_TurnsHeaterOff()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: false, temperature: 20, garage);
    SetCustomTask(garage, Now.AddMinutes(30));
    await CreateService().ExecuteAsync(CancellationToken.None);

    dateTimeProviderMock.Setup(d => d.Now).Returns(Now.AddMinutes(31));
    customTasks.Clear();
    SetHeaterStatus(garage, true);
    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Once());
  }

  // Finding #5: a failing PATCH for one garage does not skip later garages.
  [Fact]
  public async Task ExecuteAsync_FirstGaragePatchThrows_SecondGarageStillSwitched()
  {
    var broken = Garage(1);
    var other = Garage(2);
    SetupGarages(Now, heaterOn: true, temperature: 20, broken, other);
    garageClientMock.Setup(c => c.ChangeHeaterStatus(It.IsAny<string>(), broken.Ip, It.IsAny<CancellationToken>()))
      .ThrowsAsync(new TaskCanceledException());

    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(other, "OFF", Times.Once());
  }

  // Finding #6: a rejected command (client throws) is not recorded as applied, and is retried next tick.
  [Fact]
  public async Task ExecuteAsync_CommandRejected_NotLoggedAndRetried()
  {
    var garage = Garage(1);
    SetupGarages(Now, heaterOn: true, temperature: 20, garage);
    garageClientMock.Setup(c => c.ChangeHeaterStatus("OFF", garage.Ip, It.IsAny<CancellationToken>()))
      .ThrowsAsync(new HttpRequestException("500 Internal Server Error"));

    await CreateService().ExecuteAsync(CancellationToken.None);
    await CreateService().ExecuteAsync(CancellationToken.None);

    VerifySent(garage, "OFF", Times.Exactly(2));
    heatingLogRepositoryMock.Verify(r => r.AddHeatLog(It.IsAny<HeatLogEntity>(), It.IsAny<CancellationToken>()),
      Times.Never);
    Assert.False(state.LastCommanded.ContainsKey(garage.Id));
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

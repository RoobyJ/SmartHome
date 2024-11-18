using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.Hosting;

namespace SmartHome.Worker;

/// <summary>
///   The Worker is a BackgroundService that is executed periodically
///   It should not contain any business logic but should call an entrypoint service that
///   execute once per time period.
/// </summary>
public class Worker(
  ILoggerAdapter<Worker> logger,
  IHeatingService heatingService,
  WorkerSettings settings)
  : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    logger.LogInformation("SmartHome.Worker service starting at: {time}", DateTimeOffset.Now);

    while (!ct.IsCancellationRequested)
    {
      var timer = new PeriodicTimer(TimeSpan.FromSeconds(settings.DelaySeconds));
      while (await timer.WaitForNextTickAsync(ct))
      {
        await heatingService.ExecuteAsync(ct);
      }
    }

    logger.LogInformation("SmartHome.Worker service stopping at: {time}", DateTimeOffset.Now);
  }
}

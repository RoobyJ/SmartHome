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
public class Worker : BackgroundService
{
  private readonly IHeatingService _heatingService;
  private readonly ILoggerAdapter<Worker> _logger;
  private readonly WorkerSettings _settings;


  public Worker(ILoggerAdapter<Worker> logger,
    IHeatingService heatingService,
    WorkerSettings settings)
  {
    _logger = logger;
    _heatingService = heatingService;
    _settings = settings;
  }

  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    _logger.LogInformation("SmartHome.Worker service starting at: {time}", DateTimeOffset.Now);

    while (!ct.IsCancellationRequested)
    {
      var timer = new PeriodicTimer(TimeSpan.FromSeconds(_settings.DelaySeconds));
      while (await timer.WaitForNextTickAsync(ct))
      {
        await _heatingService.ExecuteAsync(ct);
      }
    }

    _logger.LogInformation("SmartHome.Worker service stopping at: {time}", DateTimeOffset.Now);
  }
}

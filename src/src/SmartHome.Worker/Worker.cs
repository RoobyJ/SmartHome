﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SmartHome.Core.Interfaces;
using SmartHome.Heater.Interfaces;

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

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("SmartHome.Worker service starting at: {time}", DateTimeOffset.Now);
    // TODO: implement here setting at start up status for each garage
    while (!stoppingToken.IsCancellationRequested)
    {
      var timer = new PeriodicTimer(TimeSpan.FromSeconds(_settings.DelaySeconds));
      while (await timer.WaitForNextTickAsync(stoppingToken))
      {
        await _heatingService.ExecuteAsync();
      }
    }

    _logger.LogInformation("SmartHome.Worker service stopping at: {time}", DateTimeOffset.Now);
  }
}
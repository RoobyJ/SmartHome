using System;
using Core;
using Core.Helpers;
using Core.Interfaces;
using Core.Services;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace SmartHome.Worker;

public abstract class Program
{
  public static void Main(string[] args)
  {
    var logger = LogManager.Setup().GetCurrentClassLogger();
    logger.Info("Application is starting...");
    logger.Info("Current version: {Version}", Constants.Version);

    try
    {
      var host = CreateHostBuilder(args).Build();
      host.Run();
    }
    catch (Exception exception)
    {
      logger.Error(exception, "Stopped program because of exception");
      throw;
    }
    finally
    {
      // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
      LogManager.Shutdown();
    }
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    return Host.CreateDefaultBuilder(args)
      .ConfigureServices((hostContext, services) =>
      {
        services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        services.AddSingleton<IHeatingService, HeatingService>();
        services.AddTransient<StartHeatingTimeCalculator>();
        services.AddCore();

        var workerSettings = new WorkerSettings();
        services.AddInfrastructure(hostContext.Configuration);
        hostContext.Configuration.Bind(nameof(WorkerSettings), workerSettings);
        services.AddSingleton(workerSettings);

        services.AddHostedService<Worker>();
      });
  }
}

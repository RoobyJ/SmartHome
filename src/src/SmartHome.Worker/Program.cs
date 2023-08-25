﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Services;
using SmartHome.Core.Settings;
using SmartHome.Heater.Interfaces;
using SmartHome.Heater.Services;
using SmartHome.Heater.Settings;
using SmartHome.Infrastructure;

namespace SmartHome.Worker;

public class Program
{
  public static void Main(string[] args)
  {
    var host = CreateHostBuilder(args).Build();
    host.Run();
  }

  private static IHostBuilder CreateHostBuilder(string[] args)
  {
    return Host.CreateDefaultBuilder(args)
      .ConfigureServices((hostContext, services) =>
      {
        services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        services.AddSingleton<IHeatingService, HeatingService>();
        services.AddSingleton<IServiceLocator, ServiceScopeFactoryLocator>();

        // Infrastructure.ContainerSetup
        services.AddDbContext(hostContext.Configuration);
        services.AddRepositories();
        services.AddUrlCheckingServices();
        services.AddMemoryCache();

        var workerSettings = new WorkerSettings();
        hostContext.Configuration.Bind(nameof(WorkerSettings), workerSettings);
        services.AddSingleton(workerSettings);
        
        var heatingSettings = new HeatingSettings();
        hostContext.Configuration.Bind(nameof(HeatingSettings), heatingSettings);
        services.AddSingleton(heatingSettings);

        services.AddHostedService<Worker>();
      });
  }
}
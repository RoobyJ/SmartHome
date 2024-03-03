using Core.Interfaces;
using Core.Services;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHome.Core.Helpers;
using SmartHome.Core.Services;
using SmartHome.Heater.Settings;

namespace SmartHome.Worker;

public abstract class Program
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
        services.AddTransient<StartHeatingTimeCalculator>();

        // Infrastructure.ContainerSetup
        services.AddDbContext(hostContext.Configuration);
        services.AddRepositories();
        services.AddUrlCheckingServices();

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

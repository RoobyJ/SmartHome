using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHome.TasksManager.Core.Interfaces;
using SmartHome.TasksManager.Core.Services;
using SmartHome.TasksManager.Core.Settings;
using SmartHome.TasksManager.Heater.Interfaces;
using SmartHome.TasksManager.Heater.Services;
using SmartHome.TasksManager.Heater.Settings;
using SmartHome.TasksManager.Infrastructure;

namespace SmartHome.TasksManager.Worker;

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

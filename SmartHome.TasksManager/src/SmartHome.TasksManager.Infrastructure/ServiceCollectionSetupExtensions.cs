using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.TasksManager.Core.Interfaces;
using SmartHome.TasksManager.Infrastructure.Data;
using SmartHome.TasksManager.Infrastructure.Http;

namespace SmartHome.TasksManager.Infrastructure;

public static class ServiceCollectionSetupExtensions
{
  public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<SmartHomeDbContext>(options =>
      options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection")));
  }

  public static void AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IRepository, EfRepository>();
    services.AddScoped<IJsonFileReader, JsonFileReader>();
  }

  public static void AddUrlCheckingServices(this IServiceCollection services)
  {
    //services.AddTransient<IUrlStatusChecker, UrlStatusChecker>();
    services.AddTransient<IHttpService, HttpService>();
  }
}

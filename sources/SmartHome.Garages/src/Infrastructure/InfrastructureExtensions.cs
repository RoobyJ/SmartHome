using System.Threading.Tasks;
using Core.Common.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext(configuration);
    services.AddRepositories();
  }

  public static async Task MigrateDatabase(this IApplicationBuilder app)
  {
    using var scope = app.ApplicationServices.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<SmartHomeDbContextInitializer>();
    await initializer.InitializeAsync();
  }

  #region private methods

  private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<SmartHomeDbContext>(options =>
      options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection"))).AddScoped<SmartHomeDbContextInitializer>();
  }

  private static void AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IGarageRepository, GarageRepository>();
    services.AddScoped<IHeatingLogRepository, HeatLogRepository>();
    services.AddScoped<IHeatTaskRepository, HeatTaskRepository>();
    services.AddScoped<ICyclicHeatTaskRepository, CyclicHeatTaskRepository>();
    services.AddScoped<ICyclicHeatTaskDayRepository, CyclicHeatTaskDayRepository>();
    services.AddScoped<IOutsideTemperatureRepository, OutsideTemperaturesRepository>();
  }

  #endregion
}

﻿using System.Threading.Tasks;
using Core.Common.Repositories;
using Infrastructure.Http;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure;

public static class ServiceCollectionSetupExtensions
{
  public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<SmartHomeDbContext>(options =>
      options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection"))).AddScoped<SmartHomeDbContextInitializer>();
  }

  public static async Task MigrateDatabase(this IApplicationBuilder app)
  {
    using var scope = app.ApplicationServices.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<SmartHomeDbContextInitializer>();
    await initializer.InitializeAsync();
  }

  public static void AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IGarageRepository, GarageRepository>();
    services.AddScoped<IHeatingLogRepository, HeatLogRepository>();
    services.AddScoped<IHeatTaskRepository<HeatTask>, HeatTaskRepository>();
    services.AddScoped<ICyclicHeatTaskRepository<CyclicHeatTask>, CyclicHeatTaskRepository>();
    services.AddScoped<IOutsideTemperatureRepository<OutsideTemperature>, OutsideTemperaturesRepository>();
  }

  public static void AddUrlCheckingServices(this IServiceCollection services)
  {
    services.AddTransient<IHttpService, HttpService>();
  }
}

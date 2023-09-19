﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Core.Interfaces;
using SmartHome.Infrastructure.Data;
using SmartHome.Infrastructure.Http;

namespace SmartHome.Infrastructure;

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
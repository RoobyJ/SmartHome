using Core.Helpers;
using Core.Interfaces;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class CoreExtensions
{
  public static void AddCore(this IServiceCollection services)
  {
    services.AddScoped<IGarageService, GarageService>();
    services.AddScoped<IHeatTaskService, HeatTaskService>();
    services.AddScoped<IGarageClient, GarageClient>();
  }
}

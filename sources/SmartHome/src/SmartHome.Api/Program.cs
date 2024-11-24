using Core;
using Infrastructure;
using NLog;
using NLog.Web;
using SmartHome.Api;

var logger = LogManager.Setup().GetCurrentClassLogger();
logger.Info("Application is starting...");
logger.Info("Current version: {Version}", Constants.Version);

try
{
  var builder = WebApplication.CreateBuilder(args);
  
  builder.Logging.ClearProviders();
  builder.Host.UseNLog();

  var migrateOnStartUp = builder.Configuration.GetValue<Boolean>("MigrateOnStartUp");
// Add services to the container.

  builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(c => { c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); });

  builder.Services.AddInfrastructure(builder.Configuration);
  builder.Services.AddCore();

  var app = builder.Build();

// Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  app.MapControllers();

  if (migrateOnStartUp)
  {
    await app.MigrateDatabase();
  }

  if (!app.Environment.IsDevelopment())
  {
    app.UseHttpsRedirection();
  }


  app.Run();
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


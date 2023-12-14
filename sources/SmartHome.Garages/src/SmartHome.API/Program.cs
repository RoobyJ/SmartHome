using Core.Interfaces;
using Core.Services;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Services;
using SmartHome.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); });

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddRepositories();

// TODO: move this into CoreExtensions.cs file
builder.Services.AddScoped<IGarageService, GarageService>();
builder.Services.AddScoped<IHeatTaskService, HeatTaskService>();
builder.Services.AddScoped<IGarageClient, GarageClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
  await app.MigrateDatabase();
}


app.Run();

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

builder.Services.AddScoped<IGarageService, GarageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

if (builder.Environment.IsDevelopment())
{
  app.UseCors();
}

if (!app.Environment.IsDevelopment())
{
  app.UseHttpsRedirection();
}


app.Run();

using Core;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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

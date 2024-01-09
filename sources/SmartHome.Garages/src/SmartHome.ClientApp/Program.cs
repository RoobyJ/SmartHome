var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseHsts();
}

app.UseHttpsRedirection();

var enableHttpLogging = builder.Configuration["EnableHttpLogging"] == "True";
if (enableHttpLogging) app.UseHttpLogging();

app.UseRouting();


// serve static files as a fallback, so if route has not matched any configured reverse proxy path
// then emit static files (SPA app) and fallback to index.html
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();

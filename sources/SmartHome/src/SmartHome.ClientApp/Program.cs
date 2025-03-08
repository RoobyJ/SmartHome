using IdentityModel.Client;
using Microsoft.VisualBasic;
using OidcProxy.Net;
using OidcProxy.Net.ModuleInitializers;
using OidcProxy.Net.OpenIdConnect;
using SmartHome.Client;
using SmartHome.Client.Models;
using Constants = SmartHome.Client.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Configuration.AddEnvironmentVariables(); 
builder.Services.AddReverseProxy()
  .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var dataProtectionOptions = builder.Configuration
  .GetSection("DataProtectionOptions")
  .Get<DataProtectionOptions?>();

builder.Services.ConfigureDataProtection(dataProtectionOptions);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseHsts();
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();

var enableHttpLogging = builder.Configuration["EnableHttpLogging"] == "True";
if (enableHttpLogging)
{
  app.UseHttpLogging();
}

app.UseRouting();

// serve static files as a fallback, so if route has not matched any configured reverse proxy path
// then emit static files (SPA app) and fallback to index.html
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.MapReverseProxy();

app.Run();

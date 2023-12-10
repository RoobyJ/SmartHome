using GoCloudNative.Bff.Authentication.ModuleInitializers;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddSecurityBff(options =>
  {
    var customHostName = builder.Configuration.GetValue<string?>("ReplaceHostWithUri");
    if (!string.IsNullOrWhiteSpace(customHostName))
    {
      options.SetCustomHostName(new Uri(customHostName));
    }

    options.SessionCookieName = "smarthome.session";
  })
  .AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseHsts();
}

app.UseHttpsRedirection();

// Enable for debugging the proxied requests
// app.UseHttpLogging();

app.UseRouting();

// serve static files as a fallback, so if route has not matched any configured reverse proxy path
// then emit static files (SPA app) and fallback to index.html
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();

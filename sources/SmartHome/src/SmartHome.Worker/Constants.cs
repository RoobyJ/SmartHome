using System.Reflection;

namespace SmartHome.Worker;

public static class Constants
{
  public static string Version => Assembly.GetEntryAssembly()
    ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
    ?.InformationalVersion ?? "unknown version";
}

using System.Reflection;

namespace SmartHome.Api;

public class Constants
{
  public static string Version => Assembly.GetEntryAssembly()
    ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
    ?.InformationalVersion ?? "unknown version";
}

using System.Reflection;

namespace SmartHome.Client;

internal static class Constants
{
    public const string ApiClient = "ApiHttpClient";

    public static string Version => Assembly.GetEntryAssembly()
                                            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                            ?.InformationalVersion ?? "unknown version";
}

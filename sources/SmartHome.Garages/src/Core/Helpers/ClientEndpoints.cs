namespace SmartHome.Core.Helpers;

public static class ClientEndpoints
{
  public static class Garage
  {
    private static string GetBase(string ip)
    {
      return $"http://{ip}";
    }

    public static string Base(string ip)
    {
      return GetBase(ip);
    }

    public static string Temperature(string ip)
    {
      return GetBase(ip) + "/temperature";
    }
    
    public static string Heater(string ip)
    {
      return GetBase(ip) + "/heater";
    }
    
    public static string HeaterStatus(string ip)
    {
      return GetBase(ip) + "/heater-status";
    }
  }
}

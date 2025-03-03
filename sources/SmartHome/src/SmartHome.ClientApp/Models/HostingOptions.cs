namespace SmartHome.Client.Models;

public class HostingOptions
{
    public bool UseHttpsRedirection { get; set; }

    public bool UseHsts { get; set; }

    public bool UseHttpLogging { get; set; }

    public string? ReverseProxyAddress { get; set; }

    public string? PathBase { get; set; }
}

using System.Net;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using SmartHome.Client.Models;
using DataProtectionOptions = SmartHome.Client.Models.DataProtectionOptions;

namespace SmartHome.Client;

using DataProtectionOptions = DataProtectionOptions;

public static class Extensions
{
    public static IServiceCollection ConfigureHttpLogging(this IServiceCollection services,
                                                          HostingOptions hostingOptions)
    {
        return hostingOptions.UseHttpLogging ? services.AddHttpLogging(_ => { }) : services;
    }

    public static IServiceCollection ConfigureForwardedHeaders(this IServiceCollection services,
                                                               HostingOptions hostingOptions)
    {
        return services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();

            if (!string.IsNullOrWhiteSpace(hostingOptions.ReverseProxyAddress))
            {
                options.KnownProxies.Add(IPAddress.Parse(hostingOptions.ReverseProxyAddress));
            }
        });
    }

    public static IServiceCollection ConfigureDataProtection(this IServiceCollection services, DataProtectionOptions? options)
    {
        if (options == null) return services;

        var directory = new DirectoryInfo(options.PersistKeysToFileSystemDirectory);
        if (!directory.Exists) directory.Create();

        var builder = services.AddDataProtection()
                              .PersistKeysToFileSystem(directory);

        var signingCertificate = options.ProtectWithCertificate.FindCertificate();
        builder.ProtectKeysWithCertificate(signingCertificate);

        options.UnprotectWithCertificates.ForEach(cert => { builder.UnprotectKeysWithAnyCertificate(cert.FindCertificate()); });

        return services;
    }
}

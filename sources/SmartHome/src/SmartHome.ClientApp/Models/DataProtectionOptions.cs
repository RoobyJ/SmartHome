namespace SmartHome.Client.Models;

using CryptographyHelper.Certificates;

public class DataProtectionOptions
{
    public string PersistKeysToFileSystemDirectory { get; set; } = default!;

    public required CertificateOption ProtectWithCertificate { get; set; }

    /// <summary>
    /// Certificates that are no longer used for protection but still can be used to unprotect data.
    /// Read more: https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview
    /// </summary>
    public required List<CertificateOption> UnprotectWithCertificates { get; set; } = [];
}

namespace CMPL.DatacarWeb.ClientApp.Services;

using GoCloudNative.Bff.Authentication.IdentityProviders;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using TheCloudNativeWebApp.Bff.Authentication.OpenIdConnect;

public class IdentityProvider : OpenIdConnectIdentityProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public IdentityProvider(IMemoryCache cache, HttpClient httpClient, OpenIdConnectConfig configuration,
                            IHttpContextAccessor httpContextAccessor)
        : base(cache, httpClient, configuration)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AuthorizeRequest> GetAuthorizeUrlAsync(string redirectUri)
    {
        var authorizeRequest = await base.GetAuthorizeUrlAsync(redirectUri);

        var queryParam = this.GetUiLocalesQueryParam();
        if (queryParam == null) return authorizeRequest;

        var newUri = QueryHelpers.AddQueryString(authorizeRequest.AuthorizeUri.ToString(), queryParam);
        return new AuthorizeRequest(new Uri(newUri), authorizeRequest.CodeVerifier!);
    }

    protected override async Task<Uri> BuildEndSessionUri(string? idToken, string redirectUri)
    {
        var uri = await base.BuildEndSessionUri(idToken, redirectUri);

        var queryParam = this.GetUiLocalesQueryParam();
        if (queryParam == null) return uri;

        var newUri = QueryHelpers.AddQueryString(uri.ToString(), queryParam);
        return new Uri(newUri);
    }

    #region Private methods
    private Dictionary<string, StringValues>? GetUiLocalesQueryParam()
    {
        if (this.httpContextAccessor.HttpContext == null) return null;

        var context = this.httpContextAccessor.HttpContext;
        if (!context.Request.Query.TryGetValue("ui_locales", out var acceptLang))
        {
            return null;
        }

        return new Dictionary<string, StringValues>
        {
            { "ui_locales", acceptLang }
        };
    }
    #endregion
}
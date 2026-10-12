using Microsoft.AspNetCore.Localization;

namespace ManuelaRuiz.Web.Services;

public sealed class RouteLocaleProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var locale = httpContext.Items["Locale"] as string;
        var culture = string.Equals(locale, "en", StringComparison.OrdinalIgnoreCase) ? "en-US" : "es-ES";
        return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture, culture));
    }
}

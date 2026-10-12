using System.Globalization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using ManuelaRuiz.Web.Middleware;
using ManuelaRuiz.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ManuelaRuiz.Web.Services.HubLatestPostClient>();
builder.Services.AddSingleton<LeadStore>();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supported = new[] { new CultureInfo("es-ES"), new CultureInfo("en-US") };
    options.DefaultRequestCulture = new RequestCulture("es-ES");
    options.SupportedCultures = supported;
    options.SupportedUICultures = supported;
    options.RequestCultureProviders =
    [
        new RouteLocaleProvider()
    ];
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseMiddleware<HostCanonicalMiddleware>();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.CacheControl = "public,max-age=604800,stale-while-revalidate=86400";
    }
});
app.UseMiddleware<EnglishPrefixMiddleware>();
app.UseMiddleware<SeoAliasMiddleware>();
app.UseRequestLocalization();
app.Use(async (ctx, next) =>
{
    ctx.Response.OnStarting(() =>
    {
        var lang = SiteLocale.IsEnglish(ctx) ? "en-US" : "es-US";
        ctx.Response.Headers.ContentLanguage = lang;
        ctx.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
        return Task.CompletedTask;
    });
    await next();
});
app.UseRouting();
app.UseAuthorization();

app.MapMethods("/sitemap.xml", ["GET", "HEAD"], (HttpContext http) =>
    Results.Text(SeoJson.Sitemap(http), "application/xml; charset=utf-8"));
app.MapMethods("/robots.txt", ["GET", "HEAD"], (HttpContext http) =>
    Results.Text(SeoJson.Robots(http), "text/plain; charset=utf-8"));
app.MapMethods("/llms.txt", ["GET", "HEAD"], (HttpContext http) =>
    Results.Text(SeoJson.Llms(http), "text/plain; charset=utf-8"));

app.MapGet("/lang/{code}", (string code, string? returnUrl) =>
{
    var path = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
    if (!path.StartsWith('/') || path.StartsWith("//")) path = "/";
    var q = path.IndexOf('?', StringComparison.Ordinal);
    if (q >= 0) path = path[..q];
    var key = SiteLocale.StripEnglishPrefix(path);
    var target = code.StartsWith("en", StringComparison.OrdinalIgnoreCase)
        ? SiteLocale.WithEnglishPrefix(key)
        : key;
    return Results.Redirect(target, permanent: false);
});

app.MapRazorPages();
app.MapControllers();
app.Run();

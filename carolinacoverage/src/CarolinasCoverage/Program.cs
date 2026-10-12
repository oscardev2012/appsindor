using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

builder.Services.Configure<CarolinasCoverage.Site.SiteSettings>(
    builder.Configuration.GetSection(CarolinasCoverage.Site.SiteSettings.SectionName));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CarolinasCoverage.Site.SiteCatalog>();
builder.Services.AddSingleton<CarolinasCoverage.Site.SeoService>();
builder.Services.AddSingleton<CarolinasCoverage.Site.HubLatestPostClient>();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Index", "/es");
    options.Conventions.AddPageRoute("/Coverage/Guide", "/es/coverage/{product}");
    options.Conventions.AddPageRoute("/Service/Demo", "/es/service/{type}");
    options.Conventions.AddPageRoute("/PortalDemo", "/es/portal-demo");
    options.Conventions.AddPageRoute("/Error", "/es/Error");
    options.Conventions.AddPageRoute("/Faqs", CarolinasCoverage.Site.FaqHubCatalog.PathEs);
    foreach (var hub in CarolinasCoverage.Site.SeoHubCatalog.All)
    {
        options.Conventions.AddPageRoute("/Hub", hub.SlugEn);
        options.Conventions.AddPageRoute("/Hub", hub.SlugEs);
    }
});

var app = builder.Build();
app.UseForwardedHeaders();
app.UseResponseCompression();
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    if (CarolinasCoverage.Site.HostCanonical.TryRedirect(context.Request, out var canonical))
    {
        context.Response.Redirect(canonical, permanent: true);
        return;
    }

    if (CarolinasCoverage.Site.LegacyRedirects.TryMatch(context.Request.Path, out var destination))
    {
        context.Response.Redirect(destination + context.Request.QueryString.Value, permanent: true);
        return;
    }

    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var name = ctx.File.Name;
        if (name.EndsWith(".css", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Context.Response.Headers.CacheControl = "public,max-age=2592000";
        }
    }
});
app.UseRouting();

app.MapGet("/sitemap.xml", (CarolinasCoverage.Site.SeoService seo) =>
    Results.Text(seo.SitemapXml(), "application/xml; charset=utf-8"));
app.MapGet("/robots.txt", (CarolinasCoverage.Site.SeoService seo) =>
    Results.Text(seo.RobotsTxt(), "text/plain; charset=utf-8"));
app.MapGet("/llms.txt", (CarolinasCoverage.Site.SeoService seo) =>
    Results.Text(seo.LlmsTxt(), "text/plain; charset=utf-8"));

app.MapRazorPages();
app.Run();

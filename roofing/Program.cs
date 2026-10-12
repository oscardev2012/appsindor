using Microsoft.AspNetCore.ResponseCompression;
using MajesticRoofing.Seo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SeoOptions>(builder.Configuration.GetSection(SeoOptions.SectionName));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<SeoCatalog>();
builder.Services.AddSingleton<FaqHub>();
builder.Services.AddScoped<ISeoService, SeoService>();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<MajesticRoofing.Updates.HubLatestPostClient>();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/json", "image/svg+xml", "application/xml"]);
});

var app = builder.Build();

app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseMiddleware<CanonicalHostMiddleware>();
app.UseMiddleware<HtmlSeoRewriteMiddleware>();
app.UseRouting();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (app.Environment.IsDevelopment()) return;
        var name = ctx.File.Name;
        if (name.EndsWith(".jpeg") || name.EndsWith(".webp") || name.EndsWith(".svg") || name.EndsWith(".css") || name.EndsWith(".js"))
        {
            ctx.Context.Response.Headers.CacheControl = "public,max-age=604800";
        }
    }
});

app.MapMethods("/sitemap.xml", ["GET", "HEAD"], (ISeoService seo) =>
{
    try
    {
        return Results.Text(seo.SitemapXml(), "application/xml; charset=utf-8");
    }
    catch
    {
        return Results.Text(
            """<?xml version="1.0" encoding="UTF-8"?><urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"><url><loc>https://roofingmajesticcharlotte.com/</loc></url></urlset>""",
            "application/xml; charset=utf-8");
    }
});

app.MapMethods("/robots.txt", ["GET", "HEAD"], (ISeoService seo) =>
{
    try
    {
        return Results.Text(seo.RobotsTxt(), "text/plain; charset=utf-8");
    }
    catch
    {
        return Results.Text("User-agent: *\nAllow: /\nSitemap: https://roofingmajesticcharlotte.com/sitemap.xml\n", "text/plain; charset=utf-8");
    }
});

app.MapMethods("/llms.txt", ["GET", "HEAD"], (ISeoService seo) =>
{
    try
    {
        return Results.Text(seo.LlmsTxt(), "text/plain; charset=utf-8");
    }
    catch
    {
        return Results.Text("Majestic Roofing, Inc.\nhttps://roofingmajesticcharlotte.com/\n", "text/plain; charset=utf-8");
    }
});

static void MapPermanentRedirect(WebApplication app, string from, string to)
{
    app.MapMethods(from, ["GET", "HEAD"], () => Results.Redirect(to, permanent: true));
}

MapPermanentRedirect(app, "/roof-leak-repair-charlotte-nc", "/active-roof-leak-repair-charlotte-nc");
MapPermanentRedirect(app, "/hail-damage-roof-repair-charlotte-nc", "/hail-damage-roof-inspection-charlotte-nc");
MapPermanentRedirect(app, "/wind-damage-roof-repair-charlotte-nc", "/wind-missing-shingles-repair-charlotte-nc");
MapPermanentRedirect(app, "/faq", "/faqs");
MapPermanentRedirect(app, "/es/faq", "/es/preguntas-frecuentes");
MapPermanentRedirect(app, "/es/faqs", "/es/preguntas-frecuentes");

app.MapRazorPages();
app.Run();

using CalentonesAgua;
using CalentonesAgua.Data;
using CalentonesAgua.Middleware;
using CalentonesAgua.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<SitePostStore>();
builder.Services.AddSingleton<QuoteLeadStore>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SitePosts", policy =>
        policy.AllowAnyOrigin().WithMethods("GET").WithHeaders("Accept"));
});
builder.Services.AddSingleton<SeoCatalog>();
builder.Services.AddSingleton<AgentMarkdown>();
builder.Services.AddRouting(options =>
{
    options.ConstraintMap["seoen"] = typeof(SeoEnglishSlugConstraint);
    options.ConstraintMap["seoes"] = typeof(SeoSpanishSlugConstraint);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.Use(async (context, next) =>
{
    if (SeoPublicOrigin.ShouldRedirectToCanonicalHost(context.RequestServices.GetRequiredService<IConfiguration>(), context.Request, out var canonical))
    {
        context.Response.Redirect(canonical, permanent: true);
        return;
    }

    await next();
});

app.UseWhen(
    context =>
    {
        var path = context.Request.Path.Value ?? "";
        return !path.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase)
            && !path.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase);
    },
    branch => branch.UseResponseCompression());
app.UseStatusCodePagesWithReExecute("/Home/NotFoundPage");
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.CacheControl = "public,max-age=604800";
    }
});

app.Use(async (context, next) =>
{
    if (HttpMethods.IsGet(context.Request.Method) && context.Request.Path.HasValue)
    {
        var path = context.Request.Path.Value!;
        if (path.Length > 1 && !path.EndsWith('/') && !System.IO.Path.HasExtension(path))
        {
            var catalog = context.RequestServices.GetRequiredService<SeoCatalog>();
            if (catalog.Contains(path) || path is "/about" or "/site-map" or "/reviews" or "/privacy" or "/terms" or "/faqs" or "/water-heater-problems")
            {
                context.Response.Redirect(path + "/" + context.Request.QueryString, permanent: true);
                return;
            }
        }
    }

    await next();
});

app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.UseMiddleware<AgentContentNegotiationMiddleware>();

app.MapControllers();
app.MapControllerRoute("lang", "lang", new { controller = "Language", action = "Set" });
app.MapControllerRoute("problems", "water-heater-problems", new { controller = "Home", action = "Problems" });
app.MapControllerRoute("faqs", "faqs", new { controller = "Home", action = "Faqs" });
app.MapControllerRoute("safe-plan", "financing/safe-plan", new { controller = "Home", action = "SafePlan" });
app.MapControllerRoute("financing", "financing", new { controller = "Home", action = "Financing" });
app.MapControllerRoute("register", "warranty/register", new { controller = "Home", action = "Register" });
app.MapControllerRoute("warranty", "warranty", new { controller = "Home", action = "Warranty" });
app.MapControllerRoute("shelby", "shelby", new { controller = "Home", action = "Shelby" });
app.MapControllerRoute("china-grove", "china-grove", new { controller = "Home", action = "ChinaGrove" });
app.MapControllerRoute("reviews-slash", "reviews/", new { controller = "Home", action = "Reviews" });
app.MapControllerRoute("reviews", "reviews", new { controller = "Home", action = "Reviews" });
app.MapControllerRoute("about-slash", "about/", new { controller = "Home", action = "About" });
app.MapControllerRoute("about", "about", new { controller = "Home", action = "About" });
app.MapControllerRoute("privacy-slash", "privacy/", new { controller = "Home", action = "Privacy" });
app.MapControllerRoute("privacy", "privacy", new { controller = "Home", action = "Privacy" });
app.MapControllerRoute("terms-slash", "terms/", new { controller = "Home", action = "Terms" });
app.MapControllerRoute("terms", "terms", new { controller = "Home", action = "Terms" });
app.MapControllerRoute("html-sitemap-slash", "site-map/", new { controller = "Seo", action = "TopicIndex" });
app.MapControllerRoute("html-sitemap", "site-map", new { controller = "Seo", action = "TopicIndex" });
app.MapControllerRoute("sitemap", "sitemap.xml", new { controller = "Sitemap", action = "Index" });
app.MapControllerRoute("robots", "robots.txt", new { controller = "Sitemap", action = "Robots" });
app.MapControllerRoute("seo-es-slash", "es/{slug:seoes}/", new { controller = "Seo", action = "Es" });
app.MapControllerRoute("seo-es", "es/{slug:seoes}", new { controller = "Seo", action = "Es" });
app.MapControllerRoute("seo-en-slash", "{slug:seoen}/", new { controller = "Seo", action = "Page" });
app.MapControllerRoute("seo-en", "{slug:seoen}", new { controller = "Seo", action = "Page" });
app.MapGet("/Productos/{**rest}", () => Results.Redirect("/water-heater-buying-guide/", true));
app.MapGet("/Productos", () => Results.Redirect("/water-heater-buying-guide/", true));
app.MapGet("/Carrito/{**rest}", () => Results.Redirect("/", true));
app.MapGet("/Carrito", () => Results.Redirect("/", true));
app.MapGet("/Contacto/{**rest}", () => Results.Redirect("/water-heater-replacement-quote/", true));
app.MapGet("/Contacto", () => Results.Redirect("/water-heater-replacement-quote/", true));
app.MapGet("/Home/Nosotros", () => Results.Redirect("/about/", true));
app.MapGet("/Home/Privacidad", () => Results.Redirect("/privacy/", true));
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program
{
}

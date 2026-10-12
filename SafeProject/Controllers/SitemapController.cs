using CalentonesAgua.Data;
using CalentonesAgua.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

public class SitemapController : Controller
{
    private readonly SeoCatalog _catalog;
    private readonly IConfiguration _config;

    public SitemapController(SeoCatalog catalog, IConfiguration config)
    {
        _catalog = catalog;
        _config = config;
    }

    [HttpGet]
    [HttpHead]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public IActionResult Index()
    {
        var xml = SitemapXml.Build(PublicOrigin(), _catalog);
        return Content(xml, "application/xml; charset=utf-8");
    }

    [HttpGet]
    [HttpHead]
    public IActionResult Robots()
    {
        var origin = PublicOrigin();
        var body =
            $"""
            User-agent: *
            Allow: /
            Allow: /llms.txt
            Disallow: /Productos
            Disallow: /Carrito
            Disallow: /Contacto
            Disallow: /Home/Nosotros
            Disallow: /Home/Privacidad
            Allow: /privacy/
            Allow: /terms/
            Disallow: /financing/safe-plan
            Disallow: /warranty/register
            Disallow: /admin

            Sitemap: {origin}/sitemap.xml
            # Agent index: {origin}/llms.txt
            # Agent index (alias): {origin}/agent-instructions
            """;
        return Content(body, "text/plain; charset=utf-8");
    }

    private string PublicOrigin()
    {
        var configured = _config["Seo:PublicOrigin"];
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return configured.TrimEnd('/');
        }

        return $"{Request.Scheme}://{Request.Host}".TrimEnd('/');
    }
}

using Microsoft.AspNetCore.Mvc;
using RealtorNews.Data;
using RealtorNews.Services;
using System.Xml.Linq;

namespace RealtorNews.Controllers;

public class SeoController : Controller
{
    private readonly RealtorNewsStore _store;

    public SeoController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("/robots.txt")]
    public IActionResult Robots()
    {
        var origin = Seo.Absolute(Request, "/").TrimEnd('/');
        var body = $"""
            User-agent: *
            Allow: /
            Disallow: /account
            Disallow: /studio
            Disallow: /news/preview

            User-agent: Googlebot
            Allow: /

            Sitemap: {origin}/sitemap.xml
            """;
        return Content(body, "text/plain; charset=utf-8");
    }

    [HttpGet("/sitemap.xml")]
    public IActionResult Sitemap()
    {
        var urls = new List<(string Path, DateTimeOffset Modified, string Frequency, string Priority)>
        {
            ("/", DateTimeOffset.UtcNow, "daily", "1.0"),
            ("/feed", DateTimeOffset.UtcNow, "hourly", "0.9"),
            ("/news", DateTimeOffset.UtcNow, "hourly", "0.9"),
            ("/seasonal", DateTimeOffset.UtcNow, "weekly", "0.8"),
            ("/cities", DateTimeOffset.UtcNow, "weekly", "0.8"),
            ("/guides", DateTimeOffset.UtcNow, "weekly", "0.8"),
            ("/guides?category=Buying", DateTimeOffset.UtcNow, "weekly", "0.7"),
            ("/guides?category=Selling", DateTimeOffset.UtcNow, "weekly", "0.7"),
            ("/guides?category=Investing", DateTimeOffset.UtcNow, "weekly", "0.6"),
            ("/guides?category=Homeownership", DateTimeOffset.UtcNow, "weekly", "0.6"),
            ("/realtors", DateTimeOffset.UtcNow, "daily", "0.8"),
            ("/for-realtors", DateTimeOffset.UtcNow, "monthly", "0.6"),
            ("/vendors", DateTimeOffset.UtcNow, "weekly", "0.8"),
            ("/ask", DateTimeOffset.UtcNow, "monthly", "0.6")
        };

        try
        {
            if (_store.Probe() is null)
            {
                foreach (var post in _store.Published(take: 200))
                {
                    urls.Add(($"/news/{post.Slug}", post.UpdatedUtc == default ? post.CreatedUtc : post.UpdatedUtc, "weekly", "0.8"));
                }

                foreach (var realtor in _store.Directory())
                {
                    urls.Add(($"/realtors/{realtor.Slug}", realtor.CreatedUtc, "weekly", "0.6"));
                }
            }
        }
        catch
        {
            // Public sitemap should still list the core pages.
        }

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
            new XElement(ns + "urlset",
                urls.Select(item =>
                    new XElement(ns + "url",
                        new XElement(ns + "loc", Seo.Absolute(Request, item.Path)),
                        new XElement(ns + "lastmod", item.Modified.UtcDateTime.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", item.Frequency),
                        new XElement(ns + "priority", item.Priority)))));

        return Content(document.Declaration + document.ToString(SaveOptions.DisableFormatting), "application/xml; charset=utf-8");
    }

    [HttpGet("/news/rss.xml")]
    public IActionResult Rss()
    {
        var origin = Seo.Absolute(Request, "/").TrimEnd('/');
        var items = new XElement("channel",
            new XElement("title", Seo.Brand),
            new XElement("link", origin),
            new XElement("description", Seo.DefaultDescription),
            new XElement("language", "en-us"),
            new XElement("lastBuildDate", DateTime.UtcNow.ToString("r")));

        try
        {
            if (_store.Probe() is null)
            {
                foreach (var post in _store.Published(take: 30))
                {
                    items.Add(new XElement("item",
                        new XElement("title", post.Title),
                        new XElement("link", $"{origin}/news/{post.Slug}"),
                        new XElement("guid", $"{origin}/news/{post.Slug}"),
                        new XElement("pubDate", (post.PublishedUtc ?? post.CreatedUtc).UtcDateTime.ToString("r")),
                        new XElement("description", post.Teaser),
                        new XElement("category", post.Category)));
                }
            }
        }
        catch
        {
        }

        var rss = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("rss", new XAttribute("version", "2.0"), items));
        return Content(rss.Declaration + rss.ToString(SaveOptions.DisableFormatting), "application/rss+xml; charset=utf-8");
    }
}

using System.Globalization;
using System.Text;
using System.Xml;
using CalentonesAgua.Data;
using CalentonesAgua.Models;

namespace CalentonesAgua.Services;

public static class SitemapXml
{
    public static string Build(string origin, SeoCatalog catalog)
    {
        origin = origin.TrimEnd('/');
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            OmitXmlDeclaration = false,
            Indent = true,
            Async = false
        };

        using var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            WriteUrl(writer, seen, origin + "/", today, "1.0", "weekly");
            WriteUrl(writer, seen, origin + "/about/", today, "0.7", "monthly");
            WriteUrl(writer, seen, origin + "/privacy/", today, "0.5", "monthly");
            WriteUrl(writer, seen, origin + "/terms/", today, "0.5", "monthly");
            WriteUrl(writer, seen, origin + "/reviews/", today, "0.6", "monthly");
            WriteUrl(writer, seen, origin + "/site-map/", today, "0.6", "weekly");
            WriteUrl(writer, seen, origin + "/water-heater-problems/", today, "0.7", "monthly");
            WriteUrl(writer, seen, origin + "/faqs/", today, "0.8", "weekly");
            WriteUrl(writer, seen, origin + "/water-heater-financing/", today, "0.7", "monthly");
            WriteUrl(writer, seen, origin + "/water-heater-insurance-warranty/", today, "0.6", "monthly");
            WriteUrl(writer, seen, origin + "/shelby", today, "0.8", "monthly");
            WriteUrl(writer, seen, origin + "/china-grove", today, "0.8", "monthly");

            foreach (var page in catalog.Pages)
            {
                var priority = page.Priority switch
                {
                    "P0" => "0.9",
                    "P1" => "0.7",
                    _ => "0.5"
                };
                WriteUrl(writer, seen, origin + page.Slug, today, priority, "weekly");
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void WriteUrl(
        XmlWriter writer,
        HashSet<string> seen,
        string loc,
        string lastmod,
        string priority,
        string changefreq)
    {
        if (!seen.Add(loc)) return;

        writer.WriteStartElement("url");
        writer.WriteElementString("loc", loc);
        writer.WriteElementString("lastmod", lastmod);
        writer.WriteElementString("changefreq", changefreq);
        writer.WriteElementString("priority", priority);
        writer.WriteEndElement();
    }
}

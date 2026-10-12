using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace RealtorNews.Data;

public sealed record SeoCrumb(string Name, string Path);

public static class Seo
{
    public const string Brand = "Charlotte Realtor Recommendations";
    public const string DefaultDescription =
        "Local Charlotte real estate education for buyers, sellers, homeowners, and people relocating. Guides, Realtor news, cities, and preferred vendors across NC and SC.";
    public const string DefaultImagePath = "/img/hero-charlotte.jpg";
    public const string Language = "en-US";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string PageTitle(string? title)
    {
        var value = string.IsNullOrWhiteSpace(title) ? Brand : title.Trim();
        if (value.Contains(Brand, StringComparison.OrdinalIgnoreCase))
        {
            return value;
        }

        return $"{value} | {Brand}";
    }

    public static string Description(string? value, int max = 158)
    {
        var text = string.IsNullOrWhiteSpace(value) ? DefaultDescription : value.Trim();
        text = NewsFormatter.ToPlain(text);
        if (text.Length <= max)
        {
            return text;
        }

        return text[..(max - 1)].TrimEnd() + "…";
    }

    public static string Absolute(HttpRequest request, string? pathAndQuery = null)
    {
        var configured = request.HttpContext.RequestServices.GetService<IConfiguration>()?["Site:PublicBaseUrl"]?.Trim().TrimEnd('/');
        var origin = string.IsNullOrWhiteSpace(configured)
            ? $"{request.Scheme}://{request.Host}"
            : configured.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(pathAndQuery))
        {
            pathAndQuery = request.Path.HasValue ? request.Path.Value! : "/";
        }

        if (!pathAndQuery.StartsWith('/'))
        {
            pathAndQuery = "/" + pathAndQuery;
        }

        return origin + pathAndQuery;
    }

    public static string Image(HttpRequest request, string? path)
    {
        var value = string.IsNullOrWhiteSpace(path) ? DefaultImagePath : path.Trim();
        if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return value;
        }

        if (!value.StartsWith('/'))
        {
            value = "/" + value;
        }

        return Absolute(request, value);
    }

    public static bool IsPrivate(PathString path)
    {
        var value = path.Value ?? "/";
        return value.StartsWith("/account", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("/studio", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("/news/preview", StringComparison.OrdinalIgnoreCase)
            || value.Contains("/Error", StringComparison.OrdinalIgnoreCase);
    }

    public static string Robots(ViewDataDictionary viewData, PathString path)
    {
        if (viewData["Robots"] is string custom && !string.IsNullOrWhiteSpace(custom))
        {
            return custom;
        }

        return IsPrivate(path)
            ? "noindex, nofollow"
            : "index, follow, max-image-preview:large, max-snippet:-1, max-video-preview:-1";
    }

    public static IHtmlContent JsonLd(params Dictionary<string, object?>[] nodes)
    {
        object payload = nodes.Length == 1
            ? nodes[0]
            : new Dictionary<string, object?>
            {
                ["@context"] = "https://schema.org",
                ["@graph"] = nodes.Select(node =>
                {
                    var copy = new Dictionary<string, object?>(node);
                    copy.Remove("@context");
                    return copy;
                }).ToArray()
            };
        return new HtmlString(JsonSerializer.Serialize(payload, JsonOptions));
    }

    public static Dictionary<string, object?> Type(string type, Dictionary<string, object?>? fields = null)
    {
        var node = new Dictionary<string, object?> { ["@type"] = type };
        if (fields is null)
        {
            return node;
        }

        foreach (var pair in fields)
        {
            node[pair.Key] = pair.Value;
        }

        return node;
    }

    public static Dictionary<string, object?> Node(string type, Dictionary<string, object?> fields)
    {
        var node = Type(type, fields);
        node["@context"] = "https://schema.org";
        return node;
    }

    public static Dictionary<string, object?> Organization(HttpRequest request)
        => Node("Organization", new()
        {
            ["name"] = Brand,
            ["url"] = Absolute(request, "/"),
            ["logo"] = Image(request, DefaultImagePath),
            ["description"] = DefaultDescription,
            ["areaServed"] = new object[]
            {
                Type("City", new() { ["name"] = "Charlotte" }),
                Type("State", new() { ["name"] = "North Carolina" }),
                Type("State", new() { ["name"] = "South Carolina" })
            },
            ["knowsAbout"] = new[]
            {
                "Charlotte real estate", "home buying", "home selling", "relocation to Charlotte", "homeownership", "preferred home-service vendors"
            }
        });

    public static Dictionary<string, object?> WebSite(HttpRequest request)
        => Node("WebSite", new()
        {
            ["name"] = Brand,
            ["url"] = Absolute(request, "/"),
            ["inLanguage"] = Language,
            ["description"] = DefaultDescription,
            ["publisher"] = Type("Organization", new() { ["name"] = Brand })
        });

    public static Dictionary<string, object?> Breadcrumb(HttpRequest request, IReadOnlyList<(string Name, string Path)> crumbs)
        => Node("BreadcrumbList", new()
        {
            ["itemListElement"] = crumbs.Select((crumb, index) => new Dictionary<string, object?>
            {
                ["@type"] = "ListItem",
                ["position"] = index + 1,
                ["name"] = crumb.Name,
                ["item"] = Absolute(request, crumb.Path)
            }).ToArray()
        });

    public static Dictionary<string, object?> Faq(params (string Question, string Answer)[] items)
        => Node("FAQPage", new()
        {
            ["mainEntity"] = items.Select(item => Type("Question", new()
            {
                ["name"] = item.Question,
                ["acceptedAnswer"] = Type("Answer", new() { ["text"] = item.Answer })
            })).ToArray()
        });
}

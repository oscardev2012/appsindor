using System.Text.Json;
using CalentonesAgua.Models;

namespace CalentonesAgua.Data;

public sealed class SeoCatalog
{
    private readonly Dictionary<string, SeoPage> _bySlug;
    private readonly IReadOnlyList<SeoPage> _pages;

    public SeoCatalog(IWebHostEnvironment env)
    {
        var drafts = SeoCopy.All();
        var queries = LoadQueries(env);
        var titleBySlug = drafts.ToDictionary(d => Normalize(d.Slug), d => d.H1, StringComparer.OrdinalIgnoreCase);

        var pages = new List<SeoPage>(drafts.Count);
        foreach (var draft in drafts)
        {
            var slug = Normalize(draft.Slug);
            var related = draft.RelatedSlugs
                .Select(RelatedHref)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(href => new SeoRelatedLink
                {
                    Slug = href,
                    Label = titleBySlug.TryGetValue(Normalize(href), out var h1) ? h1 : LabelForExisting(href)
                })
                .ToList();

            var faqs = SeoFaqBank.Build(draft, queries.GetValueOrDefault(slug) ?? []);

            pages.Add(new SeoPage
            {
                Priority = draft.Priority,
                Language = draft.Language,
                Slug = slug,
                H1 = draft.H1,
                Title = draft.Title,
                Description = draft.Description,
                Cluster = draft.Cluster,
                Intent = draft.Intent,
                Eyebrow = draft.Eyebrow,
                AnswerFirst = draft.AnswerFirst,
                CtaLabel = draft.CtaLabel,
                CtaHref = draft.CtaHref,
                SecondaryCtaLabel = draft.SecondaryCtaLabel,
                SecondaryCtaHref = draft.SecondaryCtaHref,
                ShowSafetyBar = draft.ShowSafetyBar,
                ShowPricing = draft.ShowPricing,
                IsQuotePage = draft.IsQuotePage,
                AlternateLangSlug = draft.AlternateLangSlug is null ? null : Normalize(draft.AlternateLangSlug),
                Sections = draft.Sections,
                Faqs = faqs,
                Related = related
            });
        }

        _pages = pages;
        _bySlug = pages.ToDictionary(p => p.Slug, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyList<SeoPage> Pages => _pages;

    public bool TryGet(string? slug, out SeoPage page)
    {
        page = null!;
        if (string.IsNullOrWhiteSpace(slug)) return false;
        return _bySlug.TryGetValue(Normalize(slug), out page!);
    }

    public bool Contains(string? slug) => TryGet(slug, out _);

    public static string Normalize(string slug)
    {
        slug = slug.Trim();
        if (!slug.StartsWith('/')) slug = "/" + slug;
        if (!slug.EndsWith('/')) slug += "/";
        return slug.ToLowerInvariant();
    }

    private static string RelatedHref(string slug)
    {
        slug = slug.Trim();
        if (!slug.StartsWith('/')) slug = "/" + slug;
        if (slug is "/shelby" or "/china-grove" or "/financing" or "/warranty") return slug;
        if (slug.StartsWith("/financing/", StringComparison.OrdinalIgnoreCase) ||
            slug.StartsWith("/warranty/", StringComparison.OrdinalIgnoreCase))
        {
            return slug.TrimEnd('/');
        }

        return Normalize(slug);
    }

    private static string LabelForExisting(string slug) => slug.TrimEnd('/') switch
    {
        "/shelby" => "Shelby, NC service",
        "/china-grove" => "China Grove, NC service",
        "/financing" => "Synchrony + SAFE Homeowner Plan",
        "/financing/safe-plan" => "SAFE Homeowner Plan",
        "/warranty" => "Warranty coverage",
        "/warranty/register" => "Register a unit",
        _ => slug
    };

    private static Dictionary<string, List<SeoQuery>> LoadQueries(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "seo-architecture.json");
        if (!File.Exists(path))
        {
            return new Dictionary<string, List<SeoQuery>>(StringComparer.OrdinalIgnoreCase);
        }

        using var stream = File.OpenRead(path);
        var rows = JsonSerializer.Deserialize<List<ArchitectureRow>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        var map = new Dictionary<string, List<SeoQuery>>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var slug = Normalize(row.Slug ?? "");
            if (!map.TryGetValue(slug, out var list))
            {
                list = [];
                map[slug] = list;
            }

            foreach (var q in row.Queries ?? [])
            {
                list.Add(new SeoQuery(q.En ?? "", q.Es ?? "", q.Scenario ?? "", q.Cluster ?? "", q.Note ?? ""));
            }
        }

        return map;
    }

    private sealed class ArchitectureRow
    {
        public string? Slug { get; set; }
        public List<ArchitectureQuery>? Queries { get; set; }
    }

    private sealed class ArchitectureQuery
    {
        public string? En { get; set; }
        public string? Es { get; set; }
        public string? Scenario { get; set; }
        public string? Cluster { get; set; }
        public string? Note { get; set; }
    }
}

internal readonly record struct SeoQuery(string En, string Es, string Scenario, string Cluster, string Note);

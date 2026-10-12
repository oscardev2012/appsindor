using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NcOpportunityFinder;

/// <summary>
/// Vocabulary from the "NC Third-Party Property Keyword Guide" and "Property Keyword Lists and Scenarios".
/// A post is a strong opportunity only when it combines a direct sale, a house that needs work and a
/// motivated seller; a motivated-seller phrase alone never raises a post.
/// </summary>
public static class PropertyKeywords
{
    public static readonly string[] DirectSale =
    [
        "fsbo", "for sale by owner", "house for sale by owner", "selling my house", "selling our home", "private sale",
        "selling without a realtor", "no realtors please", "looking for a cash buyer", "looking for someone to buy my house",
        "anyone interested in buying a house", "owner financing", "owner finance", "seller financing",
        "i need to sell my house", "my house needs work", "selling my parents' house", "selling my mom's house",
        "selling inherited property", "house for sale, needs work", "selling property as-is", "selling as-is", "by owner",
        "casa en venta por dueño", "por dueño", "por el dueño", "venta por propietario", "vendo mi casa", "venta privada", "sin realtor",
        "busco comprador de efectivo", "alguien interesado en comprar una casa", "financiamiento por dueño"
    ];

    public static readonly string[] Condition =
    [
        "fixer upper", "needs work", "needs repairs", "needs repair", "needs tlc", "handyman special", "investor special",
        "rehab needed", "renovation needed", "needs renovation", "cosmetic updates needed", "updating needed", "sold as-is",
        "selling as-is", "as is condition", "bring your tools", "needs love", "deferred maintenance", "rough condition",
        "project house", "flip opportunity", "fix and flip", "cash only", "cash buyers only",
        "casa necesita arreglos", "casa necesita reparación", "casa para reparar", "casa para remodelar",
        "casa para inversionista", "oportunidad para inversionista", "vendo casa como está", "solo efectivo",
        "compradores de efectivo", "necesita arreglos", "necesita reparación", "necesita reparaciones", "para remodelar",
        "como está"
    ];

    public static readonly string[] Motivated =
    [
        "need to sell", "need to sell fast", "must sell", "motivated seller", "serious buyers only", "open to offers",
        "make an offer", "bring all offers", "best offer", "obo", "reduced price", "price reduced", "moving soon", "relocating",
        "divorce sale", "job relocation", "can't keep up with repairs", "can't afford repairs", "selling below market",
        "priced to sell", "need it gone", "quick sale", "fast closing", "cash offer", "will consider offers",
        "necesito vender", "urge vender", "se vende rápido", "acepto ofertas", "dueño motivado", "precio rebajado",
        "se vende por debajo del mercado", "venta rápida", "cierre rápido", "acepto efectivo"
    ];

    public static readonly string[] Situation =
    [
        "inherited house", "estate sale", "probate", "vacant house", "empty house", "tenant occupied", "problem tenant",
        "rental property for sale", "tired landlord", "fire damage", "water damage", "flood damage", "roof leak",
        "foundation issue", "structural issue", "mold issue", "crawlspace issue", "code violations", "liens",
        "behind on taxes", "foreclosure", "pre-foreclosure",
        "casa heredada", "propiedad heredada", "casa vacía", "casa con daños de agua", "casa con daño de techo",
        "casa con problemas de fundación", "casa con problemas estructurales", "casa con humedad",
        "problemas de crawlspace", "impuestos atrasados", "embargo", "pre-embargo"
    ];

    /// <summary>Realtor, MLS and buyer posts are not third-party direct sales.</summary>
    public static readonly string[] Exclude =
    [
        "mls", "listed by realtor", "realtor listing", "new construction model home", "apartment for rent",
        "house for rent", "looking to buy", "wanted to buy", "real estate agent services"
    ];

    /// <summary>Phrases sent to the search boxes; ordered so the first ones get most of a small result limit.</summary>
    public static readonly string[] SearchPhrasesEn =
    [
        "house for sale by owner", "fixer upper house", "house needs work", "sold as is house", "handyman special",
        "investor special", "need to sell my house", "inherited house for sale", "owner financing house"
    ];

    public static readonly string[] SearchPhrasesEs = ["casa en venta por dueño", "vendo mi casa", "casa para reparar"];

    /// <summary>Facebook post and group search: one run per phrase, so only the phrases owners use most.</summary>
    public static readonly string[] FacebookSearchPhrases =
        ["house for sale by owner", "fsbo", "selling my house", "house needs work", "fixer upper for sale", "vendo mi casa"];

    private static readonly (string Term, Regex Pattern)[] DirectPatterns = Compile(DirectSale);
    private static readonly (string Term, Regex Pattern)[] ConditionPatterns = Compile(Condition);
    private static readonly (string Term, Regex Pattern)[] MotivatedPatterns = Compile(Motivated);
    private static readonly (string Term, Regex Pattern)[] SituationPatterns = Compile(Situation);
    private static readonly (string Term, Regex Pattern)[] ExcludePatterns = Compile(Exclude);

    public static KeywordMatch Classify(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return KeywordMatch.None;
        var normalized = Normalize(text);
        return new KeywordMatch(Find(DirectPatterns, normalized), Find(ConditionPatterns, normalized),
            Find(MotivatedPatterns, normalized), Find(SituationPatterns, normalized));
    }

    public static bool IsExcluded(string? text) =>
        !string.IsNullOrWhiteSpace(text) && Find(ExcludePatterns, Normalize(text)).Count > 0;

    /// <summary>Lower case, no accents, curly quotes straightened and hyphens read as spaces ("as-is" = "as is").</summary>
    public static string Normalize(string text)
    {
        var decomposed = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(decomposed.Length);
        foreach (var ch in decomposed)
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        var plain = sb.ToString().ToLowerInvariant().Replace('’', '\'').Replace('‘', '\'');
        return Regex.Replace(Regex.Replace(plain, @"[-_/,]", " "), @"\s+", " ");
    }

    private static (string, Regex)[] Compile(string[] terms) => terms
        .Select(t => (t, new Regex($@"(?<![a-z0-9]){Regex.Escape(Normalize(t))}(?![a-z0-9])", RegexOptions.Compiled)))
        .ToArray();

    private static List<string> Find((string Term, Regex Pattern)[] patterns, string normalized)
    {
        var found = patterns.Where(p => p.Pattern.IsMatch(normalized)).Select(p => p.Term).ToList();
        // "need to sell fast" also contains "need to sell"; keep only the longest phrase.
        return found.Where(t => !found.Any(o => o != t && Normalize(o).Contains(Normalize(t)))).ToList();
    }
}

public sealed record KeywordMatch(List<string> Direct, List<string> Condition, List<string> Motivated, List<string> Situation)
{
    public static readonly KeywordMatch None = new([], [], [], []);

    /// <summary>high = direct + condition + motivated; investment = direct + condition; direct = direct sale only.</summary>
    public string? Priority =>
        Direct.Count == 0 ? null
        : Condition.Count == 0 ? "direct"
        : Motivated.Count == 0 ? "investment"
        : "high";

    /// <summary>Evidence saved with the property, most important category first.</summary>
    public string? Signal(params string[] extra)
    {
        var all = Direct.Concat(Condition).Concat(Motivated).Concat(Situation).Concat(extra)
            .Distinct(StringComparer.OrdinalIgnoreCase).Take(10).ToList();
        return all.Count == 0 ? null : string.Join(" · ", all);
    }
}

namespace CalentonesAgua.Services;

/// <summary>
/// RFC 9110 Accept matching for acceptmarkdown.com: HTML vs Markdown, q-values, and 406.
/// </summary>
public static class AcceptNegotiation
{
    public static readonly string[] DocumentTypes = ["text/html", "text/markdown"];

    public static string? PreferredType(string? acceptHeader, IReadOnlyList<string> produces)
    {
        if (string.IsNullOrWhiteSpace(acceptHeader))
        {
            return produces.Count > 0 ? produces[0] : null;
        }

        var entries = Parse(acceptHeader);
        if (entries.Count == 0)
        {
            return produces.Count > 0 ? produces[0] : null;
        }

        string? best = null;
        var bestQ = -1d;
        var bestPos = int.MaxValue;

        foreach (var candidate in produces)
        {
            AcceptEntry? matched = null;
            var matchedPos = int.MaxValue;
            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                if (!Matches(entry, candidate)) continue;
                if (matched is null ||
                    entry.Specificity > matched.Value.Specificity ||
                    (entry.Specificity == matched.Value.Specificity && i < matchedPos))
                {
                    matched = entry;
                    matchedPos = i;
                }
            }

            if (matched is null || matched.Value.Q <= 0) continue;
            if (matched.Value.Q > bestQ || (matched.Value.Q.Equals(bestQ) && matchedPos < bestPos))
            {
                best = candidate;
                bestQ = matched.Value.Q;
                bestPos = matchedPos;
            }
        }

        return best;
    }

    public static IReadOnlyList<AcceptEntry> Parse(string header)
    {
        var list = new List<AcceptEntry>();
        foreach (var raw in header.Split(','))
        {
            var parts = raw.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;
            var type = parts[0].Trim().ToLowerInvariant();
            if (type.Length == 0) continue;

            var q = 1d;
            for (var i = 1; i < parts.Length; i++)
            {
                var param = parts[i];
                var eq = param.IndexOf('=');
                if (eq <= 0) continue;
                var name = param[..eq].Trim();
                if (!name.Equals("q", StringComparison.OrdinalIgnoreCase)) continue;
                if (double.TryParse(param[(eq + 1)..].Trim(), System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out var parsed))
                {
                    q = Math.Clamp(parsed, 0, 1);
                }
            }

            var specificity = type == "*/*" ? 0 : type.EndsWith("/*", StringComparison.Ordinal) ? 1 : 2;
            list.Add(new AcceptEntry(type, q, specificity));
        }

        return list;
    }

    public static bool Matches(AcceptEntry entry, string candidate)
    {
        candidate = candidate.ToLowerInvariant();
        if (entry.Type == "*/*") return true;
        if (entry.Type.EndsWith("/*", StringComparison.Ordinal))
        {
            return candidate.StartsWith(entry.Type[..^1], StringComparison.Ordinal);
        }

        return entry.Type == candidate;
    }

    public readonly record struct AcceptEntry(string Type, double Q, int Specificity);
}

using System.Globalization;
using System.Text.RegularExpressions;

namespace SafeHeaterSmsAgent;

internal static class LeadWhen
{
    public static string FormatEs(string? stored)
    {
        if (!TryCanonical(stored, out var date, out var part, out var clock))
            return string.IsNullOrWhiteSpace(stored) ? "fecha por confirmar" : stored.Trim();

        var es = CultureInfo.GetCultureInfo("es-ES");
        var dayName = date.ToString("dddd", es);
        dayName = char.ToUpper(dayName[0], es) + dayName[1..];
        var month = date.ToString("MMMM", es);
        var core = $"{dayName} {date.Day} de {month} de {date.Year}";
        var band = part == "morning" ? ", en la mañana" : part == "afternoon" ? ", en la tarde" : "";
        var clk = string.IsNullOrWhiteSpace(clock) ? "" : $" {clock.Replace("after ", "después de las ").Replace("pm", " p.m.")}";
        return core + band + clk;
    }

    public static string FormatEn(string? stored)
    {
        if (!TryCanonical(stored, out var date, out var part, out var clock))
            return string.IsNullOrWhiteSpace(stored) ? "date to confirm" : stored.Trim();

        var en = CultureInfo.GetCultureInfo("en-US");
        var core = date.ToString("dddd, MMMM d, yyyy", en);
        var band = part == "morning" ? " in the morning" : part == "afternoon" ? " in the afternoon" : "";
        var clk = string.IsNullOrWhiteSpace(clock) ? "" : $" {clock}";
        return core + band + clk;
    }

    public static string Format(string? stored, bool spanish) =>
        spanish ? FormatEs(stored) : FormatEn(stored);

    private static bool TryCanonical(string? stored, out DateTime date, out string part, out string clock)
    {
        date = default;
        part = "";
        clock = "";
        var m = Regex.Match(stored ?? "", @"\b(20\d{2}-\d{2}-\d{2})(?:\s+(morning|afternoon))?(?:\s+after\s+(\d{1,2})pm)?", RegexOptions.IgnoreCase);
        if (!m.Success || !DateTime.TryParseExact(m.Groups[1].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            return false;
        part = m.Groups[2].Success ? m.Groups[2].Value.ToLowerInvariant() : "";
        if (m.Groups[3].Success) clock = $"after {m.Groups[3].Value}pm";
        return true;
    }
}

using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;

namespace RealtorNews.Data;

public static class NewsFormatter
{
    public static string ToPlain(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return "";
        }

        var value = text.Replace("\r\n", "\n");
        value = Regex.Replace(value, @"^#{1,3}\s+", "", RegexOptions.Multiline);
        value = Regex.Replace(value, @"^>\s?", "", RegexOptions.Multiline);
        value = Regex.Replace(value, @"^[-*]\s+", "", RegexOptions.Multiline);
        value = value.Replace("**", "");
        value = Regex.Replace(value, @"\s+", " ").Trim();
        return value;
    }

    public static string Teaser(string? summary, string? body, int max = 220)
    {
        var text = string.IsNullOrWhiteSpace(summary) ? ToPlain(body) : summary.Trim();
        if (text.Length <= max)
        {
            return text;
        }

        return text[..(max - 1)].TrimEnd() + "…";
    }

    public static string? FirstCallout(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        var blocks = body.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (var block in blocks)
        {
            var lines = block.Split('\n');
            if (lines.All(line => line.TrimStart().StartsWith('>')))
            {
                var text = string.Join(" ", lines.Select(line => line.TrimStart().TrimStart('>').Trim()));
                return string.IsNullOrWhiteSpace(text) ? null : text;
            }
        }

        return null;
    }

    public static string SampleBody =>
        """
        Charlotte está cambiando más rápido de lo que muchos compradores esperan. Antes de ajustar el presupuesto, conviene mirar tasas, inventario y el vecindario juntos.

        ## Qué significa esto ahora
        Una tasa más alta no cierra la puerta. Cambia cuánto puedes pagar cada mes y qué tipo de casa encaja **sin presión**.

        > Mi recomendación: pide a tu prestamista un escenario a 30 días y otro a 90 días antes de firmar.

        - Revisa tu pago máximo, no solo el precio
        - Compara dos vecindarios, no diez
        - Confirma impuestos y HOA antes de la oferta
        """;

    public static IHtmlContent ToHtml(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return HtmlString.Empty;
        }

        var html = new StringBuilder();
        var blocks = body.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (var raw in blocks)
        {
            var block = raw.Trim();
            if (block.Length == 0)
            {
                continue;
            }

            var lines = block.Split('\n');
            if (lines[0].StartsWith("## "))
            {
                html.Append("<h2>").Append(Inline(lines[0][3..])).Append("</h2>");
                var rest = string.Join("\n", lines.Skip(1)).Trim();
                if (rest.Length > 0)
                {
                    html.Append("<p>").Append(Inline(rest.Replace("\n", " "))).Append("</p>");
                }

                continue;
            }

            if (lines.All(line => line.TrimStart().StartsWith('>')))
            {
                var text = string.Join(" ", lines.Select(line => line.TrimStart().TrimStart('>').Trim()));
                html.Append("<blockquote class=\"callout\"><span>Recomendación</span><p>")
                    .Append(Inline(text))
                    .Append("</p></blockquote>");
                continue;
            }

            if (lines.Count(line => line.TrimStart().StartsWith("- ") || line.TrimStart().StartsWith("* ")) >= 2
                || (lines.Length == 1 && (lines[0].TrimStart().StartsWith("- ") || lines[0].TrimStart().StartsWith("* "))))
            {
                html.Append("<ul>");
                foreach (var line in lines)
                {
                    var item = Regex.Replace(line.TrimStart(), @"^[-*]\s+", "");
                    html.Append("<li>").Append(Inline(item)).Append("</li>");
                }

                html.Append("</ul>");
                continue;
            }

            html.Append("<p>").Append(Inline(block.Replace("\n", " "))).Append("</p>");
        }

        return new HtmlString(html.ToString());
    }

    private static string Inline(string text)
    {
        var encoded = WebUtility.HtmlEncode(text);
        encoded = Regex.Replace(encoded, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
        encoded = Regex.Replace(encoded, @"\*(.+?)\*", "<em>$1</em>");
        return encoded;
    }
}

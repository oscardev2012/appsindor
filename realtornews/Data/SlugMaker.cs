using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace RealtorNews.Data;

public static class SlugMaker
{
    public static string From(string value, int maxLength = 140)
    {
        var text = (value ?? "").Trim().ToLowerInvariant();
        text = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(text.Length);
        foreach (var ch in text)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (category == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsLetterOrDigit(ch))
            {
                builder.Append(ch);
            }
            else if (ch is ' ' or '-' or '_' or '.')
            {
                builder.Append('-');
            }
        }

        var slug = Regex.Replace(builder.ToString(), "-{2,}", "-").Trim('-');
        if (slug.Length == 0)
        {
            slug = "realtor";
        }

        return slug.Length <= maxLength ? slug : slug[..maxLength].Trim('-');
    }
}

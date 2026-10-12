using System.Text;

namespace MajesticRoofing.Seo;

public sealed class HtmlSeoRewriteMiddleware
{
    private const string LegacyHost = "https://majestic-roofing.jrricardo29.chatgpt.site";
    private const string LegacyHostHttp = "http://majestic-roofing.jrricardo29.chatgpt.site";
    private const string LegacyHostBare = "majestic-roofing.jrricardo29.chatgpt.site";
    private readonly RequestDelegate _next;

    public HtmlSeoRewriteMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, ISeoService seo)
    {
        var path = context.Request.Path.Value ?? "";
        if (path.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase) ||
            path.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase) ||
            path.Equals("/llms.txt", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var ext = Path.GetExtension(path);
        if (!string.IsNullOrEmpty(ext) &&
            !ext.Equals(".html", StringComparison.OrdinalIgnoreCase) &&
            !ext.Equals(".htm", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var original = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;
        try
        {
            await _next(context);
            buffer.Position = 0;
            var contentType = context.Response.ContentType ?? "";
            if (contentType.Contains("text/html", StringComparison.OrdinalIgnoreCase) && buffer.Length > 0)
            {
                using var reader = new StreamReader(buffer, Encoding.UTF8, leaveOpen: true);
                var html = await reader.ReadToEndAsync();
                html = html.Replace(LegacyHost, seo.Origin, StringComparison.Ordinal);
                html = html.Replace(LegacyHostHttp, seo.Origin, StringComparison.Ordinal);
                html = html.Replace(LegacyHostBare, seo.Origin.Replace("https://", "").Replace("http://", ""), StringComparison.Ordinal);
                var bytes = Encoding.UTF8.GetBytes(html);
                context.Response.ContentLength = bytes.Length;
                context.Response.Body = original;
                await context.Response.Body.WriteAsync(bytes);
            }
            else
            {
                context.Response.Body = original;
                buffer.Position = 0;
                await buffer.CopyToAsync(original);
            }
        }
        finally
        {
            context.Response.Body = original;
        }
    }
}

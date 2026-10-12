using CalentonesAgua.Data;
using CalentonesAgua.Services;
using Microsoft.Net.Http.Headers;

namespace CalentonesAgua.Middleware;

public sealed class AgentContentNegotiationMiddleware
{
    private readonly RequestDelegate _next;

    public AgentContentNegotiationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, AgentMarkdown markdown)
    {
        if (!HttpMethods.IsGet(context.Request.Method) && !HttpMethods.IsHead(context.Request.Method))
        {
            await _next(context);
            return;
        }

        var path = context.Request.Path.Value ?? "/";
        var origin = SeoPublicOrigin.Resolve(
            context.RequestServices.GetRequiredService<IConfiguration>(),
            context.Request);

        if (IsPassthroughPath(path))
        {
            await _next(context);
            return;
        }

        context.Response.OnStarting(() =>
        {
            EnsureVaryAccept(context.Response);
            return Task.CompletedTask;
        });

        if (IsAgentIndexPath(path))
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = AgentMarkdown.MarkdownContentType;
            context.Response.Headers.Append(HeaderNames.Link, "</llms.txt>; rel=\"describedby\"");
            if (!HttpMethods.IsHead(context.Request.Method))
            {
                await context.Response.WriteAsync(markdown.BuildLlmsTxt(origin));
            }

            return;
        }

        var accept = context.Request.Headers.Accept.ToString();
        var chosen = AcceptNegotiation.PreferredType(
            string.IsNullOrWhiteSpace(accept) ? null : accept,
            AcceptNegotiation.DocumentTypes);

        if (chosen is null && !string.IsNullOrWhiteSpace(accept))
        {
            context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
            context.Response.ContentType = "text/plain; charset=utf-8";
            context.Response.Headers.Append(HeaderNames.Link, $"</llms.txt>; rel=\"describedby\"");
            if (!HttpMethods.IsHead(context.Request.Method))
            {
                await context.Response.WriteAsync("Not Acceptable\n\nAvailable: text/html, text/markdown\n");
            }

            return;
        }

        var wantsMarkdown = chosen == "text/markdown";
        AppendAgentLinks(context, origin, path, wantsMarkdown);

        if (!wantsMarkdown)
        {
            await _next(context);
            return;
        }

        markdown.TryGetDocument(path, origin, out var status, out var body);
        context.Response.StatusCode = status;
        context.Response.ContentType = AgentMarkdown.MarkdownContentType;
        if (!HttpMethods.IsHead(context.Request.Method))
        {
            await context.Response.WriteAsync(body);
        }
    }

    internal static void EnsureVaryAccept(HttpResponse response)
    {
        EnsureVaryToken(response, "Accept");
        EnsureVaryToken(response, "Accept-Encoding");
    }

    private static void EnsureVaryToken(HttpResponse response, string token)
    {
        var tokens = new List<string>();
        foreach (var raw in response.Headers.Vary)
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            tokens.AddRange(raw.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
        }

        if (tokens.Any(t => t.Equals(token, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        response.Headers.Append(HeaderNames.Vary, token);
    }

    private static void AppendAgentLinks(HttpContext context, string origin, string path, bool markdownResponse)
    {
        origin = origin.TrimEnd('/');
        var links = new List<string>
        {
            $"</llms.txt>; rel=\"describedby\""
        };
        if (!markdownResponse)
        {
            var href = string.IsNullOrEmpty(path) ? "/" : path;
            links.Add($"<{origin}{href}>; rel=\"alternate\"; type=\"text/markdown\"");
        }

        context.Response.Headers.Append(HeaderNames.Link, string.Join(", ", links));
    }

    internal static bool IsAgentIndexPath(string path)
    {
        path = path.TrimEnd('/');
        return path.Equals("/llms.txt", StringComparison.OrdinalIgnoreCase) ||
               path.Equals("/.well-known/llms.txt", StringComparison.OrdinalIgnoreCase) ||
               path.Equals("/agent-instructions", StringComparison.OrdinalIgnoreCase);
    }

    internal static bool IsPassthroughPath(string path)
    {
        if (IsAgentIndexPath(path))
        {
            return false;
        }

        if (path.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase) ||
            path.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/admin/", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var file = path.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? "";
        return file.Contains('.', StringComparison.Ordinal);
    }
}

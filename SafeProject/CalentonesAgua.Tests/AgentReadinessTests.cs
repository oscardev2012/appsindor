using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CalentonesAgua.Tests;

public class AgentReadinessTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AgentReadinessTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Unknown_path_returns_http_404()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.GetAsync("/some-path-that-does-not-exist");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Unknown_path_markdown_is_404_with_recovery_links()
    {
        var response = await Send("/", HttpMethod.Get, "text/markdown", "/some-path-that-does-not-exist");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        AssertMarkdown(response);
        Assert.True(VariesByAccept(response));
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("/llms.txt", body, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/sitemap.xml", body, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("docs index", body, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("sitemap", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<html", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Unknown_path_html_404_lists_llms_sitemap_and_docs_index()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.GetAsync("/some-path-that-does-not-exist");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("llms.txt", html);
        Assert.Contains("sitemap", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("docs index", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Homepage_accept_markdown_returns_markdown_and_vary_accept()
    {
        var response = await Send("/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        AssertMarkdown(response);
        Assert.True(VariesByAccept(response));
        Assert.True(VariesBy(response, "Accept-Encoding"));
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("SAFE Water Heater", body);
        Assert.Contains("$1,199", body);
        Assert.DoesNotContain("site-header", body);
    }

    [Fact]
    public async Task Homepage_html_still_renders_and_varies_by_accept()
    {
        var response = await Send("/", accept: "text/html");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        Assert.True(VariesByAccept(response));
        Assert.True(VariesBy(response, "Accept-Encoding"));
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("SAFE Water Heater", html);
        Assert.Contains("application/ld+json", html);
        Assert.Contains("rel=\"describedby\"", html);
    }

    [Fact]
    public async Task Faqs_html_includes_faqpage_schema_and_direct_answers()
    {
        var response = await Send("/faqs/", accept: "text/html");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("FAQPage", html);
        Assert.Contains("How much does water heater replacement cost in Charlotte NC?", html);
        Assert.Contains("$1,199", html);
        Assert.Contains("704-839-6126", html);
        Assert.Contains("650-669-6890", html);
        Assert.Contains("HOME INDOR TECHNOLOGY INC", html);
    }

    [Fact]
    public async Task Faqs_markdown_lists_direct_answers()
    {
        var response = await Send("/faqs/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        AssertMarkdown(response);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("How much does water heater replacement cost in Charlotte NC?", body);
        Assert.Contains("$1,199", body);
    }

    [Fact]
    public async Task Seo_hub_accept_markdown_returns_page_prose()
    {
        var response = await Send("/water-heater-cost-charlotte-nc/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        AssertMarkdown(response);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("#", body);
        Assert.Contains("$1,199", body);
    }

    [Fact]
    public async Task Unsupported_accept_returns_406()
    {
        var response = await Send("/", accept: "application/pdf");
        Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        Assert.True(VariesByAccept(response));
    }

    [Fact]
    public async Task Q_values_prefer_html_when_html_ranks_higher()
    {
        var response = await Send("/", accept: "text/html, text/markdown;q=0.8");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Q_values_prefer_markdown_when_markdown_ranks_higher()
    {
        var response = await Send("/", accept: "text/markdown, text/html;q=0.9");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        AssertMarkdown(response);
    }

    [Fact]
    public async Task Llms_txt_has_when_to_use_guidance()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/llms.txt");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        AssertMarkdown(response);
        var body = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("# SAFE Water Heater", body.TrimStart('\uFEFF').TrimStart());
        Assert.Contains("\n> ", body.Replace("\r\n", "\n"));
        Assert.Contains("## When to use this", body);
        Assert.Contains("How an agent should call SAFE", body, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/water-heater-replacement-charlotte-nc/", body);
        Assert.Contains("## Optional", body);
    }

    [Theory]
    [InlineData("/llms.txt")]
    [InlineData("/.well-known/llms.txt")]
    [InlineData("/agent-instructions")]
    public async Task Agent_instruction_aliases_serve_the_same_markdown(string path)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        AssertMarkdown(response);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("## When to use this", body);
    }

    [Fact]
    public async Task Robots_points_at_llms_txt()
    {
        var client = _factory.CreateClient();
        var body = await client.GetStringAsync("/robots.txt");
        Assert.Contains("/llms.txt", body);
        Assert.Contains("/sitemap.xml", body);
    }

    [Fact]
    public async Task Homepage_json_ld_script_has_organization_name_and_description()
    {
        var html = await _factory.CreateClient().GetStringAsync("/");
        var start = html.IndexOf("application/ld+json", StringComparison.OrdinalIgnoreCase);
        Assert.True(start >= 0);
        var jsonStart = html.IndexOf('{', start);
        var jsonEnd = html.IndexOf("</script>", jsonStart, StringComparison.OrdinalIgnoreCase);
        var json = html[jsonStart..jsonEnd];
        using var doc = JsonDocument.Parse(json);
        JsonElement org;
        if (doc.RootElement.TryGetProperty("@graph", out var graph))
        {
            JsonElement? found = null;
            foreach (var node in graph.EnumerateArray())
            {
                if (node.TryGetProperty("@type", out var type) &&
                    type.ValueKind == JsonValueKind.String &&
                    type.GetString() == "Organization")
                {
                    found = node;
                    break;
                }
            }

            Assert.True(found.HasValue);
            org = found.Value;
        }
        else
        {
            Assert.Equal("Organization", doc.RootElement.GetProperty("@type").GetString());
            org = doc.RootElement;
        }

        Assert.False(string.IsNullOrWhiteSpace(org.GetProperty("name").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(org.GetProperty("description").GetString()));
    }

    [Fact]
    public async Task Reviews_page_is_public_and_asks_for_google_reviews()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var slash = await client.GetAsync("/reviews");
        Assert.Equal(HttpStatusCode.MovedPermanently, slash.StatusCode);
        Assert.Contains("/reviews/", slash.Headers.Location?.ToString() ?? "", StringComparison.OrdinalIgnoreCase);

        var response = await client.GetAsync("/reviews/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("HOME INDOR TECHNOLOGY INC", html);
        Assert.Contains("Write a review", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("AggregateRating", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Language_combo_sets_cookie_and_keeps_reviews_page()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.GetAsync("/lang?code=es&returnUrl=/reviews/");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/reviews/", response.Headers.Location?.ToString() ?? "", StringComparison.OrdinalIgnoreCase);
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var cookies));
        Assert.Contains(cookies, c => c.Contains("safe_lang=es", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Language_combo_sends_english_replacement_to_spanish_hub()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.GetAsync("/lang?code=es&returnUrl=/water-heater-replacement-charlotte-nc/");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/es/reemplazo-calentador-agua-charlotte-nc/", response.Headers.Location?.ToString() ?? "", StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Language_combo_rejects_external_return_url()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.GetAsync("/lang?code=es&returnUrl=https://evil.example/");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var loc = response.Headers.Location?.ToString() ?? "";
        Assert.DoesNotContain("evil.example", loc, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Reviews_page_in_spanish_when_language_cookie_is_es()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        client.DefaultRequestHeaders.Add("Cookie", "safe_lang=es");
        var response = await client.GetAsync("/reviews/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("PARA EL CLIENTE", html);
        Assert.Contains("html lang=\"es\"", html);
        Assert.Contains("lang-switch", html);
        Assert.Contains("title=\"Español\"", html);
    }

    [Fact]
    public async Task Sitemap_xml_still_serves()
    {
        var response = await _factory.CreateClient().GetAsync("/sitemap.xml");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/xml", response.Content.Headers.ContentType?.MediaType);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("/water-heater-replacement-charlotte-nc/", body);
        Assert.Contains("/reviews/", body);
        Assert.Contains("/faqs/", body);
        Assert.DoesNotContain("/financing/safe-plan", body);
        Assert.DoesNotContain("/warranty/register", body);
    }

    [Fact]
    public async Task Robots_and_sitemap_accept_head()
    {
        var client = _factory.CreateClient();
        var robots = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, "/robots.txt"));
        var sitemap = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, "/sitemap.xml"));
        Assert.Equal(HttpStatusCode.OK, robots.StatusCode);
        Assert.Equal(HttpStatusCode.OK, sitemap.StatusCode);
    }

    [Fact]
    public async Task Sitemap_xml_serves_when_client_asks_for_xml()
    {
        var client = _factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/sitemap.xml");
        request.Headers.TryAddWithoutValidation("Accept", "application/xml,text/xml,*/*;q=0.8");
        request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
        var response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("<urlset", body);
        Assert.Contains("http://www.sitemaps.org/schemas/sitemap/0.9", body);
        Assert.Contains("/privacy/", body);
        Assert.Contains("/water-heater-replacement-charlotte-nc/", body);
        Assert.DoesNotContain("<html", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Sitemap_xml_serves_in_production_pipeline()
    {
        var factory = _factory.WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://www.safewaterheaterclt.com")
        });
        var response = await client.GetAsync("/sitemap.xml");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("<?xml", body.TrimStart());
        Assert.Contains("https://www.safewaterheaterclt.com/", body);
    }

    private async Task<HttpResponseMessage> Send(string url, HttpMethod? method = null, string accept = "text/markdown", string? path = null)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        using var request = new HttpRequestMessage(method ?? HttpMethod.Get, path ?? url);
        request.Headers.Accept.Clear();
        request.Headers.TryAddWithoutValidation("Accept", accept);
        return await client.SendAsync(request);
    }

    private static void AssertMarkdown(HttpResponseMessage response)
    {
        var type = response.Content.Headers.ContentType;
        Assert.Equal("text/markdown", type?.MediaType);
        Assert.Equal("utf-8", type?.CharSet);
    }

    private static bool VariesByAccept(HttpResponseMessage response) => VariesBy(response, "Accept");

    private static bool VariesBy(HttpResponseMessage response, string token)
    {
        if (!response.Headers.TryGetValues("Vary", out var values))
        {
            return false;
        }

        return values
            .SelectMany(v => v.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .Any(t => t.Equals(token, StringComparison.OrdinalIgnoreCase));
    }
}

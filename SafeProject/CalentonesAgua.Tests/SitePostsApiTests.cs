using System.Net;
using System.Net.Http.Json;
using CalentonesAgua.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace CalentonesAgua.Tests;

public class SitePostsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly string _path;
    private readonly WebApplicationFactory<Program> _factory;

    public SitePostsApiTests(WebApplicationFactory<Program> factory)
    {
        _path = Path.Combine(Path.GetTempPath(), "site-posts-api-" + Guid.NewGuid().ToString("n") + ".json");
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Updates:StorePath"] = _path,
                    ["ConnectionStrings:IndorDB"] = "",
                });
            });
        });
    }

    [Fact]
    public async Task Unknown_site_is_404_and_empty_known_site_is_204()
    {
        var client = _factory.CreateClient();
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/api/site-posts/not-a-site")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await client.GetAsync("/api/site-posts/safe")).StatusCode);
    }

    [Fact]
    public async Task Publish_via_store_then_api_returns_latest()
    {
        var store = new SitePostStore(new StubEnv(), new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Updates:StorePath"] = _path,
                ["ConnectionStrings:IndorDB"] = "",
            })
            .Build());
        store.Add("safe", "Charlotte crew update", "Serving Mint Hill this week.", null);

        var api = await _factory.CreateClient().GetFromJsonAsync<LatestDto>("/api/site-posts/safe");
        Assert.NotNull(api);
        Assert.Equal("Charlotte crew update", api.Title);
        Assert.Contains("Mint Hill", api.Body);
    }

    [Fact]
    public async Task Robots_disallows_admin()
    {
        var body = await _factory.CreateClient().GetStringAsync("/robots.txt");
        Assert.Contains("Disallow: /admin", body);
    }

    private sealed class LatestDto
    {
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
    }

    private sealed class StubEnv : IWebHostEnvironment
    {
        public string WebRootPath { get; set; } = "";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string ApplicationName { get; set; } = "test";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = Path.GetTempPath();
        public string EnvironmentName { get; set; } = Environments.Development;
    }
}

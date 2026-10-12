using CalentonesAgua.Models;
using CalentonesAgua.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CalentonesAgua.Tests;

public class QuoteLeadTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly string _path;
    private readonly WebApplicationFactory<Program> _factory;

    public QuoteLeadTests(WebApplicationFactory<Program> factory)
    {
        _path = Path.Combine(Path.GetTempPath(), "quote-leads-" + Guid.NewGuid().ToString("n") + ".json");
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Updates:QuoteLeadStorePath"] = _path,
                    ["ConnectionStrings:IndorDB"] = "",
                });
            });
        });
    }

    [Fact]
    public void Save_writes_contact_and_utm_when_sql_is_empty()
    {
        var store = NewStore(_path);
        var reference = store.Save(new SeoQuoteViewModel
        {
            Name = "Jane Homeowner",
            Phone = "7045550100",
            Email = "jane@example.com",
            Address = "100 Test St",
            City = "Charlotte",
            Fuel = "Electric tank",
            TankSize = "40 gallon",
            Issue = "Leaking",
            SmsConsent = true,
            UtmSource = "google",
            UtmCampaign = "replacement",
            LandingPath = "/water-heater-replacement-quote/?utm_source=google",
        });

        Assert.StartsWith("QT-", reference);
        var lead = Assert.Single(store.RecentFromFile());
        Assert.Equal("Jane Homeowner", lead.FullName);
        Assert.Equal("7045550100", lead.Phone);
        Assert.Equal("google", lead.UtmSource);
        Assert.Equal("replacement", lead.UtmCampaign);
        Assert.True(lead.SmsConsent);
    }

    [Fact]
    public async Task Quote_page_keeps_utm_for_paid_traffic()
    {
        var html = await _factory.CreateClient().GetStringAsync("/water-heater-replacement-quote/?utm_source=google&utm_campaign=replacement");
        Assert.Contains("name=\"UtmSource\"", html);
        Assert.Contains("value=\"google\"", html);
        Assert.Contains("name=\"UtmCampaign\"", html);
        Assert.Contains("value=\"replacement\"", html);
        Assert.Contains("Submit quote request", html);
        Assert.Contains("name=\"Name\"", html);
        Assert.Contains("name=\"Phone\"", html);
        var formAt = html.IndexOf("id=\"quote-form\"", StringComparison.Ordinal);
        var articleAt = html.IndexOf("The eight photos that prevent", StringComparison.Ordinal);
        Assert.True(formAt > 0 && formAt < articleAt, "Paid-traffic landing must show the form before the SEO article.");
    }

    private static QuoteLeadStore NewStore(string path) =>
        new(new StubEnv(), new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Updates:QuoteLeadStorePath"] = path,
                ["ConnectionStrings:IndorDB"] = "",
            })
            .Build(), NullLogger<QuoteLeadStore>.Instance);

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

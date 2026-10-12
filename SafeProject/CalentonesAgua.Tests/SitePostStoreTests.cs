using CalentonesAgua.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace CalentonesAgua.Tests;

public class SitePostStoreTests
{
    [Fact]
    public void Latest_returns_newest_post_for_that_site_only()
    {
        var dir = Path.Combine(Path.GetTempPath(), "site-posts-" + Guid.NewGuid().ToString("n"));
        Directory.CreateDirectory(dir);
        try
        {
            var path = Path.Combine(dir, "site-posts.json");
            var store = new SitePostStore(new StubEnv(), new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Updates:StorePath"] = path,
                    ["ConnectionStrings:IndorDB"] = "",
                })
                .Build());

            store.Add("safe", "SAFE first", "body", null);
            Thread.Sleep(5);
            store.Add("roofing", "Roofing live", "r", null);
            Thread.Sleep(5);
            store.Add("safe", "SAFE latest", "new", "https://www.safewaterheaterclt.com/safe-shield.svg");

            Assert.Equal("SAFE latest", store.Latest("safe")!.Title);
            Assert.Equal("Roofing live", store.Latest("roofing")!.Title);
            Assert.Null(store.Latest("realtor"));
            Assert.Null(store.Latest("unknown"));
            Assert.Equal(3, store.All().Count);
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }

    [Fact]
    public void SanitizeImageUrl_rejects_non_http()
    {
        Assert.Null(SitePostStore.SanitizeImageUrl("javascript:alert(1)"));
        Assert.StartsWith("https://", SitePostStore.SanitizeImageUrl("https://example.com/a.jpg"));
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

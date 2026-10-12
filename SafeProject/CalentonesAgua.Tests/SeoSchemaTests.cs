using System.Text.Json;
using CalentonesAgua.Data;
using Xunit;

namespace CalentonesAgua.Tests;

public class SeoSchemaTests
{
    [Fact]
    public void Organization_includes_name_and_description()
    {
        var json = SeoSchema.Graph(
            "https://www.safewaterheaterclt.com",
            extraTitle: "Charlotte, NC Water Heater Experts",
            pageUrl: "/",
            extraDescription: "Charlotte water heater replacement.");

        using var doc = JsonDocument.Parse(json);
        var graph = doc.RootElement.GetProperty("@graph");
        JsonElement? organization = null;
        foreach (var node in graph.EnumerateArray())
        {
            if (node.TryGetProperty("@type", out var type) &&
                type.ValueKind == JsonValueKind.String &&
                type.GetString() == "Organization")
            {
                organization = node;
                break;
            }
        }

        Assert.True(organization.HasValue, "JSON-LD graph must include an Organization node.");
        var org = organization.Value;
        Assert.Equal("SAFE Water Heater", org.GetProperty("name").GetString());
        Assert.Equal("HOME INDOR TECHNOLOGY INC", org.GetProperty("legalName").GetString());
        Assert.False(string.IsNullOrWhiteSpace(org.GetProperty("description").GetString()));
        Assert.Contains("SAFE Water Heater", org.GetProperty("description").GetString());
        Assert.Equal("ContactPoint", org.GetProperty("contactPoint").GetProperty("@type").GetString());
    }

    [Fact]
    public void Organization_card_exposes_name_and_description_at_root()
    {
        var json = SeoSchema.OrganizationCard("https://www.safewaterheaterclt.com");
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        Assert.False(root.TryGetProperty("@graph", out _));
        Assert.Equal("Organization", root.GetProperty("@type").GetString());
        Assert.Equal("SAFE Water Heater", root.GetProperty("name").GetString());
        Assert.False(string.IsNullOrWhiteSpace(root.GetProperty("description").GetString()));
        Assert.Equal("https://www.safewaterheaterclt.com/", root.GetProperty("url").GetString());
    }

    [Fact]
    public void Local_business_includes_brand_name_and_description()
    {
        var json = SeoSchema.Graph("https://www.safewaterheaterclt.com", extraTitle: "Home", pageUrl: "/");
        using var doc = JsonDocument.Parse(json);
        JsonElement? business = null;
        foreach (var node in doc.RootElement.GetProperty("@graph").EnumerateArray())
        {
            if (!node.TryGetProperty("legalName", out _)) continue;
            business = node;
            break;
        }

        Assert.True(business.HasValue);
        Assert.Equal("SAFE Water Heater", business.Value.GetProperty("name").GetString());
        Assert.False(string.IsNullOrWhiteSpace(business.Value.GetProperty("description").GetString()));
        Assert.Equal("SAFE Water Heater", business.Value.GetProperty("brand").GetProperty("name").GetString());
    }
}

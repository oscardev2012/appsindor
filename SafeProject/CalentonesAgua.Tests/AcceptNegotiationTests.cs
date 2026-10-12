using CalentonesAgua.Services;
using Xunit;

namespace CalentonesAgua.Tests;

public class AcceptNegotiationTests
{
    [Fact]
    public void Missing_header_defaults_to_html()
    {
        Assert.Equal("text/html", AcceptNegotiation.PreferredType(null, AcceptNegotiation.DocumentTypes));
        Assert.Equal("text/html", AcceptNegotiation.PreferredType("", AcceptNegotiation.DocumentTypes));
    }

    [Fact]
    public void Markdown_only_selects_markdown()
    {
        Assert.Equal("text/markdown", AcceptNegotiation.PreferredType("text/markdown", AcceptNegotiation.DocumentTypes));
    }

    [Fact]
    public void Browser_accept_selects_html()
    {
        var accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        Assert.Equal("text/html", AcceptNegotiation.PreferredType(accept, AcceptNegotiation.DocumentTypes));
    }

    [Fact]
    public void Higher_q_wins()
    {
        Assert.Equal("text/html", AcceptNegotiation.PreferredType("text/markdown;q=0.2, text/html;q=0.8", AcceptNegotiation.DocumentTypes));
        Assert.Equal("text/markdown", AcceptNegotiation.PreferredType("text/markdown, text/html;q=0.9", AcceptNegotiation.DocumentTypes));
        Assert.Equal("text/html", AcceptNegotiation.PreferredType("text/html, text/markdown;q=0.8", AcceptNegotiation.DocumentTypes));
    }

    [Fact]
    public void Q_zero_is_rejected()
    {
        Assert.Equal("text/markdown", AcceptNegotiation.PreferredType("text/html;q=0, text/markdown", AcceptNegotiation.DocumentTypes));
        Assert.Null(AcceptNegotiation.PreferredType("text/html;q=0, text/markdown;q=0", AcceptNegotiation.DocumentTypes));
    }

    [Fact]
    public void Unsupported_type_is_null_so_caller_can_return_406()
    {
        Assert.Null(AcceptNegotiation.PreferredType("application/pdf", AcceptNegotiation.DocumentTypes));
        Assert.Null(AcceptNegotiation.PreferredType("application/xml", AcceptNegotiation.DocumentTypes));
    }

    [Fact]
    public void Star_star_selects_html_first()
    {
        Assert.Equal("text/html", AcceptNegotiation.PreferredType("*/*", AcceptNegotiation.DocumentTypes));
    }
}

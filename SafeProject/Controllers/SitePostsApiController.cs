using CalentonesAgua.Data;
using CalentonesAgua.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

[ApiController]
[Route("api/site-posts")]
[EnableCors("SitePosts")]
public class SitePostsApiController : ControllerBase
{
    private readonly SitePostStore _store;

    public SitePostsApiController(SitePostStore store) => _store = store;

    [HttpGet("{site}")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
    public IActionResult Latest(string site)
    {
        if (!SitePostCatalog.TryNormalize(site, out var key))
        {
            return NotFound();
        }

        var post = _store.Latest(key);
        if (post is null)
        {
            return NoContent();
        }

        return Ok(new
        {
            title = post.Title,
            body = post.Body,
            imageUrl = post.ImageUrl,
            publishedUtc = post.CreatedUtc,
        });
    }
}

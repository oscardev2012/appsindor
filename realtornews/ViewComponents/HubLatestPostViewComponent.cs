using Microsoft.AspNetCore.Mvc;
using RealtorNews.Services;

namespace RealtorNews.ViewComponents;

public sealed class HubLatestPostViewComponent : ViewComponent
{
    private readonly HubLatestPostClient _client;

    public HubLatestPostViewComponent(HubLatestPostClient client) => _client = client;

    public IViewComponentResult Invoke()
    {
        var post = _client.Get();
        return post is null || string.IsNullOrWhiteSpace(post.Title)
            ? Content("")
            : View(post);
    }
}

using Microsoft.AspNetCore.Mvc;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.ViewComponents;

public sealed class LatestNewsViewComponent : ViewComponent
{
    private readonly RealtorNewsStore _store;

    public LatestNewsViewComponent(RealtorNewsStore store)
    {
        _store = store;
    }

    public IViewComponentResult Invoke()
    {
        IReadOnlyList<NewsPost> posts = [];
        try
        {
            if (_store.Probe() is null)
            {
                posts = _store.Published(take: 6);
            }
        }
        catch
        {
            posts = [];
        }

        return View(posts);
    }
}

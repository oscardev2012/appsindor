using CalentonesAgua.Data;

namespace CalentonesAgua;

public sealed class SeoEnglishSlugConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (httpContext is null || !values.TryGetValue(routeKey, out var raw) || raw is null)
        {
            return false;
        }

        var catalog = httpContext.RequestServices.GetRequiredService<SeoCatalog>();
        return catalog.Contains(raw.ToString());
    }
}

public sealed class SeoSpanishSlugConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (httpContext is null || !values.TryGetValue(routeKey, out var raw) || raw is null)
        {
            return false;
        }

        var catalog = httpContext.RequestServices.GetRequiredService<SeoCatalog>();
        return catalog.Contains("es/" + raw);
    }
}

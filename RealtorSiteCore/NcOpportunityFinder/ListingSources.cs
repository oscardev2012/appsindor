using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace NcOpportunityFinder;

public record SearchContext(
    string State, string StateName, string City, double Latitude, double Longitude,
    string MarketplaceSlug, string CraigslistSite, List<string> FacebookGroups, int Limit, List<string>? Keywords = null)
{
    /// <summary>The keywords the person typed for this search, or the source's own phrases from the keyword guide.</summary>
    public IReadOnlyList<string> Phrases(IEnumerable<string> defaults) => Keywords is { Count: > 0 } typed ? typed : [.. defaults];

    public bool HasKeywords => Keywords is { Count: > 0 };
}

/// <summary>
/// One Apify actor that can find houses for sale in a city. Each source knows how to ask its
/// actor for that city and how to turn one result into a property import, or skip it.
/// </summary>
public abstract class ListingSource
{
    public abstract string Key { get; }
    public abstract string Label { get; }
    public virtual bool NeedsFacebookGroups => false;
    public abstract object BuildInput(SearchContext c);

    /// <summary>Actors that take a single query get one run per phrase; the runs go in parallel and their results are merged.</summary>
    public virtual IEnumerable<object> BuildInputs(SearchContext c) => [BuildInput(c)];

    public abstract PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason);

    /// <summary>Input for the configured detail actor, built from the results that will be imported.</summary>
    public virtual object? BuildDetailInput(IReadOnlyList<JsonElement> kept) => null;

    public virtual List<JsonElement> MergeDetails(List<JsonElement> items, List<JsonElement> details) => items;

    public static readonly IReadOnlyList<ListingSource> All =
    [
        new MarketplaceSource(), new FacebookPostsSource(), new FacebookGroupPostsSource(), new FacebookGroupsSource(),
        new CraigslistSource(), new ZillowFsboSource(),
        new XSource(), new RedditSource(), new InstagramSource(), new TikTokSource()
    ];

    protected static string Slug(string value) => Regex.Replace(value.ToLowerInvariant(), "[^a-z0-9]", "");

    protected static string Place(SearchContext c) => $"{c.City} {c.State}";

    /// <summary>("fixer upper" OR "casa para reparar") for search boxes that accept OR.</summary>
    protected static string AnyOf(IEnumerable<string> phrases) => "(" + string.Join(" OR ", phrases.Select(p => $"\"{p}\"")) + ")";
}

/// <summary>
/// Marketplace ignores the city and category in a category URL and returns whatever sells near the
/// scraper's proxy (punching bags in Washington for a Charlotte search). A search URL keeps the
/// city, so the actor gets searches and every result is checked for distance and for being a home.
/// </summary>
public sealed partial class MarketplaceSource : ListingSource
{
    private const string HomesForSaleCategory = "821056594720130";
    public override string Key => "marketplace";
    public override string Label => "Facebook Marketplace";

    public override object BuildInput(SearchContext c) => new
    {
        startUrls = c.Phrases(PropertyKeywords.SearchPhrasesEn.Concat(PropertyKeywords.SearchPhrasesEs).Append("home for sale").Append("mobile home with land for sale"))
            .Select(q => new { url = $"https://www.facebook.com/marketplace/{c.MarketplaceSlug}/search/?query={Uri.EscapeDataString(q)}&exact=false" })
            .ToArray(),
        resultsLimit = c.Limit,
        includeListingDetails = true
    };

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        skipReason = null;
        if (J.Bool(item, "isSold") == true || J.Bool(item, "is_sold") == true || J.Bool(item, "isHidden") == true || J.Bool(item, "is_hidden") == true) return Skip("sold", out skipReason);

        var id = J.Str(item, "id");
        var title = J.Str(item, "listingTitle", "marketplace_listing_title", "custom_title");
        var description = J.Str(J.Path(item, "description"), "text") ?? J.Str(item, "description");
        var details = J.Arr(item, "details").SelectMany(d => J.Arr(d, "pdp_fields")).Select(f => J.Str(f, "display_label")).OfType<string>()
            .Select(l => l.Replace('·', ',')).Distinct().ToList();
        var subtitles = J.Arr(item, "customSubTitlesWithRenderingFlags").Concat(J.Arr(item, "custom_sub_titles_with_rendering_flags")).Select(s => J.Str(s, "subtitle")).OfType<string>();
        var attributes = J.Arr(item, "listingAttributes").Select(a => $"{J.Str(a, "attribute_name")}: {J.Str(a, "label")}");

        var price = J.Num(J.Path(item, "listingPrice") ?? J.Path(item, "listing_price"), "amount");
        var text = ListingText.Join(title, string.Join("\n", details), description);
        var isHomeCategory = J.Str(item, "listingCategoryId", "marketplace_listing_category_id") == HomesForSaleCategory;
        var homePost = ListingText.LooksLikeHome(text) && !ListingText.LooksLikeRent(text) && (price >= 10_000 || ListingText.LooksLikeSale(text));
        if (!isHomeCategory && !homePost) return Skip("not a home for sale", out skipReason);
        if (price is > 0 and < 5_000) return Skip("rent or not a home price", out skipReason);
        if (ListingText.IsAgentOrBuyer(ListingText.Join(title, description))) return Skip("agent post", out skipReason);

        var location = J.Path(item, "location");
        var geo = J.Path(location, "reverse_geocode_detailed") ?? J.Path(location, "reverse_geocode");
        var lat = J.Num(location, "latitude");
        var lon = J.Num(location, "longitude");
        var (city, state) = CityState(J.Str(geo, "city"), J.Str(geo, "state"),
            J.Str(J.Path(item, "locationText"), "text") ?? subtitles.Concat(details).FirstOrDefault(l => CityStatePattern().IsMatch(l)));
        if (lat is decimal la && lon is decimal lo)
        {
            if (ListingText.Miles((double)la, (double)lo, c.Latitude, c.Longitude) > ListingText.MaxMilesFromCity) return Skip("too far from city", out skipReason);
        }
        else if (state is null || !ListingText.SameState(state, c)) return Skip("other state", out skipReason);

        var images = J.Arr(item, "listingPhotos").Select(p => J.Str(J.Path(p, "image"), "uri")).OfType<string>().ToList();
        var primaryPhoto = J.Path(item, "primaryListingPhoto") ?? J.Path(item, "primary_listing_photo");
        var videos = J.Arr(item, "listingVideo").Concat(J.Arr(item, "listing_video")).Select(v => J.Str(v, "playable_url_hd", "playable_url")).OfType<string>().ToList();

        var fullPost = ListingText.Join(
            description,
            details.Count == 0 ? null : "Listing details: " + string.Join(" · ", details),
            attributes.Any() ? string.Join(" · ", attributes) : null,
            J.Str(item, "condition") is string condition && !attributes.Any() ? $"Condition: {condition}" : null,
            (J.Bool(item, "isPending") ?? J.Bool(item, "is_pending")) == true ? "Status: pending" : null,
            videos.Count == 0 ? null : $"Video: {videos.Count}");

        var dto = ListingText.Build(Label, id is null ? null : $"fbmkt-{id}",
            J.Str(item, "itemUrl", "listingUrl") ?? (id is null ? null : $"https://www.facebook.com/marketplace/item/{id}/"),
            title, text, fullPost, price ?? ListingText.Price(text), city ?? c.City, c, images,
            J.Date(item, "timestamp", "creation_time") ?? ListedAgo(details),
            J.Str(J.Path(item, "marketplace_listing_seller") ?? J.Path(item, "marketplaceListingSeller"), "name"), null);
        return dto with
        {
            PostType = isHomeCategory ? "Facebook Marketplace · Property for sale" : "Facebook Marketplace",
            State = state ?? c.State,
            Zip = J.Str(geo, "postal_code") is string zip ? zip.Split('-')[0] : null,
            Latitude = lat,
            Longitude = lon
        };
    }

    private static (string? City, string? State) CityState(string? city, string? state, string? text)
    {
        if (city is not null && state is not null) return (city, state);
        var m = text is null ? null : CityStatePattern().Match(text);
        return m is { Success: true } ? (city ?? m.Groups[1].Value.Trim(), state ?? m.Groups[2].Value) : (city, state);
    }

    /// <summary>"Listed 2 days ago", "Listed about a day ago", "Listed over a week ago".</summary>
    private static DateTime? ListedAgo(List<string> details)
    {
        var m = details.Select(d => ListedPattern().Match(d)).FirstOrDefault(x => x.Success);
        if (m is null) return null;
        var n = int.TryParse(m.Groups[1].Value, out var v) ? v : 1;
        var unit = m.Groups[2].Value.ToLowerInvariant();
        var span = unit.StartsWith("hour") ? TimeSpan.FromHours(n)
            : unit.StartsWith("week") ? TimeSpan.FromDays(7 * n)
            : unit.StartsWith("month") ? TimeSpan.FromDays(30 * n)
            : unit.StartsWith("minute") ? TimeSpan.FromMinutes(n)
            : TimeSpan.FromDays(n);
        return DateTime.UtcNow - span;
    }

    private static PropertyImportDto? Skip(string reason, out string? skipReason)
    {
        skipReason = reason;
        return null;
    }

    [GeneratedRegex(@"^([A-Za-z .'-]+),\s*([A-Z]{2})$")]
    private static partial Regex CityStatePattern();

    [GeneratedRegex(@"Listed (?:about |over |almost )?(an?|\d+) (minute|hour|day|week|month)s? ago", RegexOptions.IgnoreCase)]
    private static partial Regex ListedPattern();
}

/// <summary>
/// Public Facebook posts found by keyword: personal profiles, pages and public groups. Facebook search
/// takes one phrase at a time, so each phrase from the keyword guide is its own run with the city added.
/// </summary>
public sealed class FacebookPostsSource : ListingSource
{
    public override string Key => "fbposts";
    public override string Label => "Facebook posts";

    public override object BuildInput(SearchContext c) => BuildInputs(c).First();

    public override IEnumerable<object> BuildInputs(SearchContext c) => c.Phrases(PropertyKeywords.FacebookSearchPhrases).Select(q => (object)new
    {
        query = $"{q} {c.City}",
        resultsCount = Math.Max(10, c.Limit / c.Phrases(PropertyKeywords.FacebookSearchPhrases).Count),
        searchType = "latest",
        startDate = DateTime.UtcNow.AddDays(-120).ToString("yyyy-MM-dd")
    });

    public override object? BuildDetailInput(IReadOnlyList<JsonElement> kept) => FacebookPostDetail.Input(kept, "url");

    public override List<JsonElement> MergeDetails(List<JsonElement> items, List<JsonElement> details) =>
        FacebookPostDetail.Merge(items, details, "url", "postId");

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        var shared = J.Path(item, "sharedPost");
        var author = J.Path(item, "author");
        var url = J.Str(item, "url");
        var full = FacebookPostDetail.Read(item);
        var images = J.Arr(item, "attachments").Concat(J.Arr(shared, "attachments"))
            .Where(a => J.Str(a, "type") is null or "photo")
            .Select(a => J.Str(a, "image", "thumbnail", "url")).OfType<string>()
            .Where(u => u.Contains("fbcdn.net") || u.Contains("scontent"));
        var dto = ListingText.FromPost(Label, J.Str(item, "postId", "id"), url,
            full.Text ?? ListingText.Join(J.Str(item, "postText", "text"), J.Str(shared, "postText", "text")),
            full.Author ?? J.Str(author, "name"), full.AuthorUrl ?? J.Str(author, "profileUrl", "url"),
            J.Date(item, "timestamp", "time") ?? full.Posted, [.. full.Images.Concat(images)], c, out skipReason);
        return dto is null ? null : dto with
        {
            PostType = url?.Contains("/groups/") == true ? "Facebook · Group post" : "Facebook · Public post",
            RawText = ListingText.Join(dto.RawText, full.PhotoText),
            SellerPhone = dto.SellerPhone ?? ListingText.Phones(full.PhotoText ?? "")
        };
    }
}

/// <summary>
/// Search actors return a preview: cut text and links to photo pages instead of images. The posts
/// actor opens each kept post and adds the full text, every photo, the author and the text Facebook
/// reads in the photos (a "for sale by owner" sign with a phone number, for example).
/// </summary>
public static class FacebookPostDetail
{
    public sealed record Full(string? Text, string? PhotoText, List<string> Images, string? Author, string? AuthorUrl, DateTime? Posted);

    public static object? Input(IReadOnlyList<JsonElement> kept, string urlField)
    {
        var urls = kept.Select(i => J.Str(i, urlField)).OfType<string>().Where(u => u.Contains("facebook.com")).Distinct().ToArray();
        return urls.Length == 0 ? null : new { startUrls = urls.Select(url => new { url }).ToArray(), resultsLimit = 1 };
    }

    public static List<JsonElement> Merge(List<JsonElement> items, List<JsonElement> details, string urlField, string idField)
    {
        if (details.Count == 0) return items;
        return items.Select(item =>
        {
            var id = J.Str(item, idField) ?? IdFromUrl(J.Str(item, urlField));
            var url = Trim(J.Str(item, urlField));
            var match = details.FirstOrDefault(d =>
                (id is not null && (J.Str(d, "postId") == id || IdFromUrl(J.Str(d, "topLevelUrl")) == id))
                || (url is not null && new[] { "url", "topLevelUrl", "inputUrl", "facebookUrl" }.Any(f => Trim(J.Str(d, f)) == url)));
            if (match.ValueKind != JsonValueKind.Object || JsonNode.Parse(item.GetRawText()) is not JsonObject merged) return item;
            merged["detail"] = JsonNode.Parse(match.GetRawText());
            return JsonDocument.Parse(merged.ToJsonString()).RootElement.Clone();
        }).ToList();
    }

    public static Full Read(JsonElement item)
    {
        var d = J.Path(item, "detail");
        if (d is null) return new Full(null, null, [], null, null, null);
        var media = J.Arr(d, "media").ToList();
        var images = media.Select(m => J.Str(J.Path(m, "photo_image"), "uri") ?? J.Str(m, "thumbnail")).OfType<string>().ToList();
        var ocr = media.Select(m => J.Str(m, "ocrText")).OfType<string>().Distinct().ToList();
        var user = J.Path(d, "user");
        return new Full(J.Str(d, "text"), ocr.Count == 0 ? null : "Photo text: " + string.Join(" · ", ocr), images,
            J.Str(user, "name"), J.Str(user, "profileUrl"), J.Date(d.Value, "time"));
    }

    private static string? Trim(string? url) => url?.Split('?')[0].TrimEnd('/');

    private static string? IdFromUrl(string? url) =>
        url is null ? null : Regex.Match(url, @"/(?:posts|permalink)/(\d+)") is { Success: true } m ? m.Groups[1].Value : null;
}

/// <summary>
/// Posts from public Facebook groups found by topic, without pasting group links. The actor checks
/// each post's meaning against the phrase; the group name is kept because it usually names the city.
/// </summary>
public sealed class FacebookGroupPostsSource : ListingSource
{
    public override string Key => "fbgroupsearch";
    public override string Label => "Facebook group posts";

    public override object BuildInput(SearchContext c) => new
    {
        keywords = c.Phrases(PropertyKeywords.FacebookSearchPhrases).Select(q => $"{q} {c.City} {c.StateName}").ToArray(),
        afterDate = "120 days",
        maxPosts = Math.Max(5, c.Limit / c.Phrases(PropertyKeywords.FacebookSearchPhrases).Count),
        countryCode = "us"
    };

    public override object? BuildDetailInput(IReadOnlyList<JsonElement> kept) => FacebookPostDetail.Input(kept, "post_url");

    public override List<JsonElement> MergeDetails(List<JsonElement> items, List<JsonElement> details) =>
        FacebookPostDetail.Merge(items, details, "post_url", "post_id");

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        if (J.Str(item, "result_type") is string type && type != "post" || J.Bool(item, "isRelevant") == false)
        {
            skipReason = "not a post";
            return null;
        }
        var group = J.Str(item, "group_name");
        var url = J.Str(item, "post_url");
        var authorId = J.Str(item, "author_id");
        var full = FacebookPostDetail.Read(item);
        var images = full.Images.Count > 0 ? full.Images : new[] { J.Str(item, "image_url") }.OfType<string>().ToList();
        var dto = ListingText.FromPost(Label, J.Str(item, "post_id", "id") ?? url, url,
            ListingText.Join(full.Text ?? J.Str(item, "text"), group is null ? null : $"Group: {group}"),
            full.Author ?? J.Str(item, "author_name"), full.AuthorUrl ?? (authorId is null ? null : $"https://www.facebook.com/{authorId}"),
            J.Date(item, "date") ?? full.Posted, images, c, out skipReason);
        return dto is null ? null : dto with
        {
            PostType = group is null ? "Facebook · Group post" : $"Facebook group · {group}",
            RawText = ListingText.Join(dto.RawText, J.Str(item, "group_url") is string groupUrl ? $"Group link: {groupUrl}" : null, full.PhotoText),
            SellerPhone = dto.SellerPhone ?? ListingText.Phones(full.PhotoText ?? "")
        };
    }
}

public sealed class FacebookGroupsSource : ListingSource
{
    public override string Key => "fbgroups";
    public override string Label => "Facebook Groups";
    public override bool NeedsFacebookGroups => true;

    public override object BuildInput(SearchContext c) => new
    {
        startUrls = c.FacebookGroups.Select(u => new { url = u }).ToArray(),
        resultsLimit = c.Limit,
        viewOption = "CHRONOLOGICAL",
        onlyPostsNewerThan = "60 days"
    };

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        var user = J.Path(item, "user");
        var images = J.Arr(item, "attachments")
            .Select(a => J.Str(J.Path(a, "photo_image"), "uri") ?? J.Str(a, "thumbnail", "url"))
            .OfType<string>().Where(u => u.StartsWith("http")).ToList();
        var userId = J.Str(user, "id");
        return ListingText.FromPost(Label, J.Str(item, "postId", "id"), J.Str(item, "url", "postUrl", "facebookUrl"),
            J.Str(item, "text", "message"), J.Str(user, "name"), userId is null ? null : $"https://www.facebook.com/{userId}",
            J.Date(item, "time", "date", "timestamp"), images, c, out skipReason);
    }
}

/// <summary>
/// Real estate for sale by owner. The by-owner filter still lets through rentals, bare land and
/// people buying land or houses, so those are skipped. Opening each post (includeDetails) adds the
/// full text, every photo, coordinates and any listed phone; reply-email capture is left off because
/// it costs $0.05 and about three minutes per post and only returns a Craigslist relay address.
/// </summary>
public sealed partial class CraigslistSource : ListingSource
{
    public override string Key => "craigslist";
    public override string Label => "Craigslist";

    public override object BuildInput(SearchContext c) => new
    {
        subdomain = c.CraigslistSite,
        category = "rea",
        sellerType = "owner",
        searchQueries = c.HasKeywords ? c.Keywords : null,
        includeDetails = true,
        extractReplyEmail = false,
        sort = "date",
        maxItems = c.Limit
    };

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        skipReason = null;
        var id = J.Str(item, "id");
        var title = J.Str(item, "title") ?? "";
        var body = J.Str(item, "post")?.Replace("QR Code Link to This Post", "").Trim();
        var all = ListingText.Join(title, body);
        if (J.Str(item, "rentPeriod") is not null || ListingText.LooksLikeRent(all)) return Skip("rent", out skipReason);
        if (ListingText.IsAgentOrBuyer(all) || (BuyerPattern().IsMatch(title) && PropertyKeywords.Classify(title).Direct.Count == 0))
            return Skip("buyer or agent post", out skipReason);
        if (BuildablePattern().IsMatch(title) || (LandPattern().IsMatch(title) && !HousePattern().IsMatch(title)))
            return Skip("land, not a house", out skipReason);
        if (!HousePattern().IsMatch(all)) return Skip("not a house", out skipReason);

        var lat = J.Num(item, "latitude");
        var lon = J.Num(item, "longitude");
        if (lat is decimal la && lon is decimal lo && ListingText.Miles((double)la, (double)lo, c.Latitude, c.Longitude) > ListingText.MaxMilesFromCity)
            return Skip("too far from city", out skipReason);

        var location = J.Str(item, "location") ?? (TitlePlacePattern().Match(title) is { Success: true } place ? place.Groups[1].Value : null);
        var cityState = location is null ? null : CityStatePattern().Match(location);
        var addressObj = J.Path(item, "address");
        var listedAddress = ListingText.Join(J.Str(addressObj, "street"), J.Str(addressObj, "city"));
        var details = ListingText.Join(
            J.Str(item, "bedrooms") is string bd ? $"{bd} bedrooms" : null,
            J.Str(item, "bathrooms") is string ba ? $"{ba} baths" : null,
            J.Str(item, "space") is string space ? $"{space.Replace("ft2", "")} sqft" : null,
            J.Strings(item, "amenities").Any() ? "Amenities: " + string.Join(", ", J.Strings(item, "amenities")) : null);
        var text = ListingText.Join(listedAddress, title, details, location, body, string.Join("\n", J.Strings(item, "phoneNumbers")));
        var price = J.Num(item, "price") is decimal p && p > 0 ? p : ListingText.Price(all);

        var dto = ListingText.Build(Label, id is null ? null : $"craigslist-{id}", J.Str(item, "url"), title,
            text, ListingText.Join(body, details), price,
            cityState is { Success: true } ? cityState.Groups[1].Value.Trim() : location ?? c.City, c,
            J.Strings(item, "pics").ToList(), J.Date(item, "datetime"), null, null);
        return dto with
        {
            PostType = "Craigslist · Real estate by owner",
            SellerSignal = PropertyKeywords.Classify(text).Signal("for sale by owner"),
            State = cityState is { Success: true } ? cityState.Groups[2].Value : c.State,
            Latitude = lat,
            Longitude = lon
        };
    }

    private static PropertyImportDto? Skip(string reason, out string? skipReason)
    {
        skipReason = reason;
        return null;
    }

    [GeneratedRegex(@"^\s*([A-Za-z .'-]+?)\s*,\s*([A-Z]{2})\.?\s*$")]
    private static partial Regex CityStatePattern();

    [GeneratedRegex(@"\b(lots?|land|acres?|ac|parcel|building site|homesite)\b", RegexOptions.IgnoreCase)]
    private static partial Regex LandPattern();

    [GeneratedRegex(@"\b(house|houses|home|homes|\d\s*(br|bd|bed|beds|bedrooms?)|bath|ranch|cottage|cabin|bungalow|condo|townhouse|townhome|duplex|triplex|fourplex|single ?wide|double ?wide|mobile home|manufactured|modular|fixer)\b", RegexOptions.IgnoreCase)]
    private static partial Regex HousePattern();

    [GeneratedRegex(@"\b(we buy|buying|wanted|looking for|cash for)\b", RegexOptions.IgnoreCase)]
    private static partial Regex BuyerPattern();

    [GeneratedRegex(@"\b(build your|buildable|building (lot|site)|homesite|land only|vacant land)\b", RegexOptions.IgnoreCase)]
    private static partial Regex BuildablePattern();

    [GeneratedRegex(@"\(([A-Za-z .'-]{3,40})\)\s*$")]
    private static partial Regex TitlePlacePattern();
}

public sealed class ZillowFsboSource : ListingSource
{
    public override string Key => "zillow";
    public override string Label => "Zillow FSBO";

    public override object BuildInput(SearchContext c)
    {
        var state = new
        {
            pagination = new { },
            isMapVisible = true,
            isListVisible = true,
            usersSearchTerm = $"{c.City}, {c.State}",
            mapBounds = new
            {
                west = Math.Round(c.Longitude - 0.25, 4), east = Math.Round(c.Longitude + 0.25, 4),
                south = Math.Round(c.Latitude - 0.2, 4), north = Math.Round(c.Latitude + 0.2, 4)
            },
            filterState = new
            {
                sort = new { value = "days" },
                fsba = new { value = false },
                nc = new { value = false },
                cmsn = new { value = false },
                auc = new { value = false },
                fore = new { value = false }
            }
        };
        return new
        {
            searchUrls = new[] { new { url = "https://www.zillow.com/homes/for_sale/?searchQueryState=" + Uri.EscapeDataString(JsonSerializer.Serialize(state)) } },
            extractionMethod = "PAGINATION_WITH_ZOOM_IN",
            resultsLimit = c.Limit
        };
    }

    public override object? BuildDetailInput(IReadOnlyList<JsonElement> kept)
    {
        var urls = kept.Select(i => J.Str(i, "propertyUrl")).OfType<string>().Distinct().ToArray();
        return urls.Length == 0 ? null : new { startUrls = urls.Select(url => new { url }).ToArray(), propertyStatus = "FOR_SALE" };
    }

    /// <summary>The detail page repeats every search field and adds history, features and the full description.</summary>
    public override List<JsonElement> MergeDetails(List<JsonElement> items, List<JsonElement> details)
    {
        var byZpid = details.Where(d => J.Str(d, "zpid") is not null).GroupBy(d => J.Str(d, "zpid")!).ToDictionary(g => g.Key, g => g.First());
        return items.Select(i => J.Str(i, "zpid") is string z && byZpid.TryGetValue(z, out var d) ? d : i).ToList();
    }

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        skipReason = null;
        var status = J.Str(item, "listingStatus");
        if (status is not null && !status.Equals("forSale", StringComparison.OrdinalIgnoreCase))
        {
            skipReason = "not for sale";
            return null;
        }
        var homeType = J.Str(item, "homeType");
        if (homeType is "LOT" or "LAND")
        {
            skipReason = "land, not a house";
            return null;
        }
        var address = J.Path(item, "listingAddress");
        if (!ListingText.SameState(J.Str(address, "state"), c))
        {
            skipReason = "other state";
            return null;
        }

        var listingType = J.Path(item, "listingType");
        var priceChange = J.Num(item, "priceChange");
        var signals = new List<string>();
        if (J.Bool(listingType, "isFSBO") == true) signals.Add("for sale by owner");
        if (priceChange < 0 || J.Path(item, "priceReduction") is { ValueKind: not JsonValueKind.Null }) signals.Add("price reduced");
        if (J.Bool(listingType, "isForeclosure") == true) signals.Add("foreclosure");
        if (J.Bool(listingType, "isBankOwned") == true) signals.Add("bank owned");

        var lot = J.Path(item, "lotArea");
        var openHouse = J.Str(J.Path(item, "openHouse"), "description");
        var facts = ListingText.Join(
            homeType is null ? null : $"Home type: {homeType.Replace('_', ' ').ToLowerInvariant()}",
            J.Str(lot, "formatted") is string lotText ? $"Lot: {lotText}" : null,
            J.Num(item, "taxAssessedValue") is decimal tax ? $"Tax assessed value: {ListingText.Usd(tax)}" : null,
            J.Num(item, "rentZestimate") is decimal rent ? $"Rent Zestimate: {ListingText.Usd(rent)}/month" : null,
            priceChange is decimal change && change != 0 ? $"Price change: {ListingText.Usd(change)}" : null,
            J.Int(item, "daysOnZillow") is int dz ? $"Days on Zillow: {dz}" : null,
            J.Int(item, "photoCount") is int pc ? $"Photos on Zillow: {pc}" : null,
            openHouse,
            DetailFacts(item));

        var images = J.Arr(item, "listingPhotos").Select(p => J.Str(p, "url")).OfType<string>().ToList();
        if (images.Count == 0 && J.Str(item, "mainImage") is string main) images.Add(main);
        var zpid = J.Str(item, "zpid");
        var street = J.Str(address, "street");
        var coords = J.Path(item, "coordinates");
        var days = J.Int(item, "daysOnZillow");

        return new PropertyImportDto(
            ExternalId: zpid is null ? null : $"zillow-{zpid}",
            Source: Label,
            SourceUrl: J.Str(item, "propertyUrl"),
            PostType: "Zillow · for sale by owner",
            PostedAt: J.Date(item, "onMarketDate", "datePosted") ?? (days is int d ? DateTime.UtcNow.Date.AddDays(-d) : null),
            Title: J.Str(address, "full") ?? street,
            Address: ListingText.NormalizeAddress(street),
            City: J.Str(address, "city") ?? c.City,
            State: c.State,
            Zip: J.Str(address, "zipCode"),
            Beds: J.Num(item, "bedrooms"),
            Baths: J.Num(item, "bathrooms"),
            AskingPrice: J.Num(J.Path(item, "listingPrice"), "amount"),
            SellerSignal: PropertyKeywords.Classify(J.Str(item, "description")).Signal([.. signals]),
            RawText: ListingText.Join(J.Str(item, "description"), facts),
            Images: images.Distinct().Take(40).ToList(),
            ComparablesCount: null, ArvLow: null, ArvHigh: null, ArvEstimate: null,
            RepairsLow: null, RepairsHigh: null, RepairsEstimate: null,
            ClosingHolding: null, Contingency: null, Score: null, Notes: null,
            SellerName: J.Str(J.Path(item, "broker"), "name"),
            SellerPhone: ListingText.Phones(J.Str(J.Path(item, "broker"), "phoneNumber") ?? ""),
            SellerEmail: null,
            SellerProfileUrl: null,
            MarketValue: J.Num(item, "zestimate"),
            Sqft: J.Int(item, "livingArea"),
            Latitude: J.Num(coords, "latitude"),
            Longitude: J.Num(coords, "longitude"));
    }

    private static string? DetailFacts(JsonElement item)
    {
        var features = J.Path(item, "propertyFeatures");
        string? List(string name) => J.Strings(features, name).ToList() is { Count: > 0 } l ? string.Join(", ", l) : null;
        var parkingFeatures = J.Strings(J.Path(item, "parking"), "features").ToList();
        var history = J.Arr(item, "listingPriceHistory").Take(6).Select(h =>
            $"  {J.Date(h, "date")?.ToString("yyyy-MM-dd")} · {J.Str(h, "event")} · {(J.Num(h, "price") is decimal hp ? ListingText.Usd(hp) : "—")}{(J.Str(h, "source") is string s ? $" ({s})" : "")}").ToList();
        var taxes = J.Arr(item, "listingTaxHistory").Take(3).Select(t => $"{J.Str(t, "year")}: {(J.Num(t, "value") is decimal v ? ListingText.Usd(v) : "—")}").ToList();
        var schools = J.Arr(item, "nearbySchools").Take(3).Select(s => $"{J.Str(s, "name")} ({J.Str(s, "rating") ?? "?"}/10, {J.Str(s, "distance")} mi)").ToList();
        return ListingText.Join(
            J.Str(item, "yearBuilt") is string year ? $"Year built: {year}" : null,
            J.Num(item, "pricePerSquareFoot") is decimal ppsf ? $"Price per sqft: {ListingText.Usd(ppsf)}" : null,
            J.Num(item, "lastSoldPrice") is decimal sold ? $"Last sold price: {ListingText.Usd(sold)}" : null,
            J.Str(item, "parcelId") is string parcel ? $"Parcel ID: {parcel}" : null,
            J.Num(item, "propertyTaxRate") is decimal rate ? $"Property tax rate: {rate.ToString(CultureInfo.InvariantCulture)}%" : null,
            List("heating") is string heat ? $"Heating: {heat}" : null,
            List("cooling") is string cool ? $"Cooling: {cool}" : null,
            J.Str(features, "roofType") is string roof ? $"Roof: {roof}" : null,
            J.Str(features, "basement") is string basement ? $"Basement: {basement}" : null,
            List("flooring") is string floors ? $"Flooring: {floors}" : null,
            List("appliances") is string appliances ? $"Appliances: {appliances}" : null,
            parkingFeatures.Count == 0 ? null : $"Parking: {string.Join(", ", parkingFeatures)}",
            J.Int(item, "pageViewCount") is int views ? $"Zillow views: {views} · saves: {J.Int(item, "favoriteCount") ?? 0}" : null,
            history.Count == 0 ? null : "Price history:\n" + string.Join("\n", history),
            taxes.Count == 0 ? null : "Tax assessments: " + string.Join(" · ", taxes),
            schools.Count == 0 ? null : "Schools: " + string.Join(" · ", schools));
    }
}

public sealed class XSource : ListingSource
{
    public override string Key => "x";
    public override string Label => "X (Twitter)";

    public override object BuildInput(SearchContext c) => new
    {
        searchTerms = c.HasKeywords
            ? [$"{AnyOf(c.Keywords!)} \"{c.City}\" -filter:retweets"]
            : new[]
        {
            $"(fsbo OR \"for sale by owner\" OR \"selling my house\" OR \"need to sell my house\" OR \"owner financing\") \"{c.City}\" -filter:retweets",
            $"(\"fixer upper\" OR \"needs work\" OR \"sold as is\" OR \"handyman special\" OR \"investor special\" OR \"inherited house\") house \"{c.City}\" -filter:retweets",
            $"(\"vendo mi casa\" OR \"casa en venta por dueño\" OR \"urge vender\" OR \"casa para reparar\") \"{c.City}\" -filter:retweets"
        },
        maxItems = Math.Max(c.Limit, 50),
        sort = "Latest",
        start = DateTime.UtcNow.AddDays(-120).ToString("yyyy-MM-dd")
    };

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        var author = J.Path(item, "author");
        var images = J.Arr(J.Path(item, "extendedEntities"), "media").Select(m => J.Str(m, "media_url_https")).OfType<string>().ToList();
        var links = J.Arr(J.Path(item, "entities"), "urls").Select(u => J.Str(u, "expanded_url")).OfType<string>();
        return ListingText.FromPost(Label, J.Str(item, "id"), J.Str(item, "url", "twitterUrl"),
            ListingText.Join(J.Str(item, "fullText", "text"), string.Join("\n", links)),
            J.Str(author, "name", "userName"), J.Str(author, "url"), J.Date(item, "createdAt"), images, c, out skipReason,
            ListingText.Join(J.Str(author, "userName"), J.Str(author, "description")));
    }
}

public sealed class RedditSource : ListingSource
{
    public override string Key => "reddit";
    public override string Label => "Reddit";

    public override object BuildInput(SearchContext c)
    {
        var city = Uri.EscapeDataString($"\"{c.City}\"");
        return new
        {
            startUrls = (c.HasKeywords
                ? [$"https://www.reddit.com/r/{Regex.Replace(c.City, "[^A-Za-z0-9]", "")}/search/?q={Uri.EscapeDataString(AnyOf(c.Keywords!))}&restrict_sr=1&sort=new&t=year",
                   $"https://www.reddit.com/search/?q={Uri.EscapeDataString(AnyOf(c.Keywords!))}+{city}&sort=new&t=year&type=link"]
                : new[]
            {
                $"https://www.reddit.com/r/{Regex.Replace(c.City, "[^A-Za-z0-9]", "")}/search/?q={Uri.EscapeDataString("\"for sale by owner\" OR fsbo OR \"selling my house\" OR \"need to sell my house\" OR \"fixer upper\" OR \"inherited house\"")}&restrict_sr=1&sort=new&t=year",
                $"https://www.reddit.com/search/?q={Uri.EscapeDataString("(\"for sale by owner\" OR \"selling my house\" OR fsbo)")}+{city}&sort=new&t=year&type=link",
                $"https://www.reddit.com/search/?q={Uri.EscapeDataString("(\"fixer upper\" OR \"needs work\" OR \"sold as is\" OR \"handyman special\") house")}+{city}&sort=new&t=year&type=link"
            }).Select(url => new { url }).ToArray(),
            maxItems = c.Limit,
            maxPostCount = c.Limit,
            maxComments = 0,
            skipComments = true,
            skipUserPosts = true,
            skipCommunity = true,
            includeMediaLinks = true,
            maxCommunitiesCount = 0,
            maxUserCount = 0,
            postDateLimit = DateTime.UtcNow.AddDays(-120).ToString("yyyy-MM-dd"),
            proxy = new { useApifyProxy = true, apifyProxyGroups = new[] { "RESIDENTIAL" } }
        };
    }

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        var type = J.Str(item, "dataType") ?? "post";
        if (type is "community" or "user")
        {
            skipReason = "not a post";
            return null;
        }
        var user = J.Str(item, "username", "author")?.TrimStart('/');
        return ListingText.FromPost(Label, J.Str(item, "parsedId", "id"), J.Str(item, "url", "postUrl"),
            ListingText.Join(J.Str(item, "title"), J.Str(item, "body", "text")), user,
            user is null ? null : $"https://www.reddit.com/user/{user}/", J.Date(item, "createdAt", "created"),
            J.Strings(item, "imageUrls").ToList(), c, out skipReason);
    }
}

public sealed class InstagramSource : ListingSource
{
    public override string Key => "instagram";
    public override string Label => "Instagram";

    public override object BuildInput(SearchContext c)
    {
        var city = Slug(c.City);
        return new
        {
            hashtags = c.HasKeywords
                ? c.Keywords!.Select(k => city + Slug(k)).Distinct().ToArray()
                : new[] { $"{city}fsbo", $"forsalebyowner{city}", $"{city}fixerupper", $"{city}homeforsale", $"{city}investmentproperty" },
            resultsType = "posts",
            resultsLimit = Math.Max(1, c.Limit / 5)
        };
    }

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        var owner = J.Str(item, "ownerUsername");
        var images = new[] { J.Str(item, "displayUrl") }.OfType<string>()
            .Concat(J.Strings(item, "images"))
            .Concat(J.Arr(item, "childPosts").Select(p => J.Str(p, "displayUrl")).OfType<string>())
            .ToList();
        return ListingText.FromPost(Label, J.Str(item, "id", "shortCode"), J.Str(item, "url"),
            ListingText.Join(J.Str(item, "caption"), J.Str(item, "locationName")), J.Str(item, "ownerFullName") ?? owner,
            owner is null ? null : $"https://www.instagram.com/{owner}/", J.Date(item, "timestamp"), images, c, out skipReason,
            owner);
    }
}

public sealed class TikTokSource : ListingSource
{
    public override string Key => "tiktok";
    public override string Label => "TikTok";

    public override object BuildInput(SearchContext c) => new
    {
        searchQueries = c.HasKeywords
            ? c.Keywords!.Select(k => $"{k} {c.City}").ToArray()
            : new[]
        {
            $"house for sale by owner {Place(c)}", $"selling my house {c.City}", $"fixer upper house for sale {c.City}",
            $"house needs work for sale {c.City}", $"vendo mi casa {c.City}"
        },
        resultsPerPage = Math.Max(1, c.Limit / 5),
        searchSection = "/video"
    };

    public override PropertyImportDto? Map(JsonElement item, SearchContext c, out string? skipReason)
    {
        var author = J.Path(item, "authorMeta");
        var name = J.Str(author, "name");
        var cover = J.Str(J.Path(item, "videoMeta"), "coverUrl");
        return ListingText.FromPost(Label, J.Str(item, "id"), J.Str(item, "webVideoUrl"), J.Str(item, "text"),
            J.Str(author, "nickName") ?? name, name is null ? null : $"https://www.tiktok.com/@{name}",
            J.Date(item, "createTimeISO"), cover is null ? [] : [cover], c, out skipReason,
            ListingText.Join(name, J.Str(author, "signature")));
    }
}

/// <summary>
/// Reads what people write in their posts: phone, email, price, beds, baths, square feet,
/// street address and motivated-seller phrases. Social posts only become leads when the text
/// says a home is for sale; agent marketing is left out because the team buys from owners.
/// </summary>
public static partial class ListingText
{
    private static readonly string[] AgentPhrases =
    [
        "realtor®", "listing agent", "brokerage", "keller williams", "exp realty", "coldwell banker", "re/max", "remax",
        "century 21", "berkshire hathaway", "compass real estate", "call your agent", "buyer's agent", "#realtorlife", "licensed agent"
    ];

    public static string Join(params string?[] parts) => string.Join("\n", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

    private const int MaxPostAgeDays = 120;
    public const double MaxMilesFromCity = 60;

    public static double Miles(double lat1, double lon1, double lat2, double lon2)
    {
        static double Rad(double d) => d * Math.PI / 180;
        var a = Math.Pow(Math.Sin(Rad(lat2 - lat1) / 2), 2) + Math.Cos(Rad(lat1)) * Math.Cos(Rad(lat2)) * Math.Pow(Math.Sin(Rad(lon2 - lon1) / 2), 2);
        return 3958.8 * 2 * Math.Asin(Math.Sqrt(a));
    }

    private static readonly string[] StateNames =
    [
        "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "Florida", "Georgia",
        "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts",
        "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey",
        "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island",
        "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia",
        "Wisconsin", "Wyoming"
    ];

    /// <summary>"House for sale in Texas" found by a Charlotte search: names another state and not ours.</summary>
    private static bool OtherState(string text, SearchContext c)
    {
        if (Regex.IsMatch(text, $@"\b({Regex.Escape(c.StateName)}|{Regex.Escape(c.State)})\b", RegexOptions.IgnoreCase)) return false;
        return StateNames.Any(s => !s.Equals(c.StateName, StringComparison.OrdinalIgnoreCase)
            && Regex.IsMatch(text, $@"\b{Regex.Escape(s)}\b", RegexOptions.IgnoreCase));
    }

    public static bool IsAgentOrBuyer(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        var cleaned = NoAgentsPattern().Replace(text, " ");
        return AgentPhrases.Any(a => cleaned.Contains(a, StringComparison.OrdinalIgnoreCase)) || AgentWords().IsMatch(cleaned)
            || PropertyKeywords.IsExcluded(cleaned);
    }

    public static bool LooksLikeHome(string text) => HomeWords().IsMatch(text);

    public static bool LooksLikeSale(string text) => SaysSale(text) && !LooksLikeRent(text);

    private static bool SaysSale(string text) =>
        SaleWords().IsMatch(text) || PropertyKeywords.Classify(text) is { } k && (k.Direct.Count > 0 || k.Motivated.Count > 0);

    public static bool LooksLikeRent(string text) => RentWords().IsMatch(text);

    public static bool SameState(string? state, SearchContext c) =>
        state is null || state.Equals(c.State, StringComparison.OrdinalIgnoreCase) || state.Equals(c.StateName, StringComparison.OrdinalIgnoreCase);

    public static string Usd(decimal value) => (value < 0 ? "-$" : "$") + Math.Abs(value).ToString("N0", CultureInfo.InvariantCulture);

    /// <summary>
    /// Social searches return news, old posts, other cities and agents advertising themselves.
    /// A post becomes a lead only if it is recent, names the city, stays in the state, offers a home
    /// for sale, and neither the text nor the author's name and bio belong to an agent or a cash buyer.
    /// </summary>
    public static PropertyImportDto? FromPost(string source, string? id, string? url, string? text, string? author, string? authorUrl,
        DateTime? posted, List<string> images, SearchContext c, out string? skipReason, string? authorInfo = null)
    {
        skipReason = null;
        if (string.IsNullOrWhiteSpace(text) || (id is null && url is null)) skipReason = "empty";
        else if (posted is DateTime p && p < DateTime.UtcNow.AddDays(-MaxPostAgeDays)) skipReason = "old post";
        else if (!SaysSale(text) || !HomeWords().IsMatch(text)) skipReason = "not a sale post";
        else if (!Regex.IsMatch(text, $@"\b{Regex.Escape(c.City)}\b", RegexOptions.IgnoreCase)) skipReason = "city not mentioned";
        else if (OtherState(text, c)) skipReason = "other state";
        else if (IsAgentOrBuyer(text) || IsAgentOrBuyer(Join(author, authorInfo))) skipReason = "agent post";
        if (skipReason is not null || text is null) return null;
        var title = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault() ?? text;
        if (title.Length > 140) title = title[..140].TrimEnd() + "…";
        var key = Regex.Replace(source.ToLowerInvariant(), "[^a-z]", "");
        return Build(source, $"{key}-{id ?? url}", url, title, text, text, Price(text), c.City, c, images, posted, author, authorUrl);
    }

    public static PropertyImportDto Build(string source, string? externalId, string? url, string? title, string text, string? rawText,
        decimal? price, string city, SearchContext c, List<string> images, DateTime? posted, string? sellerName, string? sellerProfile) => new(
        ExternalId: externalId is { Length: > 200 } ? externalId[..200] : externalId,
        Source: source,
        SourceUrl: url,
        PostType: source,
        PostedAt: posted,
        Title: string.IsNullOrWhiteSpace(title) ? null : title.Length > 300 ? title[..300] : title,
        Address: AddressPattern().Match(text) is { Success: true } m ? NormalizeAddress(m.Value) : null,
        City: city,
        State: c.State,
        Zip: null,
        Beds: Count(BedsPattern(), text),
        Baths: Count(BathsPattern(), text),
        AskingPrice: price,
        SellerSignal: PropertyKeywords.Classify(text).Signal(),
        RawText: rawText,
        Images: images.Where(i => i.StartsWith("http")).Distinct().Take(40).ToList(),
        ComparablesCount: null, ArvLow: null, ArvHigh: null, ArvEstimate: null,
        RepairsLow: null, RepairsHigh: null, RepairsEstimate: null,
        ClosingHolding: null, Contingency: null, Score: null, Notes: null,
        SellerName: sellerName,
        SellerPhone: Phones(text),
        SellerEmail: Emails(text),
        SellerProfileUrl: sellerProfile,
        MarketValue: null,
        Sqft: SqftPattern().Match(text) is { Success: true } s && int.TryParse(s.Groups[1].Value.Replace(",", ""), out var sq) && sq is > 200 and < 30000 ? sq : null);

    public static string? Phones(string text)
    {
        var phones = PhonePattern().Matches(text)
            .Select(m => $"({m.Groups[1].Value}) {m.Groups[2].Value}-{m.Groups[3].Value}")
            .Distinct().Take(3).ToList();
        return phones.Count == 0 ? null : string.Join(", ", phones);
    }

    public static string? Emails(string text)
    {
        var emails = EmailPattern().Matches(text).Select(m => m.Value.ToLowerInvariant())
            .Where(e => !Regex.IsMatch(e, @"\.(png|jpe?g|gif|webp)$")).Distinct().Take(3).ToList();
        return emails.Count == 0 ? null : string.Join(", ", emails);
    }

    /// <summary>The largest dollar amount that looks like a home price: $85k, $250,000, 1.2M.</summary>
    public static decimal? Price(string text)
    {
        decimal? best = null;
        foreach (Match m in PricePattern().Matches(text))
        {
            if (!decimal.TryParse(m.Groups[1].Value.Replace(",", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out var value)) continue;
            var unit = m.Groups[2].Value.ToLowerInvariant();
            if (unit == "k") value *= 1_000;
            else if (unit is "m" or "mil") value *= 1_000_000;
            if (value is >= 10_000 and <= 10_000_000 && (best is null || value > best)) best = value;
        }
        return best;
    }

    private static decimal? Count(Regex pattern, string text) =>
        pattern.Match(text) is { Success: true } m && decimal.TryParse(m.Groups[1].Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var n) && n is > 0 and < 50 ? n : null;

    [GeneratedRegex(@"\b(for sale|selling|sell(ing)? my|fsbo|by owner|se vende|vendo|asking|listed|price reduced|must sell)\b", RegexOptions.IgnoreCase)]
    private static partial Regex SaleWords();

    [GeneratedRegex(@"\b(realtors?|realty|real estate (agent|team|group|company|co\.?|expert|broker)|broker(age)?|exp_?realty|we buy (houses|homes|land)|sell your (house|home|land)|cash offer for your|reverse mortgage|mortgage (broker|lender)|loan officer|relocation expert|listing agent)\b", RegexOptions.IgnoreCase)]
    private static partial Regex AgentWords();

    [GeneratedRegex(@"\b((no|not working with|without|sin)\s+(an?\s+)?(realtors?|agents?|brokers?|mls)|not (listed )?on (the )?mls)\b", RegexOptions.IgnoreCase)]
    private static partial Regex NoAgentsPattern();

    [GeneratedRegex(@"\b(for rent|for lease|se renta|se alquila)\b", RegexOptions.IgnoreCase)]
    private static partial Regex RentWords();

    [GeneratedRegex(@"\b(house|home|property|casa|bed|beds|bedroom|bath|baths|bd|ba|acre|acres|lot|sq\.?\s?ft|sqft|duplex|townhouse|townhome|condo|mobile home|manufactured|cabin|ranch)\b", RegexOptions.IgnoreCase)]
    private static partial Regex HomeWords();

    [GeneratedRegex(@"(?<!\d)(?:\+?1[\s.-]?)?\(?([2-9]\d{2})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})(?!\d)")]
    private static partial Regex PhonePattern();

    [GeneratedRegex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}", RegexOptions.IgnoreCase)]
    private static partial Regex EmailPattern();

    [GeneratedRegex(@"\$\s?(\d{1,3}(?:,\d{3})+|\d+(?:\.\d+)?)\s?(k|m|mil)?\b", RegexOptions.IgnoreCase)]
    private static partial Regex PricePattern();

    [GeneratedRegex(@"(\d{1,2}(?:\.\d)?)\s*(?:bd|bds|beds?|bedrooms?|br|recámaras?|habitaciones?)\b", RegexOptions.IgnoreCase)]
    private static partial Regex BedsPattern();

    [GeneratedRegex(@"(\d{1,2}(?:\.\d)?)\s*(?:ba|baths?|bathrooms?|baños?)\b", RegexOptions.IgnoreCase)]
    private static partial Regex BathsPattern();

    [GeneratedRegex(@"(\d{1,2},\d{3}|\d{3,5})\s*(?:sq\.?\s?ft|sqft|square feet|sf)\b", RegexOptions.IgnoreCase)]
    private static partial Regex SqftPattern();

    [GeneratedRegex(@"(?<![\d,.$])\b\d{2,6}[ \t]+(?:[NSEW]\.?[ \t]+)?[A-Za-z0-9'-]+(?:[ \t]+[A-Za-z0-9'-]+){0,4}?[ \t]+(?:St|Street|Rd|Road|Ave|Avenue|Dr|Drive|Ln|Lane|Ct|Court|Blvd|Boulevard|Way|Pl|Place|Cir|Circle|Trl|Trail|Pkwy|Parkway|Hwy|Highway|Ter|Terrace|Loop)\b", RegexOptions.IgnoreCase)]
    private static partial Regex AddressPattern();

    private static readonly Dictionary<string, string> Suffixes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Street"] = "St", ["Road"] = "Rd", ["Avenue"] = "Ave", ["Drive"] = "Dr", ["Lane"] = "Ln", ["Court"] = "Ct",
        ["Boulevard"] = "Blvd", ["Place"] = "Pl", ["Circle"] = "Cir", ["Trail"] = "Trl", ["Parkway"] = "Pkwy",
        ["Highway"] = "Hwy", ["Terrace"] = "Ter"
    };

    /// <summary>Same spelling for the same street everywhere, so a house found on two sites is recognized as one.</summary>
    public static string? NormalizeAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address)) return null;
        var words = Regex.Split(address.Trim().TrimEnd('.', ','), @"\s+").ToList();
        var last = words[^1].TrimEnd('.');
        words[^1] = Suffixes.TryGetValue(last, out var shortForm) ? shortForm : last;
        return string.Join(' ', words);
    }
}

/// <summary>Tolerant JSON reading: each actor names and nests fields its own way.</summary>
public static class J
{
    public static JsonElement? Path(JsonElement? element, params string[] names)
    {
        var current = element;
        foreach (var name in names)
        {
            if (current is not { ValueKind: JsonValueKind.Object } obj || !obj.TryGetProperty(name, out var next)) return null;
            current = next;
        }
        return current;
    }

    public static string? Str(JsonElement? element, params string[] names)
    {
        if (element is not { ValueKind: JsonValueKind.Object } obj) return null;
        foreach (var name in names)
        {
            if (!obj.TryGetProperty(name, out var v)) continue;
            if (v.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(v.GetString())) return v.GetString()!.Trim();
            if (v.ValueKind == JsonValueKind.Number) return v.GetRawText();
        }
        return null;
    }

    public static decimal? Num(JsonElement? element, params string[] names)
    {
        if (element is not { ValueKind: JsonValueKind.Object } obj) return null;
        foreach (var name in names)
        {
            if (!obj.TryGetProperty(name, out var v)) continue;
            if (v.ValueKind == JsonValueKind.Number && v.TryGetDecimal(out var n)) return n;
            if (v.ValueKind == JsonValueKind.String && decimal.TryParse(v.GetString()?.Replace("$", "").Replace(",", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out var s)) return s;
        }
        return null;
    }

    public static int? Int(JsonElement? element, params string[] names) => Num(element, names) is decimal d ? (int)Math.Round(d) : null;

    public static bool? Bool(JsonElement? element, string name) =>
        element is { ValueKind: JsonValueKind.Object } obj && obj.TryGetProperty(name, out var v) && v.ValueKind is JsonValueKind.True or JsonValueKind.False
            ? v.GetBoolean()
            : null;

    public static IEnumerable<JsonElement> Arr(JsonElement? element, string name) =>
        element is { ValueKind: JsonValueKind.Object } obj && obj.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Array
            ? v.EnumerateArray().Where(e => e.ValueKind == JsonValueKind.Object)
            : [];

    public static IEnumerable<string> Strings(JsonElement? element, string name) =>
        element is { ValueKind: JsonValueKind.Object } obj && obj.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Array
            ? v.EnumerateArray().Where(e => e.ValueKind == JsonValueKind.String).Select(e => e.GetString()!).Where(s => s.Length > 0)
            : [];

    public static DateTime? Date(JsonElement element, params string[] names)
    {
        foreach (var name in names)
        {
            if (!element.TryGetProperty(name, out var v)) continue;
            if (v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out var epoch) && epoch > 1_000_000_000)
                return epoch > 100_000_000_000 ? DateTimeOffset.FromUnixTimeMilliseconds(epoch).UtcDateTime : DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime;
            if (v.ValueKind != JsonValueKind.String) continue;
            var text = v.GetString();
            if (DateTimeOffset.TryParseExact(text, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var twitter))
                return twitter.UtcDateTime;
            if (DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
                return parsed.UtcDateTime;
        }
        return null;
    }
}

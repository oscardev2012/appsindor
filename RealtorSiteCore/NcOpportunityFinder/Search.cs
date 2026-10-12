using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SafeManagement.Data;

namespace NcOpportunityFinder;

public class ApifySourceOptions
{
    public bool Enabled { get; set; } = true;
    public bool Selected { get; set; } = true;
    public string ActorId { get; set; } = "";
    public decimal CostPer1000Usd { get; set; }

    /// <summary>Optional second actor that opens each kept result for the full listing (Zillow detail page).</summary>
    public string? DetailActorId { get; set; }
}

public class ApifyOptions
{
    public const string SectionName = "Apify";

    public string ApiToken { get; set; } = "";
    public string BaseUrl { get; set; } = "https://api.apify.com/v2/";
    public int MaxWaitMinutes { get; set; } = 40;
    public int DefaultLimitPerSource { get; set; } = 50;

    /// <summary>Upper bound per source; every actor charges per result returned.</summary>
    public int MaxLimitPerSource { get; set; } = 300;

    public Dictionary<string, ApifySourceOptions> Sources { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public record ApifyRun(string Id, string Status, string DatasetId);

public class ApifyClient
{
    private readonly HttpClient _http;
    private readonly ApifyOptions _options;

    public ApifyClient(HttpClient http, IOptions<ApifyOptions> options)
    {
        _http = http;
        _options = options.Value;
        _http.BaseAddress = new Uri(_options.BaseUrl);
        _http.Timeout = TimeSpan.FromMinutes(2);
        if (IsConfigured) _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiToken);
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_options.ApiToken);

    private static readonly JsonSerializerOptions InputJson = new() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };

    public async Task<ApifyRun> StartAsync(string actorId, object input, CancellationToken ct)
    {
        using var content = new StringContent(JsonSerializer.Serialize(input, InputJson), Encoding.UTF8, "application/json");
        using var response = await _http.PostAsync($"acts/{Uri.EscapeDataString(actorId)}/runs", content, ct);
        return await ReadRun(response, ct);
    }

    public async Task<ApifyRun> GetRunAsync(string runId, CancellationToken ct)
    {
        using var response = await _http.GetAsync($"actor-runs/{Uri.EscapeDataString(runId)}", ct);
        return await ReadRun(response, ct);
    }

    public async Task<List<JsonElement>> GetItemsAsync(string datasetId, CancellationToken ct)
    {
        using var response = await _http.GetAsync($"datasets/{Uri.EscapeDataString(datasetId)}/items?clean=true&format=json", ct);
        var raw = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode) throw new InvalidOperationException(Describe(raw, (int)response.StatusCode));
        using var doc = JsonDocument.Parse(raw);
        return doc.RootElement.ValueKind == JsonValueKind.Array
            ? doc.RootElement.EnumerateArray().Where(e => e.ValueKind == JsonValueKind.Object && !e.TryGetProperty("error", out _)).Select(e => e.Clone()).ToList()
            : [];
    }

    private static async Task<ApifyRun> ReadRun(HttpResponseMessage response, CancellationToken ct)
    {
        var raw = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode) throw new InvalidOperationException(Describe(raw, (int)response.StatusCode));
        using var doc = JsonDocument.Parse(raw);
        var data = doc.RootElement.GetProperty("data");
        return new ApifyRun(
            data.GetProperty("id").GetString()!,
            data.GetProperty("status").GetString()!,
            data.TryGetProperty("defaultDatasetId", out var ds) ? ds.GetString() ?? "" : "");
    }

    private static string Describe(string raw, int status)
    {
        try
        {
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.TryGetProperty("error", out var error) && error.TryGetProperty("message", out var message))
                return status is 401 or 403 ? $"Apify rejected the token: {message.GetString()}" : $"Apify ({status}): {message.GetString()}";
        }
        catch (JsonException)
        {
        }
        return status == 402 ? "Apify has no credit left on the plan." : $"Apify returned HTTP {status}.";
    }
}

public record UsPlace(string Name, double Latitude, double Longitude, bool Cdp);

/// <summary>US states and their cities from the Census Bureau gazetteer (Data/us-places.json).</summary>
public class GeoCatalog
{
    private readonly Lazy<Dictionary<string, (string Name, List<UsPlace> Places)>> _states;

    public GeoCatalog(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "us-places.json");
        _states = new(() => Load(path));
    }

    public IEnumerable<object> States() =>
        _states.Value.OrderBy(s => s.Value.Name).Select(s => new { code = s.Key, name = s.Value.Name, cities = s.Value.Places.Count });

    public List<UsPlace>? Places(string state) => _states.Value.TryGetValue(state, out var s) ? s.Places : null;

    public string? StateName(string state) => _states.Value.TryGetValue(state, out var s) ? s.Name : null;

    public UsPlace? Find(string state, string city) =>
        Places(state)?.FirstOrDefault(p => p.Name.Equals(city.Trim(), StringComparison.OrdinalIgnoreCase));

    private static Dictionary<string, (string, List<UsPlace>)> Load(string path)
    {
        using var stream = File.OpenRead(path);
        using var doc = JsonDocument.Parse(stream);
        var result = new Dictionary<string, (string, List<UsPlace>)>(StringComparer.OrdinalIgnoreCase);
        foreach (var state in doc.RootElement.EnumerateObject())
        {
            var places = state.Value.GetProperty("places").EnumerateArray()
                .Select(p => new UsPlace(p[0].GetString()!, p[1].GetDouble(), p[2].GetDouble(), p[3].GetInt32() == 1))
                .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
            result[state.Name] = (state.Value.GetProperty("name").GetString()!, places);
        }
        return result;
    }
}

public record SearchRequest(
    string? State, string? City, List<string>? Sources, int? LimitPerSource,
    string? MarketplaceSlug, string? CraigslistSite, List<string>? FacebookGroups, string? Keywords = null);

public class SourceProgress
{
    public string Label { get; set; } = "";
    public string Status { get; set; } = "Pending";
    public int Found { get; set; }
    public int Imported { get; set; }
    public int Updated { get; set; }
    public int Skipped { get; set; }
    public string? RunId { get; set; }
    public string? Error { get; set; }
    public Dictionary<string, int> Reasons { get; set; } = [];
}

public record SearchSummary(
    int Id, string StartedAt, string? FinishedAt, string Status, string State, string City,
    Dictionary<string, SourceProgress> Sources, int Found, int Imported, int Updated, int Skipped, string? Error, List<string> Keywords);

public class SearchConflictException(string message) : Exception(message);

/// <summary>
/// Runs every selected Apify actor for one city at the same time, saves each result as a
/// property (the same house seen in two places is merged) and records progress per source.
/// One search at a time keeps the Apify bill predictable.
/// </summary>
public class ListingSearchService
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private readonly SafeSql _sql;
    private readonly OpportunityStore _store;
    private readonly ApifyClient _apify;
    private readonly ApifyOptions _options;
    private readonly ILogger<ListingSearchService> _log;
    private readonly SemaphoreSlim _importLock = new(1, 1);
    private int _running;

    public ListingSearchService(SafeSql sql, OpportunityStore store, ApifyClient apify, IOptions<ApifyOptions> options, ILogger<ListingSearchService> log)
    {
        _sql = sql;
        _store = store;
        _apify = apify;
        _options = options.Value;
        _log = log;
    }

    public bool IsConfigured => _apify.IsConfigured;
    public ApifyOptions Options => _options;

    public IEnumerable<(ListingSource Source, ApifySourceOptions Options)> Available() =>
        ListingSource.All
            .Select(s => (s, _options.Sources.TryGetValue(s.Key, out var o) ? o : null))
            .Where(x => x.Item2 is { Enabled: true } && x.Item2.ActorId.Length > 0)
            .Select(x => (x.s, x.Item2!));

    public List<SearchSummary> Recent()
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT TOP 10 Id, StartedAt, FinishedAt, Status, State, City, SourceResults, Found, Imported, Updated, Skipped, Error, Options
            FROM dbo.NcOppSearches ORDER BY Id DESC
            """);
        using var r = cmd.ExecuteReader();
        var list = new List<SearchSummary>();
        while (r.Read())
        {
            list.Add(new SearchSummary(r.GetInt32(0), r.Iso(1), r.IsoOrNull(2), r.GetString(3), r.GetString(4), r.GetString(5),
                JsonSerializer.Deserialize<Dictionary<string, SourceProgress>>(r.GetString(6), Json) ?? [],
                r.GetInt32(7), r.GetInt32(8), r.GetInt32(9), r.GetInt32(10), r.IsDBNull(11) ? null : r.GetString(11),
                r.IsDBNull(12) ? [] : SavedKeywords(r.GetString(12))));
        }
        return list;
    }

    private static List<string> SavedKeywords(string options)
    {
        try
        {
            using var doc = JsonDocument.Parse(options);
            return [.. J.Strings(doc.RootElement, "keywords").Concat(J.Strings(doc.RootElement, "Keywords"))];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    public int Start(SearchContext context, List<ListingSource> sources)
    {
        if (Interlocked.CompareExchange(ref _running, 1, 0) != 0)
            throw new SearchConflictException("A search is already running. Wait for it to finish.");

        try
        {
            var progress = sources.ToDictionary(s => s.Key, s => new SourceProgress { Label = s.Label });
            int id;
            using (var db = _sql.Open())
            {
                using (var stale = db.Command("""
                    UPDATE dbo.NcOppSearches SET Status = N'Failed', FinishedAt = SYSUTCDATETIME(),
                        Error = N'The site restarted before this search finished.'
                    WHERE Status = N'Running'
                    """))
                    stale.ExecuteNonQuery();

                using var insert = db.Command("""
                    INSERT INTO dbo.NcOppSearches (State, City, Options, SourceResults)
                    OUTPUT INSERTED.Id VALUES (@state, @city, @options, @results)
                    """)
                    .With("@state", context.State)
                    .With("@city", context.City)
                    .With("@options", JsonSerializer.Serialize(context, Json))
                    .With("@results", JsonSerializer.Serialize(progress, Json));
                id = (int)insert.ExecuteScalar()!;
            }

            _ = Task.Run(() => RunAsync(id, context, sources, progress));
            return id;
        }
        catch
        {
            Interlocked.Exchange(ref _running, 0);
            throw;
        }
    }

    private async Task RunAsync(int id, SearchContext context, List<ListingSource> sources, Dictionary<string, SourceProgress> progress)
    {
        try
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(_options.MaxWaitMinutes));
            await Task.WhenAll(sources.Select(s => RunSourceAsync(id, s, context, progress, timeout.Token)));

            var states = progress.Values.Select(p => p.Status).ToList();
            var status = states.All(s => s == "Succeeded") ? "Succeeded" : states.Any(s => s == "Succeeded") ? "Partial" : "Failed";
            Save(id, progress, status, null);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Listing search {SearchId} failed", id);
            try { Save(id, progress, "Failed", ex.Message); }
            catch (Exception saveEx) { _log.LogError(saveEx, "Could not record failure of search {SearchId}", id); }
        }
        finally
        {
            Interlocked.Exchange(ref _running, 0);
        }
    }

    private async Task RunSourceAsync(int searchId, ListingSource source, SearchContext context, Dictionary<string, SourceProgress> progress, CancellationToken ct)
    {
        var p = progress[source.Key];
        try
        {
            var options = _options.Sources[source.Key];
            p.Status = "Running";
            var runs = new List<ApifyRun>();
            foreach (var input in source.BuildInputs(context))
                runs.Add(await _apify.StartAsync(options.ActorId, input, ct));
            p.RunId = string.Join(",", runs.Select(r => r.Id));
            Save(searchId, progress, "Running", null);
            runs = [.. await Task.WhenAll(runs.Select(r => WaitAsync(r, ct)))];

            // A run that times out or is aborted still keeps what it collected; use it.
            var items = new List<JsonElement>();
            foreach (var run in runs.Where(r => r.DatasetId.Length > 0))
                items.AddRange(await _apify.GetItemsAsync(run.DatasetId, ct));
            p.Found = items.Count;
            if (!string.IsNullOrWhiteSpace(options.DetailActorId))
                items = await AddDetailsAsync(source, options.DetailActorId, context, items, p, ct);
            await ImportAsync(source, context, items, p);

            var unfinished = runs.Where(r => r.Status != "SUCCEEDED").Select(r => r.Status).Distinct().ToList();
            if (unfinished.Count < runs.Count || p.Found > 0)
            {
                p.Status = "Succeeded";
                if (unfinished.Count > 0) p.Error = $"Apify run ended {string.Join(", ", unfinished)}; saved what it found.";
                p.Error ??= NothingUsable(source, context, p);
            }
            else
            {
                p.Status = "Failed";
                p.Error = $"Apify run ended {string.Join(", ", unfinished)}.";
            }
        }
        catch (OperationCanceledException)
        {
            p.Status = "Failed";
            p.Error = $"Took longer than {_options.MaxWaitMinutes} minutes.";
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Source {Source} failed in search {SearchId}", source.Key, searchId);
            p.Status = "Failed";
            p.Error = ex.Message.Length > 300 ? ex.Message[..300] : ex.Message;
        }
        Save(searchId, progress, "Running", null);
    }

    private static string? NothingUsable(ListingSource source, SearchContext c, SourceProgress p)
    {
        if (p.Imported + p.Updated > 0) return null;
        const string prefix = "source error: ";
        if (p.Reasons.Keys.FirstOrDefault(k => k.StartsWith(prefix)) is string error)
            return $"{source.Label} answered with an error: {error[prefix.Length..]}";
        if (p.Found > 0) return null;
        return source is MarketplaceSource
            ? $"Facebook returned no listings for city code \"{c.MarketplaceSlug}\". Open facebook.com/marketplace, pick the city and copy the code from the address bar."
            : "The source returned no results for this city.";
    }

    private async Task<ApifyRun> WaitAsync(ApifyRun run, CancellationToken ct)
    {
        while (run.Status is "READY" or "RUNNING" or "TIMING-OUT" or "ABORTING")
        {
            await Task.Delay(TimeSpan.FromSeconds(10), ct);
            run = await _apify.GetRunAsync(run.Id, ct);
        }
        return run;
    }

    /// <summary>
    /// Opens only the results that will be imported, so the detail actor is not paid for skipped ones.
    /// If it fails, the search results are imported as they came.
    /// </summary>
    private async Task<List<JsonElement>> AddDetailsAsync(ListingSource source, string detailActor, SearchContext context,
        List<JsonElement> items, SourceProgress p, CancellationToken ct)
    {
        var kept = items.Where(i => source.Map(i, context, out _) is not null).ToList();
        if (kept.Count == 0 || source.BuildDetailInput(kept) is not { } input) return items;
        try
        {
            var run = await WaitAsync(await _apify.StartAsync(detailActor, input, ct), ct);
            var details = run.DatasetId.Length == 0 ? [] : await _apify.GetItemsAsync(run.DatasetId, ct);
            return source.MergeDetails(items, details);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _log.LogWarning(ex, "Detail actor {Actor} failed for {Source}", detailActor, source.Key);
            p.Error = "Full listing details could not be loaded; saved the search results.";
            return items;
        }
    }

    /// <summary>Imports one batch of actor results; also used by tests with saved sample data.</summary>
    public async Task ImportAsync(ListingSource source, SearchContext context, IEnumerable<JsonElement> items, SourceProgress p)
    {
        await _importLock.WaitAsync();
        try
        {
            foreach (var item in items)
            {
                var actorError = item.ValueKind == JsonValueKind.Object ? J.Str(item, "error", "errorDescription") : null;
                string? reason = null;
                var dto = actorError is null ? source.Map(item, context, out reason) : null;
                if (dto is null)
                {
                    p.Skipped++;
                    var key = actorError is null ? reason ?? "not usable" : "source error: " + (actorError.Length > 160 ? actorError[..160] : actorError);
                    if (p.Reasons.ContainsKey(key) || p.Reasons.Count < 12) p.Reasons[key] = p.Reasons.GetValueOrDefault(key) + 1;
                    continue;
                }
                var result = _store.Import(dto);
                if (result.Duplicate) p.Updated++;
                else p.Imported++;
            }
        }
        finally
        {
            _importLock.Release();
        }
    }

    private readonly object _saveLock = new();

    private void Save(int id, Dictionary<string, SourceProgress> progress, string status, string? error)
    {
        lock (_saveLock)
        {
            using var db = _sql.Open();
            using var cmd = db.Command("""
                UPDATE dbo.NcOppSearches SET
                    Status = @status, SourceResults = @results, Found = @found, Imported = @imported,
                    Updated = @updated, Skipped = @skipped, Error = COALESCE(@error, Error),
                    FinishedAt = CASE WHEN @status = N'Running' THEN NULL ELSE SYSUTCDATETIME() END
                WHERE Id = @id
                """)
                .With("@id", id)
                .With("@status", status)
                .With("@results", JsonSerializer.Serialize(progress, Json))
                .With("@found", progress.Values.Sum(v => v.Found))
                .With("@imported", progress.Values.Sum(v => v.Imported))
                .With("@updated", progress.Values.Sum(v => v.Updated))
                .With("@skipped", progress.Values.Sum(v => v.Skipped))
                .With("@error", error);
            cmd.ExecuteNonQuery();
        }
    }
}

[ApiController]
[Route("api/opportunity")]
public partial class ListingSearchController : ControllerBase
{
    private readonly ListingSearchService _search;
    private readonly GeoCatalog _geo;

    public ListingSearchController(ListingSearchService search, GeoCatalog geo)
    {
        _search = search;
        _geo = geo;
    }

    [HttpGet("geo/states")]
    public IActionResult States() => Ok(_geo.States());

    [HttpGet("geo/cities")]
    public IActionResult Cities([FromQuery] string state)
    {
        var places = _geo.Places(state ?? "");
        return places is null
            ? NotFound(new { error = "Unknown state.", errorEs = "Estado desconocido." })
            : Ok(places.Select(p => new { name = p.Name, cdp = p.Cdp }));
    }

    [HttpGet("searches")]
    public IActionResult Searches()
    {
        List<SearchSummary> recent;
        string? dbError = null;
        try
        {
            recent = _search.Recent();
        }
        catch (SqlException)
        {
            recent = [];
            dbError = "IndorDB is not available. Searches cannot be saved until it is.";
        }
        return Ok(BuildConfig(recent, dbError));
    }

    private object BuildConfig(List<SearchSummary> recent, string? dbError) => new
    {
        configured = _search.IsConfigured,
        dbError,
        defaultLimit = _search.Options.DefaultLimitPerSource,
        maxLimit = _search.Options.MaxLimitPerSource,
        sources = _search.Available().Select(s => new
        {
            key = s.Source.Key,
            label = s.Source.Label,
            costPer1000 = s.Options.CostPer1000Usd,
            selected = s.Options.Selected,
            needsGroups = s.Source.NeedsFacebookGroups
        }),
        searches = recent
    };

    [HttpPost("searches")]
    public IActionResult Start([FromBody] SearchRequest request)
    {
        if (!_search.IsConfigured)
            return BadRequest(new
            {
                error = "Apify token is missing. Set Apify:ApiToken in the site settings.",
                errorEs = "Falta el token de Apify. Configura Apify:ApiToken en los ajustes del sitio."
            });

        var state = (request.State ?? "").Trim().ToUpperInvariant();
        var stateName = _geo.StateName(state);
        var place = stateName is null ? null : _geo.Find(state, request.City ?? "");
        if (place is null)
            return BadRequest(new { error = "Pick a state and a city from the list.", errorEs = "Elige un estado y una ciudad de la lista." });

        var available = _search.Available().ToDictionary(s => s.Source.Key, s => s.Source, StringComparer.OrdinalIgnoreCase);
        var sources = (request.Sources ?? []).Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(k => available.GetValueOrDefault(k)).OfType<ListingSource>().ToList();
        if (sources.Count == 0)
            return BadRequest(new { error = "Pick at least one source.", errorEs = "Elige al menos una fuente." });

        var limit = request.LimitPerSource ?? _search.Options.DefaultLimitPerSource;
        if (limit < 1 || limit > _search.Options.MaxLimitPerSource)
            return BadRequest(new
            {
                error = $"Results per source must be between 1 and {_search.Options.MaxLimitPerSource}.",
                errorEs = $"Los resultados por fuente deben estar entre 1 y {_search.Options.MaxLimitPerSource}."
            });

        var slug = (request.MarketplaceSlug ?? "").Trim().ToLowerInvariant();
        if (sources.Any(s => s is MarketplaceSource) && !SlugPattern().IsMatch(slug))
            return BadRequest(new { error = "Marketplace city code may only have letters and numbers.", errorEs = "El código de ciudad de Marketplace solo admite letras y números." });

        var site = (request.CraigslistSite ?? "").Trim().ToLowerInvariant();
        if (sources.Any(s => s is CraigslistSource) && !SlugPattern().IsMatch(site))
            return BadRequest(new { error = "Craigslist area may only have letters and numbers (e.g. charlotte).", errorEs = "La zona de Craigslist solo admite letras y números (ej. charlotte)." });

        var groups = (request.FacebookGroups ?? []).Select(g => g.Trim()).Where(g => g.Length > 0).Distinct().ToList();
        if (groups.Any(g => !Uri.TryCreate(g, UriKind.Absolute, out var u) || u.Scheme != Uri.UriSchemeHttps
                || !(u.Host is "facebook.com" or "www.facebook.com" or "m.facebook.com") || !u.AbsolutePath.StartsWith("/groups/")))
            return BadRequest(new { error = "Facebook groups must be https://www.facebook.com/groups/... links.", errorEs = "Los grupos deben ser enlaces https://www.facebook.com/groups/..." });
        if (groups.Count > 20)
            return BadRequest(new { error = "Use at most 20 Facebook groups.", errorEs = "Usa como máximo 20 grupos de Facebook." });
        if (groups.Count == 0) sources.RemoveAll(s => s.NeedsFacebookGroups);
        if (sources.Count == 0)
            return BadRequest(new { error = "Add at least one Facebook group link.", errorEs = "Agrega al menos un enlace de grupo de Facebook." });

        var keywords = (request.Keywords ?? "").Split(['\n', ',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(k => Regex.Replace(k, "[\"<>]", "").Trim()).Where(k => k.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        if (keywords.Count > 10 || keywords.Any(k => k.Length > 80))
            return BadRequest(new { error = "Use at most 10 keywords of up to 80 characters.", errorEs = "Usa como máximo 10 palabras clave de hasta 80 caracteres." });

        var context = new SearchContext(state, stateName!, place.Name, place.Latitude, place.Longitude, slug, site, groups, limit, keywords);
        try
        {
            var id = _search.Start(context, sources);
            return Accepted(new { id });
        }
        catch (SearchConflictException ex)
        {
            return Conflict(new { error = ex.Message, errorEs = "Ya hay una búsqueda en curso. Espera a que termine." });
        }
    }

    [GeneratedRegex("^[a-z0-9]{2,40}$")]
    private static partial Regex SlugPattern();
}

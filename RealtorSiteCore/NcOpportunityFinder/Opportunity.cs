using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using SafeManagement.Data;

namespace NcOpportunityFinder;

public record PropertyImportDto(
    string? ExternalId, string? Source, string? SourceUrl, string? PostType, DateTime? PostedAt,
    string? Title, string? Address, string? City, string? State, string? Zip,
    decimal? Beds, decimal? Baths, decimal? AskingPrice, string? SellerSignal, string? RawText, List<string>? Images,
    int? ComparablesCount, decimal? ArvLow, decimal? ArvHigh, decimal? ArvEstimate,
    decimal? RepairsLow, decimal? RepairsHigh, decimal? RepairsEstimate,
    decimal? ClosingHolding, decimal? Contingency, int? Score, string? Notes,
    string? SellerName = null, string? SellerPhone = null, string? SellerEmail = null, string? SellerProfileUrl = null,
    decimal? MarketValue = null, int? Sqft = null, decimal? Latitude = null, decimal? Longitude = null);

public record OfferNumbersDto(decimal? Arv, decimal? Repairs, decimal? ClosingHolding, decimal? Contingency);

public record ImportResult(int Id, string PublicId, bool Duplicate, string Stage, List<string> Missing);

public sealed class OpportunityProperty
{
    public int Id { get; init; }
    public string PublicId { get; init; } = "";
    public string ExternalId { get; init; } = "";
    public string Source { get; init; } = "";
    public string SourceUrl { get; init; } = "";
    public string PostType { get; init; } = "";
    public string? PostedAt { get; init; }
    public string ImportedAt { get; init; } = "";
    public string UpdatedAt { get; init; } = "";
    public string Title { get; init; } = "";
    public string Address { get; init; } = "";
    public string City { get; init; } = "";
    public string State { get; init; } = "";
    public string Zip { get; init; } = "";
    public double? Beds { get; init; }
    public double? Baths { get; init; }
    public double? Asking { get; init; }
    public string SellerSignal { get; init; } = "";
    public string? Priority { get; init; }
    public string SellerName { get; init; } = "";
    public string SellerPhone { get; init; } = "";
    public string SellerEmail { get; init; } = "";
    public string SellerProfileUrl { get; init; } = "";
    public double? MarketValue { get; init; }
    public double? Discount { get; init; }
    public int? Sqft { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string Sources { get; init; } = "";
    public string PostText { get; init; } = "";
    public bool ScoreIsAuto { get; init; }
    public List<string> ScoreReasons { get; init; } = [];
    public List<string> Images { get; init; } = [];
    public int? Comparables { get; init; }
    public double? ArvLow { get; init; }
    public double? ArvHigh { get; init; }
    public double? Arv { get; init; }
    public double? RepairsLow { get; init; }
    public double? RepairsHigh { get; init; }
    public double? Repairs { get; init; }
    public double? ClosingHolding { get; init; }
    public double? Contingency { get; init; }
    public int? Score { get; init; }
    public string Notes { get; init; } = "";
    public string Status { get; init; } = "";
    public string? ReviewRequestedAt { get; init; }
    public List<string> Missing { get; init; } = [];
    public string Stage { get; init; } = "";
    public double? MaxOffer { get; init; }
    public string? Fit { get; init; }
}

public class OpportunityStore
{
    public const double BuyBoxRate = 0.65;
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private readonly SafeSql _sql;
    public OpportunityStore(SafeSql sql) => _sql = sql;

    private const string Columns = """
        Id, PublicId, ExternalId, Source, SourceUrl, PostType, PostedAt, ImportedAt, UpdatedAt,
        Title, Address, City, State, Zip, Beds, Baths, AskingPrice, SellerSignal, Images,
        ComparablesCount, ArvLow, ArvHigh, ArvEstimate, RepairsLow, RepairsHigh, RepairsEstimate,
        ClosingHolding, Contingency, Score, Notes, Status, ReviewRequestedAt, SellerName, SellerPhone,
        SellerEmail, SellerProfileUrl, MarketValue, Sqft, Latitude, Longitude, Sources, RawText
        """;

    public List<OpportunityProperty> List()
    {
        using var db = _sql.Open();
        using var cmd = db.Command($"""
            SELECT TOP 3000 {Columns} FROM dbo.NcOppProperties
            WHERE Status <> N'Discarded' ORDER BY ImportedAt DESC, Id DESC
            """);
        var rows = new List<OpportunityProperty>();
        using var r = cmd.ExecuteReader();
        while (r.Read()) rows.Add(Map(r));
        return rows;
    }

    public OpportunityProperty? Get(int id)
    {
        using var db = _sql.Open();
        return Get(db, null, id);
    }

    private static OpportunityProperty? Get(SqlConnection db, SqlTransaction? tx, int id)
    {
        using var cmd = db.Command($"SELECT {Columns} FROM dbo.NcOppProperties WHERE Id = @id", tx).With("@id", id);
        using var r = cmd.ExecuteReader();
        return r.Read() ? Map(r) : null;
    }

    public List<object> Events(int id)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT TOP 50 Kind, Detail, CreatedAt FROM dbo.NcOppPropertyEvents
            WHERE PropertyId = @id ORDER BY CreatedAt DESC, Id DESC
            """).With("@id", id);
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new { kind = r.GetString(0), detail = JsonSerializer.Deserialize<JsonElement>(r.GetString(1)), createdAt = r.Iso(2) });
        return rows;
    }

    public ImportResult Import(PropertyImportDto d)
    {
        var images = (d.Images ?? []).Select(i => i.Trim()).Where(i => i.Length > 0).Distinct().ToList();
        using var db = _sql.Open();
        using var tx = db.BeginTransaction();

        using var find = db.Command("""
            SELECT TOP 1 Id FROM dbo.NcOppProperties
            WHERE (@ext IS NOT NULL AND ExternalId = @ext)
               OR (@url IS NOT NULL AND SourceUrl = @url)
               OR (@address IS NOT NULL AND Address = @address AND ISNULL(City, N'') = ISNULL(@city, N''))
            ORDER BY Id
            """, tx)
            .With("@ext", Clean(d.ExternalId))
            .With("@url", Clean(d.SourceUrl))
            .With("@address", Clean(d.Address))
            .With("@city", Clean(d.City));
        var existing = find.ExecuteScalar() as int?;

        int id;
        if (existing is int found)
        {
            using var update = Bind(db.Command("""
                UPDATE dbo.NcOppProperties SET
                    ExternalId = COALESCE(ExternalId, @ext),
                    SourceUrl = COALESCE(SourceUrl, @url), PostType = COALESCE(PostType, @postType),
                    PostedAt = COALESCE(@postedAt, PostedAt), Title = COALESCE(@title, Title),
                    Address = COALESCE(@address, Address), City = COALESCE(@city, City),
                    State = COALESCE(@state, State), Zip = COALESCE(@zip, Zip),
                    Beds = COALESCE(@beds, Beds), Baths = COALESCE(@baths, Baths),
                    AskingPrice = COALESCE(@asking, AskingPrice), SellerSignal = COALESCE(@signal, SellerSignal),
                    RawText = COALESCE(@raw, RawText), Images = COALESCE(@images, Images),
                    ComparablesCount = COALESCE(@comps, ComparablesCount),
                    ArvLow = COALESCE(@arvLow, ArvLow), ArvHigh = COALESCE(@arvHigh, ArvHigh), ArvEstimate = COALESCE(@arv, ArvEstimate),
                    RepairsLow = COALESCE(@repLow, RepairsLow), RepairsHigh = COALESCE(@repHigh, RepairsHigh), RepairsEstimate = COALESCE(@rep, RepairsEstimate),
                    ClosingHolding = COALESCE(@closing, ClosingHolding), Contingency = COALESCE(@contingency, Contingency),
                    Score = COALESCE(@score, Score), Notes = COALESCE(@notes, Notes),
                    SellerName = COALESCE(SellerName, @sellerName),
                    SellerPhone = CASE WHEN @sellerPhone IS NULL OR CHARINDEX(@sellerPhone, ISNULL(SellerPhone, N'')) > 0 THEN SellerPhone
                                       ELSE LEFT(CONCAT_WS(N', ', NULLIF(SellerPhone, N''), @sellerPhone), 100) END,
                    SellerEmail = CASE WHEN @sellerEmail IS NULL OR CHARINDEX(@sellerEmail, ISNULL(SellerEmail, N'')) > 0 THEN SellerEmail
                                       ELSE LEFT(CONCAT_WS(N', ', NULLIF(SellerEmail, N''), @sellerEmail), 300) END,
                    SellerProfileUrl = COALESCE(SellerProfileUrl, @sellerProfile),
                    MarketValue = COALESCE(@marketValue, MarketValue), Sqft = COALESCE(@sqft, Sqft),
                    Latitude = COALESCE(Latitude, @lat), Longitude = COALESCE(Longitude, @lon),
                    Sources = CASE WHEN @sourceName IS NULL OR CHARINDEX(@sourceName, ISNULL(Sources, N'')) > 0 THEN Sources
                                   ELSE LEFT(CONCAT_WS(N' · ', NULLIF(Sources, N''), @sourceName), 400) END,
                    UpdatedAt = SYSUTCDATETIME()
                WHERE Id = @id
                """, tx), d, Clean(d.Title), images).With("@id", found);
            update.ExecuteNonQuery();
            id = found;
        }
        else
        {
            var title = Clean(d.Title) ?? Clean(d.Address) ?? (Clean(d.City) is string city ? $"Area near {city}" : "Untitled property post");
            using var insert = Bind(db.Command("""
                INSERT INTO dbo.NcOppProperties (ExternalId, Source, SourceUrl, PostType, PostedAt, Title, Address, City, State, Zip,
                    Beds, Baths, AskingPrice, SellerSignal, RawText, Images, ComparablesCount, ArvLow, ArvHigh, ArvEstimate,
                    RepairsLow, RepairsHigh, RepairsEstimate, ClosingHolding, Contingency, Score, Notes, SellerName, SellerPhone,
                    SellerEmail, SellerProfileUrl, MarketValue, Sqft, Latitude, Longitude, Sources)
                OUTPUT INSERTED.Id
                VALUES (@ext, COALESCE(@source, N'Facebook'), @url, @postType, @postedAt, @title, @address, @city, COALESCE(@state, N'NC'), @zip,
                    @beds, @baths, @asking, @signal, @raw, COALESCE(@images, N'[]'), @comps, @arvLow, @arvHigh, @arv,
                    @repLow, @repHigh, @rep, @closing, @contingency, @score, @notes, @sellerName, @sellerPhone,
                    @sellerEmail, @sellerProfile, @marketValue, @sqft, @lat, @lon, @sourceName)
                """, tx), d, title, images);
            id = (int)insert.ExecuteScalar()!;
        }

        AddEvent(db, tx, id, existing is null ? "imported" : "updated", new { source = Clean(d.Source) ?? "Facebook", sourceUrl = Clean(d.SourceUrl) });
        var saved = Get(db, tx, id)!;
        tx.Commit();
        return new ImportResult(saved.Id, saved.PublicId, existing is not null, saved.Stage, saved.Missing);
    }

    public OpportunityProperty? SaveNumbers(int id, OfferNumbersDto n)
    {
        using var db = _sql.Open();
        using var tx = db.BeginTransaction();
        using var cmd = db.Command("""
            UPDATE dbo.NcOppProperties SET ArvEstimate = @arv, RepairsEstimate = @repairs,
                ClosingHolding = @closing, Contingency = @contingency, UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @id AND Status <> N'Discarded'
            """, tx)
            .With("@arv", n.Arv).With("@repairs", n.Repairs)
            .With("@closing", n.ClosingHolding).With("@contingency", n.Contingency).With("@id", id);
        if (cmd.ExecuteNonQuery() == 0) return null;
        var saved = Get(db, tx, id)!;
        AddEvent(db, tx, id, "numbers", new { arv = n.Arv, repairs = n.Repairs, closingHolding = n.ClosingHolding, contingency = n.Contingency, maxOffer = saved.MaxOffer });
        tx.Commit();
        return saved;
    }

    public OpportunityProperty? MarkForReview(int id)
    {
        using var db = _sql.Open();
        using var tx = db.BeginTransaction();
        using var cmd = db.Command("""
            UPDATE dbo.NcOppProperties SET Status = N'HumanReview', ReviewRequestedAt = SYSUTCDATETIME(), UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @id AND Status = N'Active'
            """, tx).With("@id", id);
        if (cmd.ExecuteNonQuery() > 0) AddEvent(db, tx, id, "review", new { });
        var saved = Get(db, tx, id);
        tx.Commit();
        return saved;
    }

    private static void AddEvent(SqlConnection db, SqlTransaction tx, int id, string kind, object detail)
    {
        using var cmd = db.Command("INSERT INTO dbo.NcOppPropertyEvents (PropertyId, Kind, Detail) VALUES (@id, @kind, @detail)", tx)
            .With("@id", id).With("@kind", kind).With("@detail", JsonSerializer.Serialize(detail, Json));
        cmd.ExecuteNonQuery();
    }

    private static SqlCommand Bind(SqlCommand cmd, PropertyImportDto d, string? title, List<string> images) => cmd
        .With("@ext", Clean(d.ExternalId))
        .With("@source", Clean(d.Source))
        .With("@url", Clean(d.SourceUrl))
        .With("@postType", Clean(d.PostType))
        .With("@postedAt", d.PostedAt?.ToUniversalTime())
        .With("@title", title)
        .With("@address", Clean(d.Address))
        .With("@city", Clean(d.City))
        .With("@state", Clean(d.State)?.ToUpperInvariant())
        .With("@zip", Clean(d.Zip))
        .With("@beds", d.Beds)
        .With("@baths", d.Baths)
        .With("@asking", d.AskingPrice)
        .With("@signal", Clean(d.SellerSignal))
        .With("@raw", Clean(d.RawText))
        .With("@images", images.Count > 0 ? JsonSerializer.Serialize(images, Json) : null)
        .With("@comps", d.ComparablesCount)
        .With("@arvLow", d.ArvLow)
        .With("@arvHigh", d.ArvHigh)
        .With("@arv", d.ArvEstimate ?? Mid(d.ArvLow, d.ArvHigh))
        .With("@repLow", d.RepairsLow)
        .With("@repHigh", d.RepairsHigh)
        .With("@rep", d.RepairsEstimate ?? Mid(d.RepairsLow, d.RepairsHigh))
        .With("@closing", d.ClosingHolding)
        .With("@contingency", d.Contingency)
        .With("@score", d.Score)
        .With("@notes", Clean(d.Notes))
        .With("@sellerName", Clean(d.SellerName))
        .With("@sellerPhone", Clean(d.SellerPhone))
        .With("@sellerEmail", Clean(d.SellerEmail)?.ToLowerInvariant())
        .With("@sellerProfile", Clean(d.SellerProfileUrl))
        .With("@marketValue", d.MarketValue)
        .With("@sqft", d.Sqft)
        .With("@lat", d.Latitude)
        .With("@lon", d.Longitude)
        .With("@sourceName", Clean(d.Source) ?? "Facebook");

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal? Mid(decimal? low, decimal? high) => low is not null && high is not null ? Math.Round((low.Value + high.Value) / 2) : null;

    private static int? IntOrNull(SqlDataReader r, int i) => r.IsDBNull(i) ? null : r.GetInt32(i);

    private static OpportunityProperty Map(SqlDataReader r)
    {
        var address = r.Text(10);
        var asking = r.NumberOrNull(16);
        var images = JsonSerializer.Deserialize<List<string>>(r.GetString(18), Json) ?? [];
        var arv = r.NumberOrNull(22);
        var repairs = r.NumberOrNull(25);
        var closing = r.NumberOrNull(26);
        var contingency = r.NumberOrNull(27);

        var missing = new List<string>();
        if (address.Length == 0) missing.Add("address");
        if (asking is null) missing.Add("price");
        if (images.Count == 0) missing.Add("photos");

        double? maxOffer = arv is null || repairs is null
            ? null
            : Math.Max(0, Math.Round(arv.Value * BuyBoxRate - repairs.Value - (closing ?? 0) - (contingency ?? 0)));

        var signal = r.Text(17);
        var phone = r.Text(33);
        var email = r.Text(34);
        var marketValue = r.NumberOrNull(36);
        var sources = r.Text(40);
        double? discount = marketValue is > 0 && asking is > 0 ? Math.Round((marketValue.Value - asking.Value) / marketValue.Value * 100, 1) : null;
        var storedScore = IntOrNull(r, 28);
        var (autoScore, reasons) = AutoScore(discount, asking, maxOffer, signal, phone.Length > 0 || email.Length > 0, missing.Count == 0, sources);

        return new OpportunityProperty
        {
            Id = r.GetInt32(0),
            PublicId = r.GetString(1),
            ExternalId = r.Text(2),
            Source = r.GetString(3),
            SourceUrl = r.Text(4),
            PostType = r.Text(5),
            PostedAt = r.IsoOrNull(6),
            ImportedAt = r.Iso(7),
            UpdatedAt = r.Iso(8),
            Title = r.GetString(9),
            Address = address,
            City = r.Text(11),
            State = r.GetString(12),
            Zip = r.Text(13),
            Beds = r.NumberOrNull(14),
            Baths = r.NumberOrNull(15),
            Asking = asking,
            SellerSignal = signal,
            Priority = PropertyKeywords.Classify(signal).Priority,
            Images = images,
            Comparables = IntOrNull(r, 19),
            ArvLow = r.NumberOrNull(20),
            ArvHigh = r.NumberOrNull(21),
            Arv = arv,
            RepairsLow = r.NumberOrNull(23),
            RepairsHigh = r.NumberOrNull(24),
            Repairs = repairs,
            ClosingHolding = closing,
            Contingency = contingency,
            Score = storedScore ?? autoScore,
            ScoreIsAuto = storedScore is null,
            ScoreReasons = reasons,
            Notes = r.Text(29),
            Status = r.GetString(30),
            ReviewRequestedAt = r.IsoOrNull(31),
            SellerName = r.Text(32),
            SellerPhone = phone,
            SellerEmail = email,
            SellerProfileUrl = r.Text(35),
            MarketValue = marketValue,
            Discount = discount,
            Sqft = IntOrNull(r, 37),
            Latitude = r.NumberOrNull(38),
            Longitude = r.NumberOrNull(39),
            Sources = sources,
            PostText = r.Text(41),
            Missing = missing,
            Stage = missing.Count > 0 ? "info" : maxOffer is null ? "compare" : "offer",
            MaxOffer = maxOffer,
            Fit = maxOffer is null || asking is null ? null : asking <= maxOffer ? "green" : "yellow"
        };
    }

    /// <summary>
    /// Transparent 0–100 score so leads from every source can be compared on the same scale.
    /// Each reason is a code the UI translates, so the team sees exactly why a house ranks high.
    /// </summary>
    private static (int Score, List<string> Reasons) AutoScore(double? discount, double? asking, double? maxOffer,
        string signal, bool hasContact, bool complete, string sources)
    {
        var score = 40.0;
        var reasons = new List<string>();
        if (discount is double d)
        {
            score += Math.Clamp(d, -20, 40);
            reasons.Add($"discount:{d.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        }
        if (asking is not null && maxOffer is not null && asking <= maxOffer)
        {
            score += 15;
            reasons.Add("belowMaxOffer");
        }
        var k = PropertyKeywords.Classify(signal);
        if (k.Direct.Count > 0)
        {
            score += 5;
            reasons.Add("direct");
        }
        if (k.Condition.Count > 0)
        {
            score += 10;
            reasons.Add($"condition:{k.Condition.Count}");
        }
        if (k.Motivated.Count > 0 && (k.Direct.Count > 0 || k.Condition.Count > 0))
        {
            score += 5;
            reasons.Add($"motivated:{k.Motivated.Count}");
        }
        if (k.Priority == "high")
        {
            score += 10;
            reasons.Add("highPriority");
        }
        if (k.Situation.Count > 0)
        {
            score += 5;
            reasons.Add($"situation:{k.Situation.Count}");
        }
        if (hasContact)
        {
            score += 10;
            reasons.Add("contact");
        }
        if (complete)
        {
            score += 5;
            reasons.Add("complete");
        }
        var seenIn = sources.Length == 0 ? 0 : sources.Split(" · ").Length;
        if (seenIn > 1)
        {
            score += 5;
            reasons.Add($"sources:{seenIn}");
        }
        return ((int)Math.Round(Math.Clamp(score, 0, 100)), reasons);
    }
}

[ApiController]
[Route("api/opportunity")]
public class OpportunityController : ControllerBase
{
    private readonly OpportunityStore _store;
    private readonly IConfiguration _config;

    public OpportunityController(OpportunityStore store, IConfiguration config)
    {
        _store = store;
        _config = config;
    }

    [HttpGet("properties")]
    public IActionResult Properties() => Ok(_store.List());

    [HttpGet("properties/{id:int}/events")]
    public IActionResult Events(int id) => Ok(_store.Events(id));

    [HttpPatch("properties/{id:int}/numbers")]
    public IActionResult Numbers(int id, [FromBody] OfferNumbersDto dto)
    {
        if (dto.Arv is not > 0) return BadRequest(new { error = "Write the ARV.", errorEs = "Escribe el ARV." });
        if (dto.Repairs is not >= 0) return BadRequest(new { error = "Write the repairs amount.", errorEs = "Escribe el monto de repairs." });
        if (dto.ClosingHolding < 0 || dto.Contingency < 0) return BadRequest(new { error = "Amounts cannot be negative.", errorEs = "Los montos no pueden ser negativos." });
        var saved = _store.SaveNumbers(id, dto);
        return saved is null ? NotFound(new { error = "Property not found.", errorEs = "No se encontró la propiedad." }) : Ok(saved);
    }

    [HttpPost("properties/{id:int}/review")]
    public IActionResult Review(int id)
    {
        var saved = _store.MarkForReview(id);
        return saved is null ? NotFound(new { error = "Property not found.", errorEs = "No se encontró la propiedad." }) : Ok(saved);
    }

    [HttpPost("import")]
    [RequestSizeLimit(2_000_000)]
    public IActionResult Import([FromBody] PropertyImportDto dto)
    {
        var expected = _config["Opportunity:ImportApiKey"];
        if (string.IsNullOrWhiteSpace(expected))
            return StatusCode(503, new { error = "Import is not configured (Opportunity:ImportApiKey)." });
        var given = Request.Headers["X-Api-Key"].ToString();
        if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(given), Encoding.UTF8.GetBytes(expected)))
            return Unauthorized(new { error = "Invalid API key." });

        if (new[] { dto.Title, dto.Address, dto.City, dto.SourceUrl, dto.RawText }.All(string.IsNullOrWhiteSpace))
            return BadRequest(new { error = "Send at least a title, address, city, source URL or post text." });
        if (dto.AskingPrice < 0 || dto.ArvLow < 0 || dto.ArvHigh < 0 || dto.ArvEstimate < 0 || dto.RepairsLow < 0 || dto.RepairsHigh < 0 || dto.RepairsEstimate < 0)
            return BadRequest(new { error = "Amounts cannot be negative." });
        if (dto.Score is < 0 or > 100) return BadRequest(new { error = "Score must be between 0 and 100." });
        if (dto.State is { Length: > 0 } s && s.Trim().Length != 2) return BadRequest(new { error = "State must be a 2-letter code." });

        return Ok(_store.Import(dto));
    }
}

public sealed class OpportunityDbErrorFilter : IExceptionFilter
{
    private readonly ILogger<OpportunityDbErrorFilter> _log;
    public OpportunityDbErrorFilter(ILogger<OpportunityDbErrorFilter> log) => _log = log;

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not SqlException ex) return;
        _log.LogError(ex, "IndorDB error on {Path}", context.HttpContext.Request.Path);
        var missingTable = ex.Number == 208;
        context.Result = new ObjectResult(new
        {
            error = missingTable
                ? "IndorDB is missing the Opportunity Finder tables. Run sql/indordb-opportunity-finder.sql."
                : "IndorDB is not available. Try again in a moment.",
            errorEs = missingTable
                ? "A IndorDB le faltan las tablas de Opportunity Finder. Corre sql/indordb-opportunity-finder.sql."
                : "IndorDB no está disponible. Intenta de nuevo en un momento."
        })
        { StatusCode = 503 };
        context.ExceptionHandled = true;
    }
}

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.Operations;

public class OperationsCatalogStore
{
    public static readonly string[] Automatic = ["Yes", "Conditional", "No"];
    private readonly SafeSql _sql;
    private readonly string _basePath;

    public OperationsCatalogStore(SafeSql sql, IWebHostEnvironment env)
    {
        _sql = sql;
        _basePath = Path.Combine(env.WebRootPath, "repair-catalog.json");
    }

    public bool IsBaseItem(string id) => SafeCatalog.BaseItems(_basePath).Any(n => (string?)n?["id"] == id);

    public List<JsonNode> All() => SafeCatalog.All(_sql, _basePath);

    public JsonNode Save(string? id, CatalogItemDto dto, string? by)
    {
        using var db = _sql.Open();
        using var tx = db.BeginTransaction();
        string kind;
        if (id is null)
        {
            kind = "custom";
            // ItemId is NVARCHAR(20): keep the temporary key short (p + 19 hex chars).
            using var reserve = db.Command("""
                INSERT INTO dbo.SafeOpsCatalogItems (ItemId, Kind, Data, UpdatedBy)
                OUTPUT INSERTED.Seq
                VALUES (CONCAT(N'p', LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 19)), N'custom', N'{}', @by)
                """, tx).With("@by", by);
            var seq = (int)reserve.ExecuteScalar()!;
            id = $"C{seq:0000}";
            using var name = db.Command("UPDATE dbo.SafeOpsCatalogItems SET ItemId = @id WHERE Seq = @seq", tx)
                .With("@id", id).With("@seq", seq);
            name.ExecuteNonQuery();
        }
        else kind = IsBaseItem(id) ? "override" : "custom";

        var data = JsonSerializer.SerializeToNode(dto with { Id = id })!;
        using var upsert = db.Command("""
            UPDATE dbo.SafeOpsCatalogItems SET Data = @data, UpdatedBy = @by, UpdatedAt = SYSUTCDATETIME() WHERE ItemId = @id;
            IF @@ROWCOUNT = 0
              INSERT INTO dbo.SafeOpsCatalogItems (ItemId, Kind, Data, UpdatedBy) VALUES (@id, @kind, @data, @by);
            SELECT UpdatedAt FROM dbo.SafeOpsCatalogItems WHERE ItemId = @id;
            """, tx)
            .With("@id", id)
            .With("@kind", kind)
            .With("@data", data.ToJsonString())
            .With("@by", by);
        var updatedAt = DateTime.SpecifyKind((DateTime)upsert.ExecuteScalar()!, DateTimeKind.Utc);
        tx.Commit();
        data["origin"] = kind;
        data["updatedAt"] = updatedAt.ToString("o");
        return data;
    }

    public bool Exists(string id)
    {
        if (IsBaseItem(id)) return true;
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT 1 FROM dbo.SafeOpsCatalogItems WHERE ItemId = @id").With("@id", id);
        return cmd.ExecuteScalar() is not null;
    }
}

public record CatalogItemDto(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("category")] string? Category,
    [property: JsonPropertyName("service")] string? Service,
    [property: JsonPropertyName("customer_en")] string? CustomerEn,
    [property: JsonPropertyName("inspector")] string? Inspector,
    [property: JsonPropertyName("customer_es")] string? CustomerEs,
    [property: JsonPropertyName("tier")] string? Tier,
    [property: JsonPropertyName("pricing_model")] string? PricingModel,
    [property: JsonPropertyName("low")] double Low,
    [property: JsonPropertyName("high")] double High,
    [property: JsonPropertyName("unit")] string? Unit,
    [property: JsonPropertyName("techs")] double Techs,
    [property: JsonPropertyName("hours")] double Hours,
    [property: JsonPropertyName("hours_low")] double? HoursLow,
    [property: JsonPropertyName("hours_high")] double? HoursHigh,
    [property: JsonPropertyName("materials")] string? Materials,
    [property: JsonPropertyName("materials_low")] double? MaterialsLow,
    [property: JsonPropertyName("materials_high")] double? MaterialsHigh,
    [property: JsonPropertyName("automatic")] string? Automatic,
    [property: JsonPropertyName("review")] string? Review,
    [property: JsonPropertyName("notes")] string? Notes,
    [property: JsonPropertyName("updatedBy")] string? UpdatedBy);

[ApiController]
[Route("api/operations")]
public class OperationsCatalogController : ControllerBase
{
    private readonly OperationsCatalogStore _store;
    public OperationsCatalogController(OperationsCatalogStore store) => _store = store;

    [HttpGet("catalog")]
    public IActionResult Catalog() => Ok(_store.All());

    [HttpPost("catalog")]
    public IActionResult Create([FromBody] CatalogItemDto dto)
    {
        var error = Validate(dto);
        if (error is not null) return BadRequest(new { error });
        var duplicate = _store.All().FirstOrDefault(n =>
            string.Equals((string?)n["service"], dto.Service!.Trim(), StringComparison.OrdinalIgnoreCase) &&
            string.Equals((string?)n["category"], dto.Category!.Trim(), StringComparison.OrdinalIgnoreCase));
        if (duplicate is not null) return Conflict(new { error = "This repair already exists in the catalog.", existing = new { id = (string?)duplicate["id"], service = (string?)duplicate["service"] } });
        return Ok(_store.Save(null, Clean(dto), dto.UpdatedBy));
    }

    [HttpPut("catalog/{id}")]
    public IActionResult Update(string id, [FromBody] CatalogItemDto dto)
    {
        if (!_store.Exists(id)) return NotFound();
        var error = Validate(dto);
        if (error is not null) return BadRequest(new { error });
        return Ok(_store.Save(id, Clean(dto), dto.UpdatedBy));
    }

    private static string? Validate(CatalogItemDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Service) || dto.Service.Trim().Length < 3) return "Write the repair name.";
        if (string.IsNullOrWhiteSpace(dto.Category)) return "Choose a category.";
        if (dto.Low < 0 || dto.High < 0 || dto.Low > 1_000_000 || dto.High > 1_000_000) return "Write valid prices.";
        if (dto.High < dto.Low) return "The high price must be equal to or above the low price.";
        if (string.IsNullOrWhiteSpace(dto.Unit)) return "Write the unit, like each or sq ft.";
        if (dto.Automatic is null || !OperationsCatalogStore.Automatic.Contains(dto.Automatic)) return "Choose whether the range is instant or needs review.";
        if (dto.Hours < 0 || dto.Techs < 0) return "Hours and technicians cannot be negative.";
        if (dto.HoursLow < 0 || dto.HoursHigh < 0 || dto.HoursHigh > 1000) return "Write valid technician-hours.";
        if (dto.HoursLow is not null && dto.HoursHigh is not null && dto.HoursHigh < dto.HoursLow) return "The maximum hours must be equal to or above the minimum.";
        if (dto.MaterialsLow < 0 || dto.MaterialsHigh < 0 || dto.MaterialsHigh > 1_000_000) return "Write valid material costs.";
        if (dto.MaterialsLow is not null && dto.MaterialsHigh is not null && dto.MaterialsHigh < dto.MaterialsLow) return "The maximum material cost must be equal to or above the minimum.";
        return null;
    }

    private static CatalogItemDto Clean(CatalogItemDto dto) => dto with
    {
        Category = dto.Category!.Trim(),
        Service = dto.Service!.Trim(),
        CustomerEn = dto.CustomerEn?.Trim() ?? "",
        CustomerEs = dto.CustomerEs?.Trim() ?? "",
        Inspector = dto.Inspector?.Trim() ?? "",
        Tier = string.IsNullOrWhiteSpace(dto.Tier) ? "Standard" : dto.Tier.Trim(),
        PricingModel = string.IsNullOrWhiteSpace(dto.PricingModel) ? "Flat rate" : dto.PricingModel.Trim(),
        Unit = dto.Unit!.Trim(),
        Materials = dto.Materials?.Trim() ?? "",
        Review = dto.Review?.Trim() ?? "",
        Notes = dto.Notes?.Trim() ?? "",
        UpdatedBy = dto.UpdatedBy?.Trim()
    };
}

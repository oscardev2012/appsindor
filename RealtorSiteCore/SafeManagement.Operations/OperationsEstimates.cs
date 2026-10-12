using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.Operations;

public class OperationsEstimateStore
{
    public static readonly string[] Statuses = ["Draft", "Sent", "Approved", "Declined"];
    private const string Columns = "Id, PublicId, RequestId, Address, Client, Lines, Total, Status, Notes, CreatedBy, CreatedAt, UpdatedAt";
    private readonly SafeSql _sql;
    public OperationsEstimateStore(SafeSql sql) => _sql = sql;

    public object Save(EstimateDto dto, List<EstimateLine> lines, double total)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeOpsEstimates (RequestId, Address, Client, Lines, Total, Status, Notes, CreatedBy)
            OUTPUT INSERTED.Id
            VALUES (@request, @address, @client, @lines, @total, N'Draft', @notes, @by)
            """)
            .With("@request", string.IsNullOrWhiteSpace(dto.RequestId) ? null : dto.RequestId.Trim())
            .With("@address", dto.Address!.Trim())
            .With("@client", dto.Client!.Trim())
            .With("@lines", JsonSerializer.Serialize(lines))
            .With("@total", (decimal)total)
            .With("@notes", (dto.Notes ?? "").Trim())
            .With("@by", (dto.CreatedBy ?? "").Trim());
        var id = (int)cmd.ExecuteScalar()!;
        return All(id).First();
    }

    public object? SetStatus(string publicId, string status)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeOpsEstimates SET Status = @status, UpdatedAt = SYSUTCDATETIME()
            OUTPUT INSERTED.Id
            WHERE PublicId = @id
            """)
            .With("@status", status)
            .With("@id", publicId);
        return cmd.ExecuteScalar() is int id ? All(id).First() : null;
    }

    public List<object> All(int? onlyId = null)
    {
        using var db = _sql.Open();
        using var cmd = db.Command($"SELECT {Columns} FROM dbo.SafeOpsEstimates WHERE @id IS NULL OR Id = @id ORDER BY Id DESC")
            .With("@id", onlyId);
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new
            {
                id = r.GetInt32(0),
                publicId = r.GetString(1),
                requestId = r.IsDBNull(2) ? null : r.GetString(2),
                address = r.GetString(3),
                client = r.GetString(4),
                lines = JsonSerializer.Deserialize<List<EstimateLine>>(r.GetString(5)),
                total = r.Number(6),
                status = r.GetString(7),
                notes = r.Text(8),
                createdBy = r.Text(9),
                createdAt = r.Iso(10),
                updatedAt = r.Iso(11),
                source = "local"
            });
        return rows;
    }
}

public record EstimateLine(string? ServiceId, string? Service, double Qty, string? Unit, double Price);
public record EstimateDto(string? RequestId, string? Address, string? Client, List<EstimateLine>? Lines, string? Notes, string? CreatedBy);
public record EstimateStatusDto(string? Status);

[ApiController]
[Route("api/operations")]
public class OperationsEstimatesController : ControllerBase
{
    private readonly OperationsEstimateStore _store;
    public OperationsEstimatesController(OperationsEstimateStore store) => _store = store;

    [HttpGet("estimates")]
    public IActionResult Estimates() => Ok(_store.All());

    [HttpPost("estimates")]
    public IActionResult Create([FromBody] EstimateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Address) || dto.Address.Trim().Length < 5) return BadRequest(new { error = "Write the property address." });
        if (string.IsNullOrWhiteSpace(dto.Client) || dto.Client.Trim().Length < 2) return BadRequest(new { error = "Write the client name." });
        var lines = (dto.Lines ?? []).Where(l => !string.IsNullOrWhiteSpace(l.Service)).ToList();
        if (lines.Count == 0) return BadRequest(new { error = "Add at least one service." });
        if (lines.Count > 50) return BadRequest(new { error = "An estimate can have up to 50 services." });
        if (lines.Any(l => l.Qty <= 0 || l.Qty > 10000 || l.Price < 0 || l.Price > 1_000_000))
            return BadRequest(new { error = "Every service needs a quantity above 0 and a valid price." });
        var total = Math.Round(lines.Sum(l => l.Qty * l.Price), 2);
        return Ok(_store.Save(dto, lines, total));
    }

    [HttpPatch("estimates/{publicId}")]
    public IActionResult Move(string publicId, [FromBody] EstimateStatusDto dto)
    {
        if (dto.Status is null || !OperationsEstimateStore.Statuses.Contains(dto.Status)) return BadRequest(new { error = "Unknown status." });
        var estimate = _store.SetStatus(publicId, dto.Status);
        return estimate is null ? NotFound() : Ok(estimate);
    }
}

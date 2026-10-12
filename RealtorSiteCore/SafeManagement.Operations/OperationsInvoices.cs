using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.Operations;

public class OperationsInvoiceStore
{
    public static readonly string[] Statuses = ["Open", "Paid", "Void"];
    private const string Columns = "Id, PublicId, EstimateId, Address, Client, Total, Status, DueDate, Notes, CreatedBy, PaidAt, CreatedAt";
    private readonly SafeSql _sql;
    public OperationsInvoiceStore(SafeSql sql) => _sql = sql;

    public object Save(InvoiceDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeOpsInvoices (EstimateId, Address, Client, Total, Status, DueDate, Notes, CreatedBy)
            OUTPUT INSERTED.Id
            VALUES (@estimate, @address, @client, @total, N'Open', @due, @notes, @by)
            """)
            .With("@estimate", (dto.EstimateId ?? "").Trim())
            .With("@address", dto.Address!.Trim())
            .With("@client", dto.Client!.Trim())
            .With("@total", (decimal)dto.Total)
            .With("@due", dto.DueDate is null ? null : DateOnly.ParseExact(dto.DueDate, "yyyy-MM-dd").ToDateTime(TimeOnly.MinValue))
            .With("@notes", (dto.Notes ?? "").Trim())
            .With("@by", (dto.CreatedBy ?? "").Trim());
        var id = (int)cmd.ExecuteScalar()!;
        return All(id).First();
    }

    public bool HasOpenFor(string estimateId)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT COUNT(1) FROM dbo.SafeOpsInvoices WHERE EstimateId = @id AND Status <> N'Void'").With("@id", estimateId.Trim());
        return (int)cmd.ExecuteScalar()! > 0;
    }

    public object? SetStatus(string publicId, string status)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeOpsInvoices
            SET Status = @status, PaidAt = CASE WHEN @status = N'Paid' THEN SYSUTCDATETIME() ELSE NULL END, UpdatedAt = SYSUTCDATETIME()
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
        using var cmd = db.Command($"SELECT {Columns} FROM dbo.SafeOpsInvoices WHERE @id IS NULL OR Id = @id ORDER BY Id DESC").With("@id", onlyId);
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new
            {
                id = r.GetInt32(0),
                publicId = r.GetString(1),
                estimateId = r.Text(2),
                address = r.GetString(3),
                client = r.GetString(4),
                total = r.Number(5),
                status = r.GetString(6),
                dueDate = r.IsDBNull(7) ? "" : r.GetDateTime(7).ToString("yyyy-MM-dd"),
                notes = r.Text(8),
                createdBy = r.Text(9),
                paidAt = r.IsoOrNull(10),
                createdAt = r.Iso(11),
                source = "local"
            });
        return rows;
    }
}

public record InvoiceDto(string? EstimateId, string? Address, string? Client, double Total, string? DueDate, string? Notes, string? CreatedBy);
public record InvoiceStatusDto(string? Status);

[ApiController]
[Route("api/operations")]
public class OperationsInvoicesController : ControllerBase
{
    private readonly OperationsInvoiceStore _store;
    public OperationsInvoicesController(OperationsInvoiceStore store) => _store = store;

    [HttpGet("invoices")]
    public IActionResult Invoices() => Ok(_store.All());

    [HttpPost("invoices")]
    public IActionResult Create([FromBody] InvoiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Address) || dto.Address.Trim().Length < 5) return BadRequest(new { error = "Write the property address." });
        if (string.IsNullOrWhiteSpace(dto.Client) || dto.Client.Trim().Length < 2) return BadRequest(new { error = "Write the client name." });
        if (dto.Total <= 0 || dto.Total > 10_000_000) return BadRequest(new { error = "The invoice total must be above $0." });
        if (dto.DueDate is not null && !DateOnly.TryParseExact(dto.DueDate, "yyyy-MM-dd", out _)) return BadRequest(new { error = "Invalid due date." });
        if (!string.IsNullOrWhiteSpace(dto.EstimateId) && _store.HasOpenFor(dto.EstimateId)) return Conflict(new { error = "This estimate already has an invoice." });
        return Ok(_store.Save(dto));
    }

    [HttpPatch("invoices/{publicId}")]
    public IActionResult Move(string publicId, [FromBody] InvoiceStatusDto dto)
    {
        if (dto.Status is null || !OperationsInvoiceStore.Statuses.Contains(dto.Status)) return BadRequest(new { error = "Unknown status." });
        var invoice = _store.SetStatus(publicId, dto.Status);
        return invoice is null ? NotFound() : Ok(invoice);
    }
}

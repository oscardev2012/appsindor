using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.Operations;

public class OperationsRequestStore
{
    private const string Columns = "Id, PublicId, Address, City, Contact, Source, Type, Status, Priority, Due, Owner, Items, Value, ApprovedAt, CreatedAt";
    private readonly SafeSql _sql;
    public OperationsRequestStore(SafeSql sql) => _sql = sql;

    public object Save(RequestDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeOpsRequests (Address, City, Contact, Source, Type, Status, Priority, Due, Owner)
            OUTPUT INSERTED.Id
            VALUES (@address, @city, @contact, @source, @type, N'New', N'medium', @due, N'Unassigned')
            """)
            .With("@address", dto.Address!.Trim())
            .With("@city", (dto.City ?? "").Trim())
            .With("@contact", dto.Contact!.Trim())
            .With("@source", string.IsNullOrWhiteSpace(dto.Source) ? "Manual" : dto.Source.Trim())
            .With("@type", string.IsNullOrWhiteSpace(dto.Type) ? "Management review" : dto.Type.Trim())
            .With("@due", dto.Due is null ? null : DateOnly.ParseExact(dto.Due, "yyyy-MM-dd").ToDateTime(TimeOnly.MinValue));
        var id = (int)cmd.ExecuteScalar()!;
        return All(id).First();
    }

    public object? Update(string publicId, RequestUpdateDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeOpsRequests SET
              Status = COALESCE(@status, Status),
              Owner = COALESCE(@owner, Owner),
              ApprovedAt = CASE WHEN @status IS NULL THEN ApprovedAt ELSE @approved END,
              Value = COALESCE(@value, Value),
              Items = COALESCE(@items, Items),
              UpdatedAt = SYSUTCDATETIME()
            OUTPUT INSERTED.Id
            WHERE PublicId = @id
            """)
            .With("@status", dto.Status)
            .With("@owner", dto.Owner)
            .With("@approved", DateTime.TryParse(dto.ApprovedAt, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var at) ? at : null)
            .With("@value", dto.Value is null ? null : (decimal)dto.Value.Value)
            .With("@items", dto.Items)
            .With("@id", publicId);
        return cmd.ExecuteScalar() is int id ? All(id).First() : null;
    }

    public List<object> All(int? onlyId = null)
    {
        using var db = _sql.Open();
        using var cmd = db.Command($"SELECT {Columns} FROM dbo.SafeOpsRequests WHERE @id IS NULL OR Id = @id ORDER BY Id DESC")
            .With("@id", onlyId);
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new
            {
                dbId = r.GetInt32(0),
                id = r.GetString(1),
                address = r.GetString(2),
                city = r.Text(3),
                contact = r.GetString(4),
                client = r.GetString(4),
                source = r.GetString(5),
                type = r.GetString(6),
                status = r.GetString(7),
                priority = r.GetString(8),
                due = r.IsDBNull(9) ? "" : r.GetDateTime(9).ToString("yyyy-MM-dd"),
                owner = r.GetString(10),
                items = r.GetInt32(11),
                value = r.Number(12),
                approvedAt = r.IsoOrNull(13),
                createdAt = r.Iso(14),
                origin = "local"
            });
        return rows;
    }
}

public record RequestDto(string? Address, string? City, string? Contact, string? Source, string? Type, string? Due);
public record RequestUpdateDto(string? Status, string? Owner, string? ApprovedAt, double? Value, int? Items);

[ApiController]
[Route("api/operations")]
public class OperationsRequestsController : ControllerBase
{
    private readonly OperationsRequestStore _store;
    public OperationsRequestsController(OperationsRequestStore store) => _store = store;

    [HttpGet("requests")]
    public IActionResult Requests() => Ok(_store.All());

    [HttpPost("requests")]
    public IActionResult Create([FromBody] RequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Address) || dto.Address.Trim().Length < 5) return BadRequest(new { error = "Write the property address." });
        if (string.IsNullOrWhiteSpace(dto.Contact) || dto.Contact.Trim().Length < 2) return BadRequest(new { error = "Write the Realtor or client name." });
        if (dto.Due is not null && !DateOnly.TryParseExact(dto.Due, "yyyy-MM-dd", out _)) return BadRequest(new { error = "Invalid due date." });
        return Ok(_store.Save(dto));
    }

    [HttpPatch("requests/{publicId}")]
    public IActionResult Update(string publicId, [FromBody] RequestUpdateDto dto)
    {
        var request = _store.Update(publicId, dto);
        return request is null ? NotFound() : Ok(request);
    }
}

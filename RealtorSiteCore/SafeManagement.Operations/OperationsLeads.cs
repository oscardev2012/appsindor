using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.Operations;

public class OperationsLeadStore
{
    public static readonly string[] Stages = ["New", "Qualified", "Conversation", "Demo"];
    private const string Columns = "Id, PublicId, Name, Firm, Email, Phone, Channel, Stage, Notes, CreatedAt, UpdatedAt";
    private readonly SafeSql _sql;
    public OperationsLeadStore(SafeSql sql) => _sql = sql;

    public object? FindDuplicate(LeadDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT TOP 1 PublicId, Name, Stage FROM dbo.SafeOpsLeads
            WHERE (@email <> N'' AND LOWER(Email) = @email) OR LOWER(Name) = @name
            """)
            .With("@email", (dto.Email ?? "").Trim().ToLowerInvariant())
            .With("@name", dto.Name!.Trim().ToLowerInvariant());
        using var r = cmd.ExecuteReader();
        return r.Read() ? new { publicId = r.GetString(0), name = r.GetString(1), stage = r.GetString(2) } : null;
    }

    public object Save(LeadDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeOpsLeads (Name, Firm, Email, Phone, Channel, Stage, Notes)
            OUTPUT INSERTED.Id
            VALUES (@name, @firm, @email, @phone, @channel, @stage, @notes)
            """)
            .With("@name", dto.Name!.Trim())
            .With("@firm", (dto.Firm ?? "").Trim())
            .With("@email", (dto.Email ?? "").Trim())
            .With("@phone", (dto.Phone ?? "").Trim())
            .With("@channel", string.IsNullOrWhiteSpace(dto.Channel) ? "Manual" : dto.Channel.Trim())
            .With("@stage", dto.Stage)
            .With("@notes", (dto.Notes ?? "").Trim());
        var id = (int)cmd.ExecuteScalar()!;
        return All(id).First();
    }

    public object? SetStage(string publicId, string stage)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeOpsLeads SET Stage = @stage, UpdatedAt = SYSUTCDATETIME()
            OUTPUT INSERTED.Id
            WHERE PublicId = @id
            """)
            .With("@stage", stage)
            .With("@id", publicId);
        return cmd.ExecuteScalar() is int id ? All(id).First() : null;
    }

    public List<object> All(int? onlyId = null)
    {
        using var db = _sql.Open();
        using var cmd = db.Command($"SELECT {Columns} FROM dbo.SafeOpsLeads WHERE @id IS NULL OR Id = @id ORDER BY Id")
            .With("@id", onlyId);
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new
            {
                id = r.GetInt32(0),
                publicId = r.GetString(1),
                name = r.GetString(2),
                firm = r.Text(3),
                email = r.Text(4),
                phone = r.Text(5),
                channel = r.GetString(6),
                stage = r.GetString(7),
                notes = r.Text(8),
                createdAt = r.Iso(9),
                updatedAt = r.Iso(10),
                source = "local"
            });
        return rows;
    }
}

public record LeadDto(string? Name, string? Firm, string? Email, string? Phone, string? Channel, string? Stage, string? Notes);
public record LeadStageDto(string? Stage);

[ApiController]
[Route("api/operations")]
public class OperationsLeadsController : ControllerBase
{
    private readonly OperationsLeadStore _store;
    public OperationsLeadsController(OperationsLeadStore store) => _store = store;

    [HttpGet("leads")]
    public IActionResult Leads() => Ok(_store.All());

    [HttpPost("leads")]
    public IActionResult Create([FromBody] LeadDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Trim().Length < 2) return BadRequest(new { error = "Write the lead name." });
        if (dto.Stage is null || !OperationsLeadStore.Stages.Contains(dto.Stage)) return BadRequest(new { error = "Unknown stage." });
        if (!string.IsNullOrWhiteSpace(dto.Email) && !System.Text.RegularExpressions.Regex.IsMatch(dto.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$"))
            return BadRequest(new { error = "Write a valid email, like name@firm.com." });
        if (!string.IsNullOrWhiteSpace(dto.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(dto.Phone.Trim(), @"^\d{10}$"))
            return BadRequest(new { error = "The phone must have exactly 10 digits, numbers only." });
        var dup = _store.FindDuplicate(dto);
        if (dup is not null) return Conflict(new { error = "This lead already exists.", existing = dup });
        return Ok(_store.Save(dto));
    }

    [HttpPatch("leads/{publicId}")]
    public IActionResult Move(string publicId, [FromBody] LeadStageDto dto)
    {
        if (dto.Stage is null || !OperationsLeadStore.Stages.Contains(dto.Stage)) return BadRequest(new { error = "Unknown stage." });
        var lead = _store.SetStage(publicId, dto.Stage);
        return lead is null ? NotFound() : Ok(lead);
    }
}

using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.Operations;

public class OperationsContactStore
{
    private const string Columns = "Id, PublicId, Role, Name, Email, Phone, Firm, Address, Notes, CreatedAt";
    private readonly SafeSql _sql;
    public OperationsContactStore(SafeSql sql) => _sql = sql;

    public object? FindDuplicate(ContactDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT TOP 1 PublicId, Name FROM dbo.SafeOpsContacts
            WHERE Role = @role AND ((@email <> N'' AND LOWER(Email) = @email) OR LOWER(Name) = @name)
            """)
            .With("@role", dto.Role)
            .With("@email", (dto.Email ?? "").Trim().ToLowerInvariant())
            .With("@name", dto.Name!.Trim().ToLowerInvariant());
        using var r = cmd.ExecuteReader();
        return r.Read() ? new { publicId = r.GetString(0), name = r.GetString(1) } : null;
    }

    public object Save(ContactDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeOpsContacts (Role, Name, Email, Phone, Firm, Address, Notes)
            OUTPUT INSERTED.Id
            VALUES (@role, @name, @email, @phone, @firm, @address, @notes)
            """)
            .With("@role", dto.Role)
            .With("@name", dto.Name!.Trim())
            .With("@email", (dto.Email ?? "").Trim())
            .With("@phone", (dto.Phone ?? "").Trim())
            .With("@firm", (dto.Firm ?? "").Trim())
            .With("@address", (dto.Address ?? "").Trim())
            .With("@notes", (dto.Notes ?? "").Trim());
        var id = (int)cmd.ExecuteScalar()!;
        return All(id).First();
    }

    public List<object> All(int? onlyId = null)
    {
        using var db = _sql.Open();
        using var cmd = db.Command($"SELECT {Columns} FROM dbo.SafeOpsContacts WHERE @id IS NULL OR Id = @id ORDER BY Id")
            .With("@id", onlyId);
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new
            {
                id = r.GetInt32(0),
                publicId = r.GetString(1),
                role = r.GetString(2),
                name = r.GetString(3),
                email = r.Text(4),
                phone = r.Text(5),
                firm = r.Text(6),
                address = r.Text(7),
                notes = r.Text(8),
                createdAt = r.Iso(9),
                source = "local"
            });
        return rows;
    }
}

public record ContactDto(string? Role, string? Name, string? Email, string? Phone, string? Firm, string? Address, string? Notes);

[ApiController]
[Route("api/operations")]
public class OperationsContactsController : ControllerBase
{
    private static readonly HashSet<string> Roles = ["Realtor", "Client", "Worker"];
    private readonly OperationsContactStore _store;
    public OperationsContactsController(OperationsContactStore store) => _store = store;

    [HttpGet("contacts")]
    public IActionResult Contacts() => Ok(_store.All());

    [HttpPost("contacts")]
    public IActionResult Create([FromBody] ContactDto dto)
    {
        if (dto.Role is null || !Roles.Contains(dto.Role)) return BadRequest(new { error = "Unknown contact type." });
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Trim().Length < 2) return BadRequest(new { error = "Write the full name." });
        if (!string.IsNullOrWhiteSpace(dto.Email) && !System.Text.RegularExpressions.Regex.IsMatch(dto.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$"))
            return BadRequest(new { error = "Write a valid email, like name@firm.com." });
        if (!string.IsNullOrWhiteSpace(dto.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(dto.Phone.Trim(), @"^\d{10}$"))
            return BadRequest(new { error = "The phone must have exactly 10 digits, numbers only." });
        var dup = _store.FindDuplicate(dto);
        if (dup is not null) return Conflict(new { error = "This contact already exists.", existing = dup });
        return Ok(_store.Save(dto));
    }
}

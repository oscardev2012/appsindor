using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.FieldTeam;

public class FieldProfileStore
{
    private const int Iterations = 100_000;
    private readonly SafeSql _sql;
    public FieldProfileStore(SafeSql sql) => _sql = sql;

    public static string Key(string? email) => (email ?? "").Trim().ToLowerInvariant();

    private static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
        return $"pbkdf2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    private static bool Verify(string password, string stored)
    {
        var parts = stored.Split('$');
        if (parts.Length != 4 || parts[0] != "pbkdf2" || !int.TryParse(parts[1], out var iterations)) return false;
        var salt = Convert.FromBase64String(parts[2]);
        var expected = Convert.FromBase64String(parts[3]);
        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }

    public bool Exists(string email)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT COUNT(1) FROM dbo.SafeFieldWorkerProfiles WHERE Email = @email").With("@email", Key(email));
        return (int)cmd.ExecuteScalar()! > 0;
    }

    public object Create(FieldProfileDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeFieldWorkerProfiles (Email, Name, Company, Phone, Specialties, PasswordHash)
            VALUES (@email, @name, @company, @phone, @specialties, @hash)
            """)
            .With("@email", Key(dto.Email))
            .With("@name", dto.Name!.Trim())
            .With("@company", (dto.Company ?? "").Trim())
            .With("@phone", (dto.Phone ?? "").Trim())
            .With("@specialties", JsonSerializer.Serialize(Clean(dto.Specialties)))
            .With("@hash", Hash(dto.Password!));
        cmd.ExecuteNonQuery();
        return Find(dto.Email)!;
    }

    public object? SignIn(string email, string password)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT PasswordHash FROM dbo.SafeFieldWorkerProfiles WHERE Email = @email").With("@email", Key(email));
        return cmd.ExecuteScalar() is string stored && Verify(password, stored) ? Find(email) : null;
    }

    public object? Update(string email, FieldProfileDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeFieldWorkerProfiles
            SET Name = @name, Company = @company, Phone = @phone, Specialties = @specialties, UpdatedAt = SYSUTCDATETIME()
            WHERE Email = @email
            """)
            .With("@email", Key(email))
            .With("@name", dto.Name!.Trim())
            .With("@company", (dto.Company ?? "").Trim())
            .With("@phone", (dto.Phone ?? "").Trim())
            .With("@specialties", JsonSerializer.Serialize(Clean(dto.Specialties)));
        return cmd.ExecuteNonQuery() > 0 ? Find(email) : null;
    }

    public object? Find(string? email)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Email, Name, Company, Phone, Specialties, CreatedAt FROM dbo.SafeFieldWorkerProfiles WHERE Email = @email")
            .With("@email", Key(email));
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return new
        {
            email = r.GetString(0),
            name = r.GetString(1),
            company = r.Text(2),
            phone = r.Text(3),
            specialties = JsonSerializer.Deserialize<List<string>>(r.GetString(4)) ?? [],
            createdAt = r.Iso(5)
        };
    }

    private static List<string> Clean(List<string>? values)
        => (values ?? []).Select(v => v.Trim()).Where(v => v.Length is > 0 and <= 80).Distinct().Take(20).ToList();
}

public record FieldProfileDto(string? Email, string? Name, string? Company, string? Phone, List<string>? Specialties, string? Password);
public record FieldSignInDto(string? Email, string? Password);

[ApiController]
[Route("api/field")]
public class FieldProfilesController : ControllerBase
{
    private readonly FieldProfileStore _store;
    public FieldProfilesController(FieldProfileStore store) => _store = store;

    private static string? Invalid(FieldProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Trim().Length < 2) return "Write your name.";
        if ((dto.Company ?? "").Trim().Length > 200) return "The company name is too long.";
        if (!string.IsNullOrWhiteSpace(dto.Phone) && !Regex.IsMatch(dto.Phone.Trim(), @"^\d{10}$")) return "The phone must have exactly 10 digits.";
        if (dto.Specialties is not { Count: > 0 }) return "Choose at least one specialty.";
        return null;
    }

    [HttpPost("profiles")]
    public IActionResult Create([FromBody] FieldProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || !Regex.IsMatch(dto.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$")) return BadRequest(new { error = "Write a valid email." });
        if ((dto.Password ?? "").Length < 6) return BadRequest(new { error = "Use at least 6 characters for the password." });
        if (Invalid(dto) is { } error) return BadRequest(new { error });
        if (_store.Exists(dto.Email)) return Conflict(new { error = "This email already has an account. Sign in." });
        return Ok(_store.Create(dto));
    }

    [HttpPost("profiles/signin")]
    public IActionResult SignIn([FromBody] FieldSignInDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrEmpty(dto.Password)) return BadRequest(new { error = "Write your email and password." });
        if (!_store.Exists(dto.Email)) return NotFound(new { error = "No account found for this email." });
        var profile = _store.SignIn(dto.Email, dto.Password);
        return profile is null ? Unauthorized(new { error = "Wrong password." }) : Ok(profile);
    }

    [HttpPut("profiles/{email}")]
    public IActionResult Update(string email, [FromBody] FieldProfileDto dto)
    {
        if (Invalid(dto) is { } error) return BadRequest(new { error });
        var profile = _store.Update(email, dto);
        return profile is null ? NotFound(new { error = "Profile not found." }) : Ok(profile);
    }

    [HttpGet("catalog")]
    public IActionResult Catalog([FromServices] SafeSql sql, [FromServices] IWebHostEnvironment env)
    {
        var basePath = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), "repair-catalog.json");
        return System.IO.File.Exists(basePath) ? Ok(SafeCatalog.All(sql, basePath)) : NotFound();
    }
}

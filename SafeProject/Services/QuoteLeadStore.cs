using System.Text.Json;
using CalentonesAgua.Models;
using Microsoft.Data.SqlClient;

namespace CalentonesAgua.Services;

public sealed class QuoteLeadStore
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string? _connectionString;
    private readonly string _path;
    private readonly ILogger<QuoteLeadStore> _log;
    private readonly object _gate = new();

    public QuoteLeadStore(IWebHostEnvironment env, IConfiguration config, ILogger<QuoteLeadStore> log)
    {
        _connectionString = config.GetConnectionString("IndorDB")?.Trim();
        var configured = config["Updates:QuoteLeadStorePath"];
        _path = string.IsNullOrWhiteSpace(configured)
            ? Path.Combine(env.ContentRootPath, "App_Data", "quote-leads.json")
            : configured;
        _log = log;
    }

    public string Save(SeoQuoteViewModel model)
    {
        var lead = new QuoteLead
        {
            Id = Guid.NewGuid(),
            Reference = $"QT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
            FullName = Required(model.Name, 120),
            Phone = Required(model.Phone, 40),
            Email = Clip(model.Email, 180),
            Address = Required(model.Address, 240),
            City = Required(model.City, 80),
            Fuel = Required(model.Fuel, 40),
            TankSize = Clip(model.TankSize, 40),
            Issue = Clip(model.Issue, 500),
            SmsConsent = model.SmsConsent,
            UtmSource = Clip(model.UtmSource, 80),
            UtmCampaign = Clip(model.UtmCampaign, 120),
            Gclid = Clip(model.Gclid, 120),
            LandingPath = Clip(model.LandingPath, 200),
            CreatedUtc = DateTimeOffset.UtcNow,
        };

        if (UseSql)
        {
            try
            {
                InsertSql(lead);
                return lead.Reference;
            }
            catch (Exception ex) when (ex is SqlException or InvalidOperationException)
            {
                _log.LogError(ex, "Quote lead {Reference} could not be written to IndorDB; saving a local copy.", lead.Reference);
            }
        }

        AppendFile(lead);
        return lead.Reference;
    }

    public IReadOnlyList<QuoteLead> RecentFromFile()
    {
        lock (_gate)
        {
            return LoadUnlocked();
        }
    }

    private void InsertSql(QuoteLead lead)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var cmd = new SqlCommand("""
            INSERT INTO dbo.QuoteLeads
                (Id, Reference, FullName, Phone, Email, Address, City, Fuel, TankSize, Issue, SmsConsent, UtmSource, UtmCampaign, Gclid, LandingPath, CreatedUtc)
            VALUES
                (@id, @reference, @name, @phone, @email, @address, @city, @fuel, @size, @issue, @sms, @utmSource, @utmCampaign, @gclid, @landing, @created)
            """, connection);
        cmd.Parameters.AddWithValue("@id", lead.Id);
        cmd.Parameters.AddWithValue("@reference", lead.Reference);
        cmd.Parameters.AddWithValue("@name", lead.FullName);
        cmd.Parameters.AddWithValue("@phone", lead.Phone);
        cmd.Parameters.AddWithValue("@email", (object?)lead.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@address", lead.Address);
        cmd.Parameters.AddWithValue("@city", lead.City);
        cmd.Parameters.AddWithValue("@fuel", lead.Fuel);
        cmd.Parameters.AddWithValue("@size", (object?)lead.TankSize ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@issue", (object?)lead.Issue ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@sms", lead.SmsConsent);
        cmd.Parameters.AddWithValue("@utmSource", (object?)lead.UtmSource ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@utmCampaign", (object?)lead.UtmCampaign ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@gclid", (object?)lead.Gclid ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@landing", (object?)lead.LandingPath ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@created", lead.CreatedUtc);
        cmd.ExecuteNonQuery();
    }

    private void AppendFile(QuoteLead lead)
    {
        lock (_gate)
        {
            var leads = LoadUnlocked();
            leads.Add(lead);
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllText(_path, JsonSerializer.Serialize(new FileShape { Leads = leads }, JsonOptions));
        }
    }

    private bool UseSql => !string.IsNullOrWhiteSpace(_connectionString);

    private List<QuoteLead> LoadUnlocked()
    {
        if (!File.Exists(_path))
        {
            return [];
        }

        try
        {
            var json = File.ReadAllText(_path);
            var file = JsonSerializer.Deserialize<FileShape>(json, JsonOptions);
            return file?.Leads ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private static string Required(string? value, int max) => Clip(value, max) ?? "";

    private static string? Clip(string? value, int max)
    {
        var text = (value ?? "").Trim();
        if (text.Length == 0)
        {
            return null;
        }

        return text.Length <= max ? text : text[..max];
    }

    private sealed class FileShape
    {
        public List<QuoteLead> Leads { get; set; } = [];
    }
}

public sealed class QuoteLead
{
    public Guid Id { get; set; }
    public string Reference { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string? Email { get; set; }
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string Fuel { get; set; } = "";
    public string? TankSize { get; set; }
    public string? Issue { get; set; }
    public bool SmsConsent { get; set; }
    public string? UtmSource { get; set; }
    public string? UtmCampaign { get; set; }
    public string? Gclid { get; set; }
    public string? LandingPath { get; set; }
    public DateTimeOffset CreatedUtc { get; set; }
}

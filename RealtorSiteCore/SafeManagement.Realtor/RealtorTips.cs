using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SafeManagement.Data;
using System.Text.Json;

namespace SafeManagement.Realtor;

public class RealtorTipStore
{
    private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true };
    private readonly SafeSql _sql;
    public RealtorTipStore(SafeSql sql) => _sql = sql;

    private static string Email(string? value) => (value ?? "").Trim().ToLowerInvariant();

    private static string Utc(DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc).ToString("o");

    public void SaveClient(ClientServiceDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeRealtorClientServices
              SET Name = @name, Email = @email, Phone = @phone, Address = @address, Services = @services, UpdatedAt = SYSUTCDATETIME()
              WHERE RealtorEmail = @realtor AND ClientKey = @key;
            IF @@ROWCOUNT = 0
              INSERT INTO dbo.SafeRealtorClientServices (RealtorEmail, ClientKey, Name, Email, Phone, Address, Services)
              VALUES (@realtor, @key, @name, @email, @phone, @address, @services);
            """)
            .With("@realtor", Email(dto.RealtorEmail))
            .With("@key", ClientKey(dto.Email, dto.Name))
            .With("@name", dto.Name.Trim())
            .With("@email", dto.Email ?? "")
            .With("@phone", dto.Phone ?? "")
            .With("@address", dto.Address ?? "")
            .With("@services", JsonSerializer.Serialize(dto.Services ?? new List<ServicePickDto>()));
        cmd.ExecuteNonQuery();
    }

    public List<object> Clients(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Name, Email, Phone, Address, Services, UpdatedAt FROM dbo.SafeRealtorClientServices WHERE RealtorEmail = @realtor ORDER BY Name")
            .With("@realtor", Email(realtorEmail));
        var rows = new List<object>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var services = JsonSerializer.Deserialize<List<ServicePickDto>>(reader.GetString(4), Json) ?? new();
            rows.Add(new
            {
                name = reader.GetString(0),
                email = reader.Text(1),
                phone = reader.Text(2),
                address = reader.Text(3),
                services = services.Select(s => new { id = s.Id ?? "", service = s.Service ?? "", category = s.Category ?? "" }),
                updatedAt = reader.Iso(5)
            });
        }
        return rows;
    }

    public object SaveClientProfile(ClientProfileDto dto)
    {
        var key = ClientKey(dto.Email, dto.Name);
        using var db = _sql.Open();
        using var cmd = db.Command("""
            IF @oldKey <> N'' AND @oldKey <> @key
              DELETE FROM dbo.SafeRealtorClientProfiles WHERE RealtorEmail = @realtor AND ClientKey = @oldKey;
            UPDATE dbo.SafeRealtorClientProfiles
              SET Name = @name, Email = @email, Phone = @phone, Address = @address, Side = @side, ClosingDate = @closing,
                  Birthday = @birthday, PreferredChannel = @channel, Household = @household, Interests = @interests,
                  Notes = @notes, Consent = @consent, UpdatedAt = SYSUTCDATETIME()
              WHERE RealtorEmail = @realtor AND ClientKey = @key;
            IF @@ROWCOUNT = 0
              INSERT INTO dbo.SafeRealtorClientProfiles (RealtorEmail, ClientKey, Name, Email, Phone, Address, Side, ClosingDate, Birthday, PreferredChannel, Household, Interests, Notes, Consent)
              VALUES (@realtor, @key, @name, @email, @phone, @address, @side, @closing, @birthday, @channel, @household, @interests, @notes, @consent);
            SELECT UpdatedAt FROM dbo.SafeRealtorClientProfiles WHERE RealtorEmail = @realtor AND ClientKey = @key;
            """)
            .With("@realtor", Email(dto.RealtorEmail))
            .With("@key", key)
            .With("@oldKey", (dto.Key ?? "").Trim().ToLowerInvariant())
            .With("@name", dto.Name.Trim())
            .With("@email", (dto.Email ?? "").Trim())
            .With("@phone", (dto.Phone ?? "").Trim())
            .With("@address", (dto.Address ?? "").Trim())
            .With("@side", (dto.Side ?? "").Trim())
            .With("@closing", (object?)dto.ClosingDate?.ToDateTime(TimeOnly.MinValue) ?? DBNull.Value)
            .With("@birthday", (object?)dto.Birthday?.ToDateTime(TimeOnly.MinValue) ?? DBNull.Value)
            .With("@channel", (dto.PreferredChannel ?? "").Trim())
            .With("@household", (dto.Household ?? "").Trim())
            .With("@interests", (dto.Interests ?? "").Trim())
            .With("@notes", (dto.Notes ?? "").Trim())
            .With("@consent", dto.Consent);
        var updatedAt = (DateTime)cmd.ExecuteScalar()!;
        return MapProfile(key, dto, Utc(updatedAt));
    }

    public List<object> ClientProfiles(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT ClientKey, Name, Email, Phone, Address, Side, ClosingDate, Birthday, PreferredChannel, Household, Interests, Notes, Consent, UpdatedAt
            FROM dbo.SafeRealtorClientProfiles WHERE RealtorEmail = @realtor ORDER BY Name
            """).With("@realtor", Email(realtorEmail));
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        static DateOnly? Day(SqlDataReader r, int i) => r.IsDBNull(i) ? null : DateOnly.FromDateTime(r.GetDateTime(i));
        while (r.Read())
        {
            var dto = new ClientProfileDto(null, null, r.GetString(1), r.Text(2), r.Text(3), r.Text(4), r.Text(5), Day(r, 6), Day(r, 7),
                r.Text(8), r.Text(9), r.Text(10), r.Text(11), r.GetBoolean(12));
            rows.Add(MapProfile(r.GetString(0), dto, Utc(r.GetDateTime(13))));
        }
        return rows;
    }

    private static object MapProfile(string key, ClientProfileDto d, string updatedAt) => new
    {
        key,
        name = d.Name.Trim(),
        email = d.Email ?? "",
        phone = d.Phone ?? "",
        address = d.Address ?? "",
        side = d.Side ?? "",
        closingDate = d.ClosingDate?.ToString("yyyy-MM-dd") ?? "",
        birthday = d.Birthday?.ToString("yyyy-MM-dd") ?? "",
        preferredChannel = d.PreferredChannel ?? "",
        household = d.Household ?? "",
        interests = d.Interests ?? "",
        notes = d.Notes ?? "",
        consent = d.Consent,
        updatedAt
    };

    public object SaveTip(TipSendDto dto)
    {
        var channel = string.IsNullOrWhiteSpace(dto.Channel) ? "Gmail" : dto.Channel!;
        var status = string.IsNullOrWhiteSpace(dto.Status) ? "Sent" : dto.Status!;
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeRealtorTipSends (RealtorEmail, TemplateId, Title, Body, Channel, SendDate, Status, Recipients)
            OUTPUT INSERTED.Id, INSERTED.CreatedAt
            VALUES (@realtor, @template, @title, @body, @channel, @sendDate, @status, @recipients)
            """)
            .With("@realtor", Email(dto.RealtorEmail))
            .With("@template", dto.TemplateId ?? "")
            .With("@title", dto.Title.Trim())
            .With("@body", (dto.Body ?? "").Trim())
            .With("@channel", channel)
            .With("@sendDate", dto.SendDate ?? "")
            .With("@status", status)
            .With("@recipients", JsonSerializer.Serialize(dto.Recipients));
        using var r = cmd.ExecuteReader();
        r.Read();
        return MapTip(r.GetInt32(0), dto.TemplateId ?? "", dto.Title.Trim(), (dto.Body ?? "").Trim(), channel, dto.SendDate ?? "", status, dto.Recipients ?? new(), r.Iso(1));
    }

    public List<object> Tips(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT TOP 30 Id, TemplateId, Title, Body, Channel, SendDate, Status, Recipients, CreatedAt FROM dbo.SafeRealtorTipSends WHERE RealtorEmail = @realtor ORDER BY Id DESC")
            .With("@realtor", Email(realtorEmail));
        var rows = new List<object>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var recipients = JsonSerializer.Deserialize<List<TipRecipientDto>>(reader.GetString(7), Json) ?? new();
            rows.Add(MapTip(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.Text(5), reader.GetString(6), recipients, reader.Iso(8)));
        }
        return rows;
    }

    private static object MapTip(long id, string templateId, string title, string body, string channel, string sendDate, string status, List<TipRecipientDto> recipients, string createdAt) => new
    {
        id,
        templateId,
        title,
        body,
        channel,
        sendDate,
        status,
        createdAt,
        recipients = recipients.Select(r => new
        {
            name = r.Name,
            email = r.Email ?? "",
            phone = r.Phone ?? "",
            address = r.Address ?? "",
            body = r.Body ?? "",
            services = (r.Services ?? new()).Select(s => new { id = s.Id ?? "", service = s.Service ?? "", category = s.Category ?? "" })
        })
    };

    public object SaveGiftCard(GiftCardDto dto)
    {
        var code = dto.Code ?? $"SAFE-{dto.Amount}";
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeRealtorGiftCards (RealtorEmail, ClientName, Address, Firm, Amount, Code, Logo)
            OUTPUT INSERTED.Id, INSERTED.CreatedAt
            VALUES (@realtor, @client, @address, @firm, @amount, @code, @logo)
            """)
            .With("@realtor", Email(dto.RealtorEmail))
            .With("@client", dto.ClientName!.Trim())
            .With("@address", dto.Address ?? "")
            .With("@firm", dto.Firm ?? "")
            .With("@amount", dto.Amount)
            .With("@code", code)
            .With("@logo", dto.Logo);
        using var r = cmd.ExecuteReader();
        r.Read();
        return new { id = r.GetInt32(0), clientName = dto.ClientName.Trim(), address = dto.Address ?? "", firm = dto.Firm ?? "", amount = dto.Amount, code, logo = dto.Logo, status = "Issued", createdAt = r.Iso(1) };
    }

    public List<object> GiftCards(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Id, ClientName, Address, Firm, Amount, Code, Logo, CreatedAt FROM dbo.SafeRealtorGiftCards WHERE RealtorEmail = @realtor ORDER BY Id DESC")
            .With("@realtor", Email(realtorEmail));
        var rows = new List<object>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            rows.Add(new { id = reader.GetInt32(0), clientName = reader.GetString(1), address = reader.Text(2), firm = reader.Text(3), amount = reader.GetInt32(4), code = reader.GetString(5), logo = reader.GetString(6), status = "Issued", createdAt = reader.Iso(7) });
        return rows;
    }

    public object Profile(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Logo, UpdatedAt FROM dbo.SafeRealtorProfiles WHERE RealtorEmail = @realtor")
            .With("@realtor", Email(realtorEmail));
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return new { logo = "", updatedAt = "" };
        return new { logo = reader.Text(0), updatedAt = reader.Iso(1) };
    }

    public void SaveLogo(string realtorEmail, string? logo)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeRealtorProfiles SET Logo = @logo, UpdatedAt = SYSUTCDATETIME() WHERE RealtorEmail = @realtor;
            IF @@ROWCOUNT = 0
              INSERT INTO dbo.SafeRealtorProfiles (RealtorEmail, Logo) VALUES (@realtor, @logo);
            """)
            .With("@realtor", Email(realtorEmail))
            .With("@logo", logo ?? "");
        cmd.ExecuteNonQuery();
    }

    public List<object> Connections(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Channel, Account, ConnectedAt FROM dbo.SafeRealtorChannelConnections WHERE RealtorEmail = @realtor")
            .With("@realtor", Email(realtorEmail));
        var rows = new List<object>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            rows.Add(new { channel = reader.GetString(0), account = reader.GetString(1), connectedAt = reader.Iso(2) });
        return rows;
    }

    public object Connect(ConnectionDto dto)
    {
        var channel = dto.Channel!.Trim().ToLowerInvariant();
        var account = dto.Account!.Trim();
        var now = DateTime.UtcNow;
        using var db = _sql.Open();
        using var cmd = db.Command("""
            UPDATE dbo.SafeRealtorChannelConnections SET Account = @account, ConnectedAt = @now WHERE RealtorEmail = @realtor AND Channel = @channel;
            IF @@ROWCOUNT = 0
              INSERT INTO dbo.SafeRealtorChannelConnections (RealtorEmail, Channel, Account, ConnectedAt) VALUES (@realtor, @channel, @account, @now);
            """)
            .With("@realtor", Email(dto.RealtorEmail))
            .With("@channel", channel)
            .With("@account", account)
            .With("@now", now);
        cmd.ExecuteNonQuery();
        return new { channel, account, connectedAt = Utc(now) };
    }

    public void Disconnect(string? realtorEmail, string channel)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("DELETE FROM dbo.SafeRealtorChannelConnections WHERE RealtorEmail = @realtor AND Channel = @channel")
            .With("@realtor", Email(realtorEmail))
            .With("@channel", channel.Trim().ToLowerInvariant());
        cmd.ExecuteNonQuery();
    }

    public object SaveChannelMessage(ChannelMessageDto dto)
    {
        var channel = (dto.Channel ?? "").Trim().ToLowerInvariant();
        var key = ClientKey(dto.ClientKey, dto.ClientName);
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeRealtorChannelMessages (RealtorEmail, Channel, ClientKey, ClientName, Destination, Body)
            OUTPUT INSERTED.Id, INSERTED.CreatedAt
            VALUES (@realtor, @channel, @key, @name, @destination, @body)
            """)
            .With("@realtor", Email(dto.RealtorEmail))
            .With("@channel", channel)
            .With("@key", key)
            .With("@name", dto.ClientName.Trim())
            .With("@destination", dto.Destination ?? "")
            .With("@body", dto.Body!.Trim());
        using var r = cmd.ExecuteReader();
        r.Read();
        return new { id = r.GetInt32(0), channel, clientKey = key, clientName = dto.ClientName.Trim(), destination = dto.Destination ?? "", body = dto.Body.Trim(), createdAt = r.Iso(1) };
    }

    public List<object> ChannelMessages(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Id, Channel, ClientKey, ClientName, Destination, Body, CreatedAt FROM dbo.SafeRealtorChannelMessages WHERE RealtorEmail = @realtor ORDER BY Id")
            .With("@realtor", Email(realtorEmail));
        var rows = new List<object>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            rows.Add(new
            {
                id = reader.GetInt32(0),
                channel = reader.GetString(1),
                clientKey = reader.GetString(2),
                clientName = reader.GetString(3),
                destination = reader.Text(4),
                body = reader.GetString(5),
                createdAt = reader.Iso(6)
            });
        return rows;
    }

    public object SavePlan(RepairPlanDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            IF @id > 0 AND EXISTS (SELECT 1 FROM dbo.SafeRealtorRepairPlans WHERE Id = @id AND RealtorEmail = @realtor)
            BEGIN
              UPDATE dbo.SafeRealtorRepairPlans
                SET ClientName = @client, ClientEmail = @clientEmail, Address = @address, Stage = @stage, TargetClose = @targetClose, Goal = @goal,
                    Items = @items, Pricing = @pricing, HourlyRate = @rate, Credit = @credit, MaxAuthorization = @max,
                    PreferredDate = @date, PreferredWindow = @window, AccessNotes = @access, Status = @status, RequestId = @requestId, Step = @step,
                    UpdatedAt = SYSUTCDATETIME()
                WHERE Id = @id;
              SELECT Id, CreatedAt, UpdatedAt FROM dbo.SafeRealtorRepairPlans WHERE Id = @id;
            END
            ELSE
              INSERT INTO dbo.SafeRealtorRepairPlans (RealtorEmail, ClientName, ClientEmail, Address, Stage, TargetClose, Goal, Items, Pricing, HourlyRate, Credit, MaxAuthorization, PreferredDate, PreferredWindow, AccessNotes, Status, RequestId, Step)
              OUTPUT INSERTED.Id, INSERTED.CreatedAt, INSERTED.UpdatedAt
              VALUES (@realtor, @client, @clientEmail, @address, @stage, @targetClose, @goal, @items, @pricing, @rate, @credit, @max, @date, @window, @access, @status, @requestId, @step);
            """)
            .With("@id", dto.Id ?? 0)
            .With("@realtor", Email(dto.RealtorEmail))
            .With("@client", (dto.ClientName ?? "").Trim())
            .With("@clientEmail", (dto.ClientEmail ?? "").Trim())
            .With("@address", (dto.Address ?? "").Trim())
            .With("@stage", dto.Stage ?? "")
            .With("@targetClose", dto.TargetClose ?? "")
            .With("@goal", dto.Goal ?? "")
            .With("@items", JsonSerializer.Serialize(dto.Items ?? new()))
            .With("@pricing", dto.Pricing ?? "fixed")
            .With("@rate", dto.HourlyRate)
            .With("@credit", dto.Credit)
            .With("@max", dto.MaxAuthorization)
            .With("@date", dto.PreferredDate ?? "")
            .With("@window", dto.PreferredWindow ?? "")
            .With("@access", (dto.AccessNotes ?? "").Trim())
            .With("@status", dto.Status ?? "Draft")
            .With("@requestId", dto.RequestId ?? "")
            .With("@step", Math.Clamp(dto.Step, 1, 8));
        using var r = cmd.ExecuteReader();
        r.Read();
        return MapPlan(r.GetInt32(0), dto, r.Iso(1), r.Iso(2));
    }

    public List<object> Plans(string? realtorEmail)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT TOP 30 Id, ClientName, ClientEmail, Address, Stage, TargetClose, Goal, Items, Pricing, HourlyRate, Credit, MaxAuthorization,
                   PreferredDate, PreferredWindow, AccessNotes, Status, RequestId, Step, CreatedAt, UpdatedAt
            FROM dbo.SafeRealtorRepairPlans WHERE RealtorEmail = @realtor ORDER BY UpdatedAt DESC
            """)
            .With("@realtor", Email(realtorEmail));
        var rows = new List<object>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var items = JsonSerializer.Deserialize<List<RepairPlanItemDto>>(reader.GetString(7), Json) ?? new();
            var dto = new RepairPlanDto(reader.GetInt32(0), realtorEmail, reader.Text(1), reader.Text(2), reader.Text(3), reader.Text(4), reader.Text(5), reader.Text(6),
                items, reader.Text(8), reader.GetDecimal(9), reader.GetInt32(10), reader.GetDecimal(11), reader.Text(12), reader.Text(13), reader.Text(14),
                reader.Text(15), reader.Text(16), reader.GetInt32(17));
            rows.Add(MapPlan(dto.Id ?? 0, dto, reader.Iso(18), reader.Iso(19)));
        }
        return rows;
    }

    private static object MapPlan(int id, RepairPlanDto d, string createdAt, string updatedAt) => new
    {
        id,
        clientName = d.ClientName ?? "",
        clientEmail = d.ClientEmail ?? "",
        address = d.Address ?? "",
        stage = d.Stage ?? "",
        targetClose = d.TargetClose ?? "",
        goal = d.Goal ?? "",
        items = d.Items ?? new(),
        pricing = d.Pricing ?? "fixed",
        hourlyRate = d.HourlyRate,
        credit = d.Credit,
        maxAuthorization = d.MaxAuthorization,
        preferredDate = d.PreferredDate ?? "",
        preferredWindow = d.PreferredWindow ?? "",
        accessNotes = d.AccessNotes ?? "",
        status = d.Status ?? "Draft",
        requestId = d.RequestId ?? "",
        step = d.Step,
        createdAt,
        updatedAt
    };

    private static string ClientKey(string? email, string name)
        => !string.IsNullOrWhiteSpace(email) ? email.Trim().ToLowerInvariant() : name.Trim().ToLowerInvariant();
}

public record ServicePickDto(string? Id, string? Service, string? Category);
public record ClientProfileDto(string? RealtorEmail, string? Key, string Name, string? Email, string? Phone, string? Address, string? Side,
    DateOnly? ClosingDate, DateOnly? Birthday, string? PreferredChannel, string? Household, string? Interests, string? Notes, bool Consent);
public record ClientServiceDto(string? RealtorEmail, string Name, string? Email, string? Phone, string? Address, List<ServicePickDto>? Services);
public record TipRecipientDto(string Name, string? Email, string? Phone, string? Address, string? Body, List<ServicePickDto>? Services);
public record TipSendDto(string? RealtorEmail, string? TemplateId, string Title, string? Body, string? Channel, string? SendDate, string? Status, List<TipRecipientDto>? Recipients);
public record GiftCardDto(string? RealtorEmail, string? ClientName, string? Address, string? Firm, int Amount, string? Code, string? Logo);
public record LogoDto(string? RealtorEmail, string? Logo);
public record ConnectionDto(string? RealtorEmail, string? Channel, string? Account);
public record ChannelMessageDto(string? RealtorEmail, string? Channel, string? ClientKey, string ClientName, string? Destination, string? Body);
public record RepairPlanItemDto(string Id, string? Service, string? Category, decimal Qty, string? Unit, decimal TechHours, decimal? Low, decimal? High, string? Materials, string? PricingModel, string? Finding, bool Quote,
    decimal? TechHoursHigh = null, decimal? MaterialsLow = null, decimal? MaterialsHigh = null);
public record RepairPlanDto(int? Id, string? RealtorEmail, string? ClientName, string? ClientEmail, string? Address, string? Stage, string? TargetClose, string? Goal,
    List<RepairPlanItemDto>? Items, string? Pricing, decimal HourlyRate, int Credit, decimal MaxAuthorization, string? PreferredDate, string? PreferredWindow,
    string? AccessNotes, string? Status, string? RequestId, int Step);

public class RepairPlanOptions
{
    public decimal HourlyRate { get; set; } = 125;
    public decimal RepairDayLow { get; set; } = 949;
    public decimal RepairDayHigh { get; set; } = 1299;
    public decimal RepairDayHours { get; set; } = 8;
}

[ApiController]
[Route("api/realtor")]
public class RealtorTipsController : ControllerBase
{
    private static readonly string[] PlanStatuses = { "Draft", "Approval sent", "Approved", "Sent to Management" };
    private readonly RealtorTipStore _store;
    private readonly RepairPlanOptions _plan;
    public RealtorTipsController(RealtorTipStore store, IConfiguration config)
    {
        _store = store;
        _plan = config.GetSection("RepairPlan").Get<RepairPlanOptions>() ?? new RepairPlanOptions();
    }

    private BadRequestObjectResult Bad(string en, string es) => BadRequest(new { error = en, errorEs = es });

    [HttpGet("client-services")]
    public IActionResult Clients([FromQuery] string? realtor) => Ok(_store.Clients(realtor));

    [HttpPost("client-services")]
    public IActionResult SaveClient([FromBody] ClientServiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return Bad("Name is required.", "Escribe el nombre.");
        if (dto.Services is null || dto.Services.Count == 0) return Bad("Choose the services this client needs.", "Elige los servicios que necesita este cliente.");
        _store.SaveClient(dto);
        return Ok(new { ok = true });
    }

    [HttpGet("tips")]
    public IActionResult Tips([FromQuery] string? realtor) => Ok(_store.Tips(realtor));

    [HttpPost("tips")]
    public IActionResult Send([FromBody] TipSendDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Body))
            return Bad("The message is empty.", "El mensaje está vacío.");
        if (dto.Recipients is null || dto.Recipients.Count == 0 || dto.Recipients.Any(r => string.IsNullOrWhiteSpace(r.Name)))
            return Bad("Choose the clients who need this service.", "Elige los clientes que necesitan este servicio.");
        return Ok(_store.SaveTip(dto));
    }

    [HttpGet("gift-cards")]
    public IActionResult GiftCards([FromQuery] string? realtor) => Ok(_store.GiftCards(realtor));

    [HttpPost("gift-cards")]
    [RequestSizeLimit(2_000_000)]
    public IActionResult GiftCard([FromBody] GiftCardDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RealtorEmail)) return Bad("Sign in first.", "Primero inicia sesión.");
        if (string.IsNullOrWhiteSpace(dto.ClientName)) return Bad("Write the client name.", "Escribe el nombre del cliente.");
        if (string.IsNullOrWhiteSpace(dto.Logo) || !dto.Logo.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
            return Bad("Upload the Realty firm logo.", "Sube el logo de la firma inmobiliaria.");
        if (dto.Amount is not (50 or 100 or 250 or 300)) return Bad("Choose a valid gift value.", "Elige un valor válido para la gift card.");
        return Ok(_store.SaveGiftCard(dto));
    }

    [HttpGet("profile")]
    public IActionResult Profile([FromQuery] string? realtor) => Ok(_store.Profile(realtor));

    [HttpPost("profile/logo")]
    [RequestSizeLimit(2_000_000)]
    public IActionResult Logo([FromBody] LogoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RealtorEmail)) return Bad("Sign in first.", "Primero inicia sesión.");
        if (!string.IsNullOrEmpty(dto.Logo) && !dto.Logo.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
            return Bad("The logo must be an image.", "El logo debe ser una imagen.");
        _store.SaveLogo(dto.RealtorEmail, dto.Logo);
        return Ok(new { ok = true });
    }

    [HttpGet("connections")]
    public IActionResult Connections([FromQuery] string? realtor) => Ok(_store.Connections(realtor));

    [HttpPost("connections")]
    public IActionResult Connect([FromBody] ConnectionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RealtorEmail) || string.IsNullOrWhiteSpace(dto.Channel) || string.IsNullOrWhiteSpace(dto.Account))
            return Bad("Write the account for this channel.", "Escribe la cuenta de este canal.");
        return Ok(_store.Connect(dto));
    }

    [HttpDelete("connections/{channel}")]
    public IActionResult Disconnect(string channel, [FromQuery] string? realtor)
    {
        _store.Disconnect(realtor, channel);
        return Ok(new { ok = true });
    }

    [HttpGet("channel-messages")]
    public IActionResult ChannelMessages([FromQuery] string? realtor) => Ok(_store.ChannelMessages(realtor));

    [HttpPost("channel-messages")]
    public IActionResult ChannelMessage([FromBody] ChannelMessageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Channel) || string.IsNullOrWhiteSpace(dto.ClientName) || string.IsNullOrWhiteSpace(dto.Body))
            return Bad("Choose a client and write the message.", "Elige un cliente y escribe el mensaje.");
        return Ok(_store.SaveChannelMessage(dto));
    }

    [HttpGet("client-profiles")]
    public IActionResult ClientProfiles([FromQuery] string? realtor) => Ok(_store.ClientProfiles(realtor));

    [HttpPost("client-profiles")]
    public IActionResult SaveClientProfile([FromBody] ClientProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RealtorEmail)) return Bad("Sign in first.", "Primero inicia sesión.");
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Trim().Length > 160) return Bad("Write the client name.", "Escribe el nombre del cliente.");
        if (!string.IsNullOrWhiteSpace(dto.Email) && !System.Text.RegularExpressions.Regex.IsMatch(dto.Email.Trim(), @"^[^\s@]+@[^\s@]+\.[A-Za-z]{2,}$"))
            return Bad("Check the client email.", "Revisa el correo del cliente.");
        var digits = new string((dto.Phone ?? "").Where(char.IsDigit).ToArray());
        if (!string.IsNullOrWhiteSpace(dto.Phone) && (digits.Length != 10 || digits.Length != (dto.Phone ?? "").Trim().Length))
            return Bad("The phone must have 10 digits.", "El teléfono debe tener 10 dígitos.");
        if (dto.Birthday is { } b && b > DateOnly.FromDateTime(DateTime.Today))
            return Bad("The birthday cannot be in the future.", "El cumpleaños no puede ser una fecha futura.");
        if ((dto.Household?.Length ?? 0) > 400 || (dto.Interests?.Length ?? 0) > 400 || (dto.Notes?.Length ?? 0) > 2000)
            return Bad("Some text is too long.", "Algún texto es demasiado largo.");
        return Ok(_store.SaveClientProfile(dto));
    }

    [HttpGet("catalog")]
    public IActionResult Catalog([FromServices] SafeSql sql, [FromServices] IWebHostEnvironment env)
    {
        try { return Ok(SafeCatalog.All(sql, Path.Combine(env.WebRootPath, "repair-catalog.json"))); }
        catch (Exception ex) { return StatusCode(503, new { error = ex.Message }); }
    }

    [HttpGet("repair-plan-settings")]
    public IActionResult PlanSettings() => Ok(_plan);

    [HttpGet("repair-plans")]
    public IActionResult Plans([FromQuery] string? realtor) => Ok(_store.Plans(realtor));

    [HttpPost("repair-plans")]
    public IActionResult SavePlan([FromBody] RepairPlanDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RealtorEmail)) return Bad("Sign in first.", "Primero inicia sesión.");
        if (string.IsNullOrWhiteSpace(dto.Address) || string.IsNullOrWhiteSpace(dto.ClientName))
            return Bad("Write the property address and the client.", "Escribe la dirección de la propiedad y el cliente.");
        if (!string.IsNullOrWhiteSpace(dto.ClientEmail) && !dto.ClientEmail.Contains('@'))
            return Bad("Check the client email.", "Revisa el correo del cliente.");
        var items = dto.Items ?? new();
        if (items.Count > 100) return Bad("A plan can have up to 100 repairs.", "Un plan puede tener hasta 100 reparaciones.");
        if (items.Any(i => string.IsNullOrWhiteSpace(i.Id) || i.Qty <= 0 || i.Qty > 10000))
            return Bad("Every repair needs a quantity greater than zero.", "Cada reparación necesita una cantidad mayor que cero.");
        if (dto.Status is not null && !PlanStatuses.Contains(dto.Status)) return Bad("Unknown plan status.", "Estado del plan no válido.");
        if (dto.HourlyRate < 0 || dto.MaxAuthorization < 0 || dto.Credit < 0) return Bad("Amounts cannot be negative.", "Los montos no pueden ser negativos.");
        return Ok(_store.SavePlan(dto));
    }
}

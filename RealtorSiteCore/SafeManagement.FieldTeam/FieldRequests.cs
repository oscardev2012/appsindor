using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;
using System.Text.Json;

namespace SafeManagement.FieldTeam;

public class FieldRequestStore
{
    private readonly SafeSql _sql;
    public FieldRequestStore(SafeSql sql) => _sql = sql;

    private static string Key(string? worker) => (worker ?? "").Trim().ToLowerInvariant();

    private static string Cut(string value, int max) => value.Length <= max ? value : value[..max];

    public object Save(WorkerRequestDto dto, List<WorkerRequestItem> items)
    {
        var total = items.Count == 0 ? (decimal?)null : items.Sum(i => (decimal)i.Price);
        using var db = _sql.Open();
        using var tx = db.BeginTransaction();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeFieldWorkerRequests (Worker, WorkerName, Kind, Address, ClientName, ClientPhone, Notes, RepairId, RepairName, Low, High, Status)
            OUTPUT INSERTED.Id
            VALUES (@worker, @workerName, @kind, @address, @client, @phone, @notes, @repairId, @repairName, @total, @total, N'Pending')
            """, tx)
            .With("@worker", Key(dto.Worker))
            .With("@workerName", dto.WorkerName ?? "")
            .With("@kind", items.Count == 0 ? "new_property" : "estimate")
            .With("@address", dto.Address!.Trim())
            .With("@client", dto.ClientName ?? "")
            .With("@phone", dto.ClientPhone ?? "")
            .With("@notes", Cut(dto.Notes ?? "", 2000))
            .With("@repairId", items.Count == 0 ? "" : Cut(items[0].RepairId!, 20))
            .With("@repairName", Cut(string.Join(", ", items.Select(i => i.RepairName)), 200))
            .With("@total", total);
        var id = (int)cmd.ExecuteScalar()!;
        foreach (var item in items)
        {
            using var line = db.Command("""
                INSERT INTO dbo.SafeFieldWorkerRequestItems (RequestId, RepairId, RepairName, Price)
                VALUES (@request, @repairId, @repairName, @price)
                """, tx)
                .With("@request", id)
                .With("@repairId", Cut(item.RepairId!.Trim(), 20))
                .With("@repairName", Cut(item.RepairName!.Trim(), 200))
                .With("@price", (decimal)item.Price);
            line.ExecuteNonQuery();
        }
        tx.Commit();
        return Requests(dto.Worker).First();
    }

    public List<object> Requests(string? worker)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT TOP 50 Id, Kind, Address, ClientName, ClientPhone, Notes, RepairId, RepairName, Low, High, Status, CreatedAt
            FROM dbo.SafeFieldWorkerRequests WHERE Worker = @worker ORDER BY Id DESC
            """).With("@worker", Key(worker));
        var rows = new List<(int Id, Dictionary<string, object?> Row)>();
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                rows.Add((r.GetInt32(0), new Dictionary<string, object?>
                {
                    ["id"] = r.GetInt32(0),
                    ["kind"] = r.GetString(1),
                    ["address"] = r.GetString(2),
                    ["clientName"] = r.Text(3),
                    ["clientPhone"] = r.Text(4),
                    ["notes"] = r.Text(5),
                    ["repairId"] = r.Text(6),
                    ["repairName"] = r.Text(7),
                    ["low"] = r.NumberOrNull(8),
                    ["high"] = r.NumberOrNull(9),
                    ["status"] = r.GetString(10),
                    ["createdAt"] = r.Iso(11)
                }));
        var items = FieldRequestItems.For(db, rows.Select(x => x.Id));
        foreach (var (id, row) in rows) row["items"] = items.TryGetValue(id, out var list) ? list : [];
        return rows.Select(x => (object)x.Row).ToList();
    }
}

public static class FieldRequestItems
{
    public static Dictionary<int, List<object>> For(Microsoft.Data.SqlClient.SqlConnection db, IEnumerable<int> ids)
    {
        var list = ids.ToList();
        var map = new Dictionary<int, List<object>>();
        if (list.Count == 0) return map;
        using var cmd = db.Command($"SELECT RequestId, RepairId, RepairName, Price FROM dbo.SafeFieldWorkerRequestItems WHERE RequestId IN ({string.Join(",", list)}) ORDER BY Id");
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            var key = r.GetInt32(0);
            if (!map.TryGetValue(key, out var lines)) map[key] = lines = [];
            lines.Add(new { repairId = r.GetString(1), repairName = r.GetString(2), price = r.Number(3) });
        }
        return map;
    }
}

public class FieldActivityStore
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private readonly SafeSql _sql;
    public FieldActivityStore(SafeSql sql) => _sql = sql;

    private static string Key(string? worker) => (worker ?? "").Trim().ToLowerInvariant();

    public object Save(WorkerActivityDto dto)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeFieldWorkerActivity (Worker, WorkerName, Kind, WorkOrderId, Address, Detail, Body, Photos)
            VALUES (@worker, @workerName, @kind, @wo, @address, @detail, @body, @photos)
            """)
            .With("@worker", Key(dto.Worker))
            .With("@workerName", dto.WorkerName ?? "")
            .With("@kind", dto.Kind)
            .With("@wo", dto.WorkOrderId ?? "")
            .With("@address", (dto.Address ?? "").Trim())
            .With("@detail", dto.Detail ?? "")
            .With("@body", dto.Body ?? "")
            .With("@photos", JsonSerializer.Serialize(dto.Photos ?? [], Json));
        cmd.ExecuteNonQuery();
        return Activity(dto.Worker).First();
    }

    public List<object> Activity(string? worker)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            SELECT TOP 100 Id, Kind, WorkOrderId, Address, Detail, Body, Photos, CreatedAt, WorkerName
            FROM dbo.SafeFieldWorkerActivity WHERE Worker = @worker ORDER BY Id DESC
            """).With("@worker", Key(worker));
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new
            {
                id = r.GetInt32(0),
                kind = r.GetString(1),
                workOrderId = r.Text(2),
                address = r.GetString(3),
                detail = r.Text(4),
                body = r.Text(5),
                photos = JsonSerializer.Deserialize<List<string>>(r.GetString(6), Json) ?? [],
                createdAt = r.Iso(7),
                author = r.Text(8)
            });
        return rows;
    }
}

public record WorkerActivityDto(string? Worker, string? WorkerName, string? Kind, string? WorkOrderId, string? Address, string? Detail, string? Body, List<string>? Photos);

[ApiController]
[Route("api/field")]
public class FieldActivityController : ControllerBase
{
    /* "reply" lo escribe Management desde su portal; aquí solo entran acciones del trabajador. */
    private static readonly HashSet<string> Kinds = ["checkin", "access", "photos", "change", "message"];
    private readonly FieldActivityStore _store;
    public FieldActivityController(FieldActivityStore store) => _store = store;

    [HttpGet("activity")]
    public IActionResult Activity([FromQuery] string? worker) => Ok(_store.Activity(worker));

    [HttpPost("activity")]
    [RequestSizeLimit(12_000_000)]
    public IActionResult Create([FromBody] WorkerActivityDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Worker)) return BadRequest(new { error = "Missing worker identity." });
        if (dto.Kind is null || !Kinds.Contains(dto.Kind)) return BadRequest(new { error = "Unknown action." });
        if (dto.Kind != "message" && (string.IsNullOrWhiteSpace(dto.Address) || dto.Address.Trim().Length < 5)) return BadRequest(new { error = "Write the property address." });
        if (dto.Kind == "photos")
        {
            if (dto.Photos is not { Count: > 0 } || dto.Photos.Count > 8) return BadRequest(new { error = "Add between 1 and 8 photos." });
            if (dto.Photos.Any(p => !p.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))) return BadRequest(new { error = "Photos must be images." });
        }
        if (dto.Kind == "checkin")
        {
            if (!DateTimeOffset.TryParse(dto.Detail, out var arrival)) return BadRequest(new { error = "Write the arrival time." });
            if (arrival > DateTimeOffset.UtcNow.AddMinutes(10)) return BadRequest(new { error = "The arrival time can't be in the future." });
            if (arrival < DateTimeOffset.UtcNow.AddDays(-7)) return BadRequest(new { error = "The arrival time is too old." });
        }
        if (dto.Kind is "access" or "change" or "message" && string.IsNullOrWhiteSpace(dto.Body)) return BadRequest(new { error = "Write the message for Management." });
        if ((dto.Body ?? "").Length > 2000) return BadRequest(new { error = "The message is too long." });
        return Ok(_store.Save(dto));
    }
}

public record WorkerRequestItem(string? RepairId, string? RepairName, double Price);
public record WorkerRequestDto(string? Worker, string? WorkerName, string? Address, string? ClientName, string? ClientPhone, string? Notes, List<WorkerRequestItem>? Items);

[ApiController]
[Route("api/field")]
public class FieldRequestsController : ControllerBase
{
    private readonly FieldRequestStore _store;
    public FieldRequestsController(FieldRequestStore store) => _store = store;

    [HttpGet("requests")]
    public IActionResult Requests([FromQuery] string? worker) => Ok(_store.Requests(worker));

    [HttpPost("requests")]
    public IActionResult Create([FromBody] WorkerRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Worker)) return BadRequest(new { error = "Missing worker identity." });
        if (string.IsNullOrWhiteSpace(dto.Address) || dto.Address.Trim().Length < 5) return BadRequest(new { error = "Write the property address." });
        if (!string.IsNullOrWhiteSpace(dto.ClientPhone) && !System.Text.RegularExpressions.Regex.IsMatch(dto.ClientPhone.Trim(), @"^\d{10}$"))
            return BadRequest(new { error = "The phone must have exactly 10 digits." });
        var items = (dto.Items ?? []).Where(i => !string.IsNullOrWhiteSpace(i.RepairId) && !string.IsNullOrWhiteSpace(i.RepairName)).ToList();
        if (items.Count > 30) return BadRequest(new { error = "A request can have up to 30 services." });
        if (items.Any(i => i.Price <= 0 || i.Price > 1_000_000)) return BadRequest(new { error = "Every service needs a final price above 0." });
        return Ok(_store.Save(dto, items));
    }
}

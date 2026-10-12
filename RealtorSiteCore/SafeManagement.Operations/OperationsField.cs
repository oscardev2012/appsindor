using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SafeManagement.Data;

namespace SafeManagement.Operations;

/* Lo que el Equipo de campo guarda en IndorDB (perfiles, solicitudes, actividad y mensajes privados), leído por Management. */
public class OperationsFieldStore
{
    public static readonly string[] RequestStatuses = ["Pending", "Reviewed", "Approved", "Declined", "Converted"];
    private readonly SafeSql _sql;
    public OperationsFieldStore(SafeSql sql) => _sql = sql;

    public object Inbox()
    {
        using var db = _sql.Open();
        var profiles = new List<object>();
        using (var cmd = db.Command("SELECT Email, Name, Company, Phone, Specialties, CreatedAt FROM dbo.SafeFieldWorkerProfiles ORDER BY Name"))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                profiles.Add(new
                {
                    email = r.GetString(0),
                    name = r.GetString(1),
                    company = r.Text(2),
                    phone = r.Text(3),
                    specialties = JsonSerializer.Deserialize<List<string>>(r.GetString(4)) ?? [],
                    createdAt = r.Iso(5)
                });

        var requests = new List<(int Id, Dictionary<string, object?> Row)>();
        using (var cmd = db.Command("""
            SELECT TOP 200 Id, Worker, WorkerName, Kind, Address, ClientName, ClientPhone, Notes, RepairName, Low, Status, CreatedAt
            FROM dbo.SafeFieldWorkerRequests ORDER BY Id DESC
            """))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                requests.Add((r.GetInt32(0), new Dictionary<string, object?>
                {
                    ["id"] = r.GetInt32(0),
                    ["worker"] = r.GetString(1),
                    ["workerName"] = r.Text(2),
                    ["kind"] = r.GetString(3),
                    ["address"] = r.GetString(4),
                    ["clientName"] = r.Text(5),
                    ["clientPhone"] = r.Text(6),
                    ["notes"] = r.Text(7),
                    ["repairName"] = r.Text(8),
                    ["total"] = r.NumberOrNull(9),
                    ["status"] = r.GetString(10),
                    ["createdAt"] = r.Iso(11)
                }));
        var items = Items(db, requests.Select(x => x.Id));
        foreach (var (id, row) in requests) row["items"] = items.TryGetValue(id, out var list) ? list : [];

        var activity = new List<object>();
        using (var cmd = db.Command("""
            SELECT TOP 300 Id, Worker, WorkerName, Kind, WorkOrderId, Address, Detail, Body,
                   CASE WHEN Photos = N'[]' THEN 0 ELSE (SELECT COUNT(*) FROM OPENJSON(Photos)) END, CreatedAt
            FROM dbo.SafeFieldWorkerActivity ORDER BY Id DESC
            """))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                activity.Add(new
                {
                    id = r.GetInt32(0),
                    worker = r.GetString(1),
                    workerName = r.Text(2),
                    kind = r.GetString(3),
                    workOrderId = r.Text(4),
                    address = r.Text(5),
                    detail = r.Text(6),
                    body = r.Text(7),
                    photoCount = r.GetInt32(8),
                    createdAt = r.Iso(9)
                });

        return new { profiles, requests = requests.Select(x => (object)x.Row), activity };
    }

    public List<string>? Photos(int id)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Photos FROM dbo.SafeFieldWorkerActivity WHERE Id = @id").With("@id", id);
        return cmd.ExecuteScalar() is string json ? JsonSerializer.Deserialize<List<string>>(json) ?? [] : null;
    }

    private static Dictionary<int, List<object>> Items(Microsoft.Data.SqlClient.SqlConnection db, IEnumerable<int> ids)
    {
        var list = ids.ToList();
        var map = new Dictionary<int, List<object>>();
        if (list.Count == 0) return map;
        using var cmd = db.Command($"SELECT RequestId, RepairId, RepairName, Price FROM dbo.SafeFieldWorkerRequestItems WHERE RequestId IN ({string.Join(",", list)}) ORDER BY Id");
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            if (!map.TryGetValue(r.GetInt32(0), out var lines)) map[r.GetInt32(0)] = lines = [];
            lines.Add(new { repairId = r.GetString(1), repairName = r.GetString(2), price = r.Number(3) });
        }
        return map;
    }

    public bool SetStatus(int id, string status)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("UPDATE dbo.SafeFieldWorkerRequests SET Status = @status WHERE Id = @id").With("@status", status).With("@id", id);
        return cmd.ExecuteNonQuery() > 0;
    }

    public (string Address, string Client, string Repairs, string Worker)? Request(int id)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT Address, ClientName, RepairName, WorkerName, Kind FROM dbo.SafeFieldWorkerRequests WHERE Id = @id").With("@id", id);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (r.GetString(0), r.Text(1), r.GetString(4) == "new_property" ? "New property" : r.Text(2), r.Text(3));
    }

    public void Reply(string worker, string author, string body)
    {
        using var db = _sql.Open();
        using var cmd = db.Command("""
            INSERT INTO dbo.SafeFieldWorkerActivity (Worker, WorkerName, Kind, Address, Body)
            VALUES (@worker, @author, N'reply', N'', @body)
            """)
            .With("@worker", worker.Trim().ToLowerInvariant())
            .With("@author", author.Trim())
            .With("@body", body.Trim());
        cmd.ExecuteNonQuery();
    }

    public List<object> RealtorGiftCards()
    {
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT TOP 200 Id, RealtorEmail, ClientName, Address, Firm, Amount, Code, CreatedAt FROM dbo.SafeRealtorGiftCards ORDER BY Id DESC");
        var rows = new List<object>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            rows.Add(new
            {
                id = r.GetInt32(0),
                realtorEmail = r.GetString(1),
                clientName = r.GetString(2),
                address = r.Text(3),
                firm = r.Text(4),
                amount = r.GetInt32(5),
                code = r.GetString(6),
                createdAt = r.Iso(7)
            });
        return rows;
    }
}

public record FieldStatusDto(string? Status);
public record FieldReplyDto(string? Worker, string? Author, string? Body);

[ApiController]
[Route("api/operations")]
public class OperationsFieldController : ControllerBase
{
    private readonly OperationsFieldStore _store;
    private readonly OperationsRequestStore _requests;
    public OperationsFieldController(OperationsFieldStore store, OperationsRequestStore requests)
    {
        _store = store;
        _requests = requests;
    }

    [HttpGet("field")]
    public IActionResult Inbox() => Ok(_store.Inbox());

    [HttpGet("field/activity/{id:int}/photos")]
    public IActionResult Photos(int id) => _store.Photos(id) is { } photos ? Ok(photos) : NotFound();

    [HttpPatch("field/requests/{id:int}")]
    public IActionResult SetStatus(int id, [FromBody] FieldStatusDto dto)
    {
        if (dto.Status is null || !OperationsFieldStore.RequestStatuses.Contains(dto.Status)) return BadRequest(new { error = "Unknown status." });
        return _store.SetStatus(id, dto.Status) ? Ok(new { id, status = dto.Status }) : NotFound();
    }

    [HttpPost("field/requests/{id:int}/convert")]
    public IActionResult Convert(int id)
    {
        if (_store.Request(id) is not { } field) return NotFound();
        var created = _requests.Save(new RequestDto(field.Address, "", string.IsNullOrWhiteSpace(field.Client) ? field.Worker : field.Client, "Field Team", field.Repairs, null));
        _store.SetStatus(id, "Converted");
        return Ok(created);
    }

    [HttpPost("field/messages")]
    public IActionResult Reply([FromBody] FieldReplyDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Worker)) return BadRequest(new { error = "Choose the worker." });
        if (string.IsNullOrWhiteSpace(dto.Body) || dto.Body.Length > 2000) return BadRequest(new { error = "Write the message (up to 2000 characters)." });
        _store.Reply(dto.Worker, string.IsNullOrWhiteSpace(dto.Author) ? "Management" : dto.Author, dto.Body);
        return Ok(new { ok = true });
    }

    [HttpGet("realtor-gift-cards")]
    public IActionResult RealtorGiftCards() => Ok(_store.RealtorGiftCards());
}

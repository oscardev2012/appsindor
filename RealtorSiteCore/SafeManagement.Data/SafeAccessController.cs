using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace SafeManagement.Data;

[ApiController]
[Route("api/safe/work-orders/{id}/access")]
public class SafeAccessController : ControllerBase
{
    private readonly SafeDbContext _db;
    private readonly SafeHubService _hub;
    private readonly SafeSql _sql;
    private readonly IHostEnvironment _env;

    public SafeAccessController(SafeDbContext db, SafeHubService hub, SafeSql sql, IHostEnvironment env)
    {
        _db = db;
        _hub = hub;
        _sql = sql;
        _env = env;
    }

    private bool IsOperations => _env.ApplicationName == "SafeManagement.Operations";
    private bool IsFieldTeam => _env.ApplicationName == "SafeManagement.FieldTeam";

    private static ObjectResult Bad(int status, string en, string es) => new(new { error = en, errorEs = es }) { StatusCode = status };

    private Task<SafeWorkOrder?> WorkOrder(string id)
        => _db.WorkOrders.FirstOrDefaultAsync(x => x.PublicId == id || x.Id.ToString() == id);

    [HttpPost]
    public async Task<IActionResult> Send(string id, [FromBody] SendAccessDto dto)
    {
        var instruction = (dto.Instruction ?? "").Trim();
        if (instruction.Length is 0 or > 500)
            return Bad(400, "Write the access instruction (up to 500 characters).", "Escribe la instrucción de acceso (hasta 500 caracteres).");
        var wo = await WorkOrder(id);
        if (wo is null) return Bad(404, "Work order not found.", "No se encontró la orden de trabajo.");
        if (wo.WorkerId is null)
            return Bad(409, "This job has no assigned worker yet.", "Este trabajo aún no tiene trabajador asignado.");
        var sender = (dto.SenderName ?? "").Trim();
        using (var db = _sql.Open())
        using (var cmd = db.Command("INSERT INTO dbo.SafeAccessInstructions (WorkOrderId, Instruction, SentBy) VALUES (@wo, @instruction, @sender)")
            .With("@wo", wo.Id)
            .With("@instruction", instruction)
            .With("@sender", sender.Length > 160 ? sender[..160] : sender))
        {
            cmd.ExecuteNonQuery();
        }
        var request = await _db.Requests.FindAsync(wo.RequestId);
        var notice = string.IsNullOrWhiteSpace(dto.Notice) ? "Secure access instruction sent · value hidden" : dto.Notice.Trim();
        await _hub.PostMessage(new CreateMessageDto(null, request?.PublicId ?? wo.RequestId.ToString(), "Realtor",
            string.IsNullOrWhiteSpace(sender) ? "Realtor" : sender, "job", notice.Length > 200 ? notice[..200] : notice, dto.SourceApp ?? "Ecosystem"));
        return Ok(new { ok = true, workOrderId = wo.PublicId });
    }

    [HttpGet]
    public async Task<IActionResult> View(string id, [FromQuery] string? worker)
    {
        if (!IsOperations && !IsFieldTeam)
            return Bad(403, "Only the assigned worker and Management can view this instruction.", "Solo el trabajador asignado y Management pueden ver esta instrucción.");
        var wo = await WorkOrder(id);
        if (wo is null) return Bad(404, "Work order not found.", "No se encontró la orden de trabajo.");
        if (IsFieldTeam)
        {
            var email = (worker ?? "").Trim();
            var assigned = wo.WorkerId is int wid ? await _db.Contacts.FindAsync(wid) : null;
            if (email.Length == 0 || assigned?.Email is null || !string.Equals(assigned.Email.Trim(), email, StringComparison.OrdinalIgnoreCase))
                return Bad(403, "Only the worker assigned to this job can view the access instruction.", "Solo el trabajador asignado a este trabajo puede ver la instrucción de acceso.");
        }
        using var db = _sql.Open();
        using var cmd = db.Command("SELECT TOP 1 Instruction, SentBy, CreatedAt FROM dbo.SafeAccessInstructions WHERE WorkOrderId = @wo ORDER BY Id DESC")
            .With("@wo", wo.Id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return Bad(404, "No access instruction has been sent for this job yet.", "Aún no se ha enviado instrucción de acceso para este trabajo.");
        return Ok(new { instruction = reader.GetString(0), sentBy = reader.Text(1), created = reader.Iso(2) });
    }
}

public record SendAccessDto(string? Instruction, string? SenderName, string? Notice, string? SourceApp);

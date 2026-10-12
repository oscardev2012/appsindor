using Microsoft.EntityFrameworkCore;

namespace MarketplaceSafe.Data;

public sealed class SafeDb(DbContextOptions<SafeDb> options) : DbContext(options)
{
    public DbSet<ClientRecord> Clients => Set<ClientRecord>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();
    public DbSet<MessageThread> Threads => Set<MessageThread>();
    public DbSet<ThreadMessage> Messages => Set<ThreadMessage>();
    public DbSet<GiftCard> Gifts => Set<GiftCard>();
    public DbSet<ReferralEarning> Referrals => Set<ReferralEarning>();
    public DbSet<ScheduleEvent> Events => Set<ScheduleEvent>();
    public DbSet<FieldJob> FieldJobs => Set<FieldJob>();
    public DbSet<RepairPlanRecord> Plans => Set<RepairPlanRecord>();
    public DbSet<WorkRequest> Requests => Set<WorkRequest>();
}

public sealed class ClientRecord
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string StageEn { get; set; } = "";
    public string StageEs { get; set; } = "";
    public string NextEn { get; set; } = "";
    public string NextEs { get; set; } = "";
    public string ValueLabel { get; set; } = "";
    public string ListKey { get; set; } = "";
}

public sealed class FollowUp
{
    public int Id { get; set; }
    public string Owner { get; set; } = "";
    public string TaskEn { get; set; } = "";
    public string TaskEs { get; set; } = "";
    public string DueEn { get; set; } = "";
    public string DueEs { get; set; } = "";
    public string Column { get; set; } = "today";
}

public sealed class MessageThread
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string PreviewEn { get; set; } = "";
    public string PreviewEs { get; set; } = "";
    public string WhenLabel { get; set; } = "";
    public string Address { get; set; } = "";
    public string Meta { get; set; } = "";
    public bool Active { get; set; }
}

public sealed class ThreadMessage
{
    public int Id { get; set; }
    public int ThreadId { get; set; }
    public string FromName { get; set; } = "";
    public string BodyEn { get; set; } = "";
    public string BodyEs { get; set; } = "";
    public bool FromMe { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class GiftCard
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string ClientName { get; set; } = "";
    public string Address { get; set; } = "";
    public string StatusEn { get; set; } = "";
    public string StatusEs { get; set; } = "";
}

public sealed class ReferralEarning
{
    public int Id { get; set; }
    public string ClientName { get; set; } = "";
    public string ServiceEn { get; set; } = "";
    public string ServiceEs { get; set; } = "";
    public decimal Sale { get; set; }
    public string Eligibility { get; set; } = "";
    public decimal? Earning { get; set; }
}

public sealed class ScheduleEvent
{
    public int Id { get; set; }
    public int Day { get; set; }
    public string DateLabel { get; set; } = "";
    public string Clock { get; set; } = "";
    public string TitleEn { get; set; } = "";
    public string TitleEs { get; set; } = "";
    public string Detail { get; set; } = "";
    public string PillEn { get; set; } = "";
    public string PillEs { get; set; } = "";
}

public sealed class FieldJob
{
    public int Id { get; set; }
    public string Technician { get; set; } = "";
    public string WorkOrder { get; set; } = "";
    public string DayLabel { get; set; } = "";
    public string TimeLabel { get; set; } = "";
    public string TitleEn { get; set; } = "";
    public string TitleEs { get; set; } = "";
    public string DetailEn { get; set; } = "";
    public string DetailEs { get; set; } = "";
    public int Sort { get; set; }
}

public sealed class RepairPlanRecord
{
    public int Id { get; set; }
    public string Address { get; set; } = "";
    public string ClientName { get; set; } = "";
    public string Stage { get; set; } = "";
    public string TargetClose { get; set; } = "";
    public string RealtorName { get; set; } = "";
    public string Goal { get; set; } = "";
    public string Status { get; set; } = "draft";
    public string Pricing { get; set; } = "fixed";
    public int Credit { get; set; }
    public string SelectedIds { get; set; } = "";
}

public sealed class WorkRequest
{
    public int Id { get; set; }
    public string Portal { get; set; } = "realtor";
    public string Address { get; set; } = "";
    public string ClientName { get; set; } = "";
    public string Notes { get; set; } = "";
    public string Status { get; set; } = "submitted";
    public DateTime CreatedAt { get; set; }
}

public static class WorkspaceApi
{
    public static void MapWorkspaceApi(this WebApplication app)
    {
        app.MapGet("/api/workspace", async (SafeDb db) => await Snapshot(db));

        app.MapPost("/api/followups/{id:int}/complete", async (int id, SafeDb db) =>
        {
            var row = await db.FollowUps.FindAsync(id);
            if (row is null) return Results.NotFound();
            row.Column = "done";
            await db.SaveChangesAsync();
            return Results.Ok(await Snapshot(db));
        });

        app.MapPost("/api/messages", async (MessageInput input, SafeDb db) =>
        {
            var thread = await db.Threads.FindAsync(input.ThreadId);
            if (thread is null) return Results.NotFound();
            var body = (input.Body ?? "").Trim();
            if (body.Length == 0) return Results.BadRequest();
            db.Messages.Add(new ThreadMessage
            {
                ThreadId = thread.Id,
                FromName = "Realtor Partner",
                BodyEn = body,
                BodyEs = body,
                FromMe = true,
                CreatedAt = DateTime.UtcNow
            });
            thread.PreviewEn = body;
            thread.PreviewEs = body;
            thread.WhenLabel = "Now";
            await db.SaveChangesAsync();
            return Results.Ok(await Snapshot(db));
        });

        app.MapPost("/api/requests", async (RequestInput input, SafeDb db) =>
        {
            var address = (input.Address ?? "").Trim();
            if (address.Length == 0) return Results.BadRequest();
            var client = (input.Client ?? "").Trim();
            var notes = (input.Notes ?? "").Trim();
            db.Requests.Add(new WorkRequest
            {
                Portal = string.IsNullOrWhiteSpace(input.Portal) ? "realtor" : input.Portal.Trim(),
                Address = address,
                ClientName = client,
                Notes = notes,
                Status = "submitted",
                CreatedAt = DateTime.UtcNow
            });
            db.FollowUps.Add(new FollowUp
            {
                Owner = string.IsNullOrWhiteSpace(client) ? address : client,
                TaskEn = string.IsNullOrWhiteSpace(notes) ? "New SAFE work request" : notes,
                TaskEs = string.IsNullOrWhiteSpace(notes) ? "Nueva solicitud de trabajo SAFE" : notes,
                DueEn = "Today",
                DueEs = "Hoy",
                Column = "today"
            });
            await db.SaveChangesAsync();
            return Results.Ok(await Snapshot(db));
        });

        app.MapPost("/api/clients", async (ClientInput input, SafeDb db) =>
        {
            var name = (input.Name ?? "").Trim();
            var address = (input.Address ?? "").Trim();
            if (name.Length == 0 || address.Length == 0) return Results.BadRequest();
            db.Clients.Add(new ClientRecord
            {
                Name = name,
                Address = address,
                StageEn = "New client",
                StageEs = "Cliente nuevo",
                NextEn = "Add the next follow-up",
                NextEs = "Agregar el próximo seguimiento",
                ValueLabel = "—",
                ListKey = "closing"
            });
            await db.SaveChangesAsync();
            return Results.Ok(await Snapshot(db));
        });

        app.MapPost("/api/gifts", async (GiftInput input, SafeDb db) =>
        {
            var name = (input.ClientName ?? "").Trim();
            var address = (input.Address ?? "").Trim();
            if (name.Length == 0) return Results.BadRequest();
            db.Gifts.Add(new GiftCard
            {
                Amount = input.Amount <= 0 ? 50 : input.Amount,
                ClientName = name,
                Address = address,
                StatusEn = "Issued",
                StatusEs = "Emitida"
            });
            await db.SaveChangesAsync();
            return Results.Ok(await Snapshot(db));
        });

        app.MapPut("/api/plan", async (PlanInput input, SafeDb db) =>
        {
            var plan = await db.Plans.OrderBy(x => x.Id).FirstOrDefaultAsync();
            if (plan is null)
            {
                plan = new RepairPlanRecord();
                db.Plans.Add(plan);
            }
            if (input.Address is not null) plan.Address = input.Address.Trim();
            if (input.ClientName is not null) plan.ClientName = input.ClientName.Trim();
            if (input.Stage is not null) plan.Stage = input.Stage;
            if (input.TargetClose is not null) plan.TargetClose = input.TargetClose;
            if (input.RealtorName is not null) plan.RealtorName = input.RealtorName.Trim();
            if (input.Goal is not null) plan.Goal = input.Goal;
            if (input.Status is not null) plan.Status = input.Status;
            if (input.Pricing is not null) plan.Pricing = input.Pricing;
            if (input.Credit is not null) plan.Credit = input.Credit.Value;
            if (input.SelectedIds is not null) plan.SelectedIds = string.Join(",", input.SelectedIds);
            await db.SaveChangesAsync();
            return Results.Ok(await Snapshot(db));
        });
    }

    public static async Task Seed(SafeDb db)
    {
        if (await db.FollowUps.AnyAsync()) return;

        db.Clients.AddRange(
            new ClientRecord { Name = "Manuela Ruiz", Address = "1022 Lake Forest Dr", StageEn = "Estimate ready", StageEs = "Estimado listo", NextEn = "Today", NextEs = "Hoy", ValueLabel = "$6,450", ListKey = "options" },
            new ClientRecord { Name = "Esperanza Guandique", Address = "4515 Fox Thorne Ln", StageEn = "Work scheduled", StageEs = "Trabajo programado", NextEn = "Gift card due", NextEs = "Gift card pendiente", ValueLabel = "$9,062", ListKey = "closing" },
            new ClientRecord { Name = "Michael Grant", Address = "88 Red Tail Ct", StageEn = "New homeowner", StageEs = "Nuevo propietario", NextEn = "Oct 12 · Winter care", NextEs = "Oct 12 · Cuidado de invierno", ValueLabel = "—", ListKey = "paint" },
            new ClientRecord { Name = "Sarah Collins", Address = "7225 McTeal Pl", StageEn = "Inspection review", StageEs = "Revisión inspección", NextEn = "Tomorrow 10:00 AM", NextEs = "Mañana 10:00 AM", ValueLabel = "$5,800", ListKey = "diligence" }
        );
        db.FollowUps.AddRange(
            new FollowUp { Owner = "Manuela Ruiz", TaskEn = "Send the three-option flooring estimate.", TaskEs = "Enviar estimado de tres opciones.", DueEn = "Today · 4:00 PM", DueEs = "Hoy · 4:00 PM", Column = "today" },
            new FollowUp { Owner = "Esperanza Guandique", TaskEn = "Create the closing gift card.", TaskEs = "Crear gift card de cierre.", DueEn = "Today", DueEs = "Hoy", Column = "today" },
            new FollowUp { Owner = "Sarah Collins", TaskEn = "Ask whether buyer wants a SAFE site visit.", TaskEs = "Preguntar si desea visita SAFE.", DueEn = "Tomorrow", DueEs = "Mañana", Column = "next" },
            new FollowUp { Owner = "Michael Grant", TaskEn = "Send winter homeowner maintenance tips.", TaskEs = "Enviar tips de mantenimiento de invierno.", DueEn = "Oct 12", DueEs = "Oct 12", Column = "next" },
            new FollowUp { Owner = "Partner account", TaskEn = "Referral activity review completed.", TaskEs = "Revisión de actividad de referidos terminada.", DueEn = "Yesterday", DueEs = "Ayer", Column = "done" }
        );
        db.Threads.AddRange(
            new MessageThread { Title = "WO-2026-1047", PreviewEn = "Worker arrived · access requested", PreviewEs = "Trabajador llegó · solicita acceso", WhenLabel = "Now", Address = "1022 Lake Forest Dr", Meta = "REQ-1047 · WO-2026-1047 · Gabriel + Realtor + Management", Active = true },
            new MessageThread { Title = "Manuela Ruiz", PreviewEn = "I reviewed the flooring options…", PreviewEs = "Revisé las opciones del piso…", WhenLabel = "9:42", Address = "1022 Lake Forest Dr", Meta = "Manuela Ruiz" },
            new MessageThread { Title = "Esperanza Guandique", PreviewEn = "Thank you for the update.", PreviewEs = "Gracias por la actualización.", WhenLabel = "Yesterday", Address = "4515 Fox Thorne Ln", Meta = "Esperanza Guandique" }
        );
        db.Gifts.AddRange(
            new GiftCard { Amount = 250, ClientName = "Esperanza Guandique", Address = "4515 Fox Thorne Ln", StatusEn = "Redeemed · Sep 23", StatusEs = "Usada · Sep 23" },
            new GiftCard { Amount = 50, ClientName = "Michael Grant", Address = "88 Red Tail Ct", StatusEn = "Delivered · Sep 18", StatusEs = "Entregada · Sep 18" },
            new GiftCard { Amount = 100, ClientName = "Sarah Collins", Address = "7225 McTeal Pl", StatusEn = "Draft · logo required", StatusEs = "Borrador · falta logo" }
        );
        db.Referrals.AddRange(
            new ReferralEarning { ClientName = "Michael Grant", ServiceEn = "Water heater", ServiceEs = "Water heater", Sale = 1490, Eligibility = "eligible", Earning = 44.70m },
            new ReferralEarning { ClientName = "Sarah Collins", ServiceEn = "Crawlspace makeover", ServiceEs = "Crawlspace makeover", Sale = 3500, Eligibility = "pending", Earning = 105 },
            new ReferralEarning { ClientName = "Esperanza Guandique", ServiceEn = "Closing repairs", ServiceEs = "Reparaciones de cierre", Sale = 9062, Eligibility = "gift-only", Earning = null }
        );
        db.Events.AddRange(
            new ScheduleEvent { Day = 2, DateLabel = "OCT 02", Clock = "10:00", TitleEn = "Fall home checkup", TitleEs = "Revisión de otoño", Detail = "18 closed clients · Gmail", PillEn = "Fall tips · 10:00", PillEs = "Tips otoño · 10:00" },
            new ScheduleEvent { Day = 8, DateLabel = "OCT 08", Clock = "9:00", TitleEn = "Estimate follow-up", TitleEs = "Seguimiento estimado", Detail = "Manuela Ruiz · Gmail", PillEn = "2 follow-ups", PillEs = "2 seguimientos" },
            new ScheduleEvent { Day = 12, DateLabel = "OCT 12", Clock = "11:30", TitleEn = "Home anniversary", TitleEs = "Aniversario de casa", Detail = "Michael Grant · SMS", PillEn = "Home anniversary", PillEs = "Aniversario casa" },
            new ScheduleEvent { Day = 20, DateLabel = "OCT 20", Clock = "3:00", TitleEn = "Inspection decision", TitleEs = "Decisión inspección", Detail = "Sarah Collins · Call", PillEn = "Inspection review", PillEs = "Revisión inspección" }
        );
        db.FieldJobs.AddRange(
            new FieldJob { Technician = "Gabriel", WorkOrder = "WO-2026-1047", DayLabel = "Tue Oct 6", TimeLabel = "8:00", TitleEn = "123 Main Street", TitleEs = "123 Main Street", DetailEn = "Electrical + handyman scope · 5 tasks", DetailEs = "Alcance eléctrico + handyman · 5 tareas", Sort = 1 },
            new FieldJob { Technician = "Gabriel", WorkOrder = "WO-2026-1047", DayLabel = "Tue Oct 6", TimeLabel = "12:30", TitleEn = "Material / lunch buffer", TitleEs = "Materiales / almuerzo", DetailEn = "30 min", DetailEs = "30 min", Sort = 2 },
            new FieldJob { Technician = "Gabriel", WorkOrder = "WO-2026-1047", DayLabel = "Tue Oct 6", TimeLabel = "1:00", TitleEn = "816 Sharon Dr", TitleEs = "816 Sharon Dr", DetailEn = "Garbage disposal service", DetailEs = "Servicio garbage disposal", Sort = 3 }
        );
        db.Plans.Add(new RepairPlanRecord
        {
            Address = "123 Main Street, Charlotte NC 28277",
            ClientName = "John & Maria Smith",
            Stage = "Under contract / due diligence",
            TargetClose = "2026-10-23",
            RealtorName = "Realtor Partner",
            Goal = "Buyer repair negotiation",
            Status = "draft",
            Pricing = "fixed",
            Credit = 100,
            SelectedIds = "RP-01,RP-02,RP-03,RP-05,RP-06"
        });
        await db.SaveChangesAsync();

        var thread = await db.Threads.OrderBy(x => x.Id).FirstAsync();
        db.Messages.AddRange(
            new ThreadMessage { ThreadId = thread.Id, FromName = "Management", BodyEn = "The approved work order was assigned for today at 9:00 AM.", BodyEs = "La orden aprobada fue asignada para hoy a las 9:00 AM.", CreatedAt = DateTime.UtcNow.AddHours(-2) },
            new ThreadMessage { ThreadId = thread.Id, FromName = "Field worker", BodyEn = "I arrived at the property. Please send the access instructions.", BodyEs = "Ya llegué a la propiedad. Por favor envíen las instrucciones de acceso.", CreatedAt = DateTime.UtcNow.AddHours(-1) },
            new ThreadMessage { ThreadId = thread.Id, FromName = "Realtor Partner", BodyEn = "I received the arrival notification. I am sending the secure access information now.", BodyEs = "Recibí la notificación de llegada. Estoy enviando la información segura de acceso.", FromMe = true, CreatedAt = DateTime.UtcNow.AddMinutes(-30) }
        );
        await db.SaveChangesAsync();
    }

    private static async Task<object> Snapshot(SafeDb db)
    {
        var referrals = await db.Referrals.OrderBy(x => x.Id).ToListAsync();
        var plan = await db.Plans.OrderBy(x => x.Id).FirstOrDefaultAsync();
        return new
        {
            clients = await db.Clients.OrderBy(x => x.Id).ToListAsync(),
            followUps = await db.FollowUps.OrderBy(x => x.Id).ToListAsync(),
            threads = await db.Threads.OrderBy(x => x.Id).ToListAsync(),
            messages = await db.Messages.OrderBy(x => x.CreatedAt).ToListAsync(),
            gifts = await db.Gifts.OrderByDescending(x => x.Id).ToListAsync(),
            referrals,
            events = await db.Events.OrderBy(x => x.Day).ToListAsync(),
            fieldJobs = await db.FieldJobs.OrderBy(x => x.Sort).ToListAsync(),
            requests = await db.Requests.OrderByDescending(x => x.CreatedAt).ToListAsync(),
            plan = plan is null ? null : new
            {
                plan.Id,
                plan.Address,
                plan.ClientName,
                plan.Stage,
                plan.TargetClose,
                plan.RealtorName,
                plan.Goal,
                plan.Status,
                plan.Pricing,
                plan.Credit,
                selectedIds = plan.SelectedIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            },
            stats = new
            {
                available = referrals.Where(x => x.Eligibility == "eligible").Sum(x => x.Earning ?? 0),
                pending = referrals.Where(x => x.Eligibility == "pending").Sum(x => x.Earning ?? 0),
                sales = referrals.Where(x => x.Eligibility != "gift-only").Sum(x => x.Sale)
            }
        };
    }
}

public sealed record MessageInput(int ThreadId, string? Body);
public sealed record RequestInput(string? Address, string? Client, string? Notes, string? Portal);
public sealed record ClientInput(string? Name, string? Address);
public sealed record GiftInput(string? ClientName, string? Address, decimal Amount);
public sealed record PlanInput(string? Address, string? ClientName, string? Stage, string? TargetClose, string? RealtorName, string? Goal, string? Status, string? Pricing, int? Credit, List<string>? SelectedIds);

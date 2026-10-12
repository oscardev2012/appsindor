using Microsoft.EntityFrameworkCore;

namespace SafeManagement.Data;

public class SafeHubService
{
    private readonly SafeDbContext _db;
    public SafeHubService(SafeDbContext db) => _db = db;

    public async Task<object> Snapshot(string portal)
    {
        var requests = await _db.Requests.OrderByDescending(x => x.Id).Take(100).ToListAsync();
        var workOrders = await _db.WorkOrders.OrderByDescending(x => x.Id).Take(100).ToListAsync();
        var contacts = await _db.Contacts.OrderBy(x => x.Id).ToListAsync();
        var properties = await _db.Properties.OrderBy(x => x.Id).ToListAsync();
        var promotions = await _db.Promotions.OrderByDescending(x => x.Id).ToListAsync();
        var campaigns = await _db.Campaigns.OrderByDescending(x => x.Id).ToListAsync();
        var leads = await _db.Leads.OrderByDescending(x => x.Id).ToListAsync();
        var messages = await _db.Messages.OrderBy(x => x.Id).Take(200).ToListAsync();
        var threads = await _db.Threads.ToListAsync();
        var invoices = await _db.Invoices.OrderByDescending(x => x.Id).Take(50).ToListAsync();
        var estimates = await _db.Estimates.OrderByDescending(x => x.Id).Take(50).ToListAsync();
        var events = await _db.Events.OrderByDescending(x => x.Id).Take(40).ToListAsync();
        var giftCards = await _db.GiftCards.OrderByDescending(x => x.Id).Take(50).ToListAsync();
        var workerName = (int? id) => contacts.FirstOrDefault(c => c.Id == id)?.Name ?? "Unassigned";

        return new
        {
            portal,
            database = "IndorDB",
            requests = requests.Select(MapRequest),
            workOrders = workOrders.Select(w =>
            {
                var req = requests.FirstOrDefault(x => x.Id == w.RequestId);
                var thread = threads.FirstOrDefault(t => t.WorkOrderId == w.Id || t.RequestId == w.RequestId);
                return new
                {
                    id = w.PublicId,
                    dbId = w.Id,
                    requestId = req?.PublicId,
                    address = req?.Address,
                    city = req?.City,
                    client = req?.ClientName,
                    contact = req?.ContactName,
                    value = req?.Value ?? 0,
                    status = w.Status,
                    progress = w.Progress,
                    scope = w.Scope ?? req?.Type,
                    worker = workerName(w.WorkerId),
                    workerId = w.WorkerId,
                    checkInAt = w.CheckInAt,
                    threadId = thread?.PublicId
                };
            }),
            contacts = contacts.Select(c => new { c.Id, c.PublicId, c.Role, c.Name, c.Email, c.Phone, c.Firm }),
            properties = properties.Select(p => new { p.Id, p.PublicId, p.ClientId, p.RealtorId, p.Address, p.City, p.Status }),
            promotions = promotions.Select(p => new { p.Id, p.PublicId, p.Title, p.Offer, p.Body, p.Status }),
            campaigns = campaigns.Select(c => new { c.Id, c.PublicId, c.Title, c.Body, c.Status, c.RealtorId }),
            leads = leads.Select(l => new { l.Id, l.PublicId, l.Name, l.Channel, l.Stage, l.Notes }),
            messages = messages.Select(m => new { m.Id, threadId = m.ThreadId, m.SenderRole, m.SenderName, m.Channel, m.Body, created = m.CreatedAt }),
            threads = threads.Select(t => new { t.Id, t.PublicId, t.RequestId, t.WorkOrderId }),
            invoices = invoices.Select(i => new { i.Id, i.PublicId, i.Total, i.Status, i.RequestId }),
            estimates = estimates.Select(e => new { e.Id, e.PublicId, e.Total, e.Status, e.RequestId, e.RangeText }),
            giftCards = giftCards.Select(g => new { g.Id, g.PublicId, g.ClientName, g.Address, g.Firm, g.Amount, g.Code, g.Status }),
            events = events.Select(e => new { e.Id, e.SourceApp, e.EventName, e.RequestId, e.WorkOrderId, e.Payload, created = e.CreatedAt })
        };
    }

    public async Task<object> CreateRequest(CreateRequestDto dto)
    {
        var n = await _db.Requests.CountAsync() + 1048;
        var row = new SafeRequest
        {
            PublicId = string.IsNullOrWhiteSpace(dto.Id) ? $"REQ-{n}" : dto.Id!,
            SourceApp = string.IsNullOrWhiteSpace(dto.SourceApp) ? "Ecosystem" : dto.SourceApp!,
            Source = dto.Source ?? "Realtor Portal",
            Type = dto.Type ?? "Management review",
            Status = "New",
            Priority = dto.Priority ?? "medium",
            Owner = "Unassigned",
            ContactName = dto.Contact,
            ClientName = dto.Client,
            Address = dto.Address,
            City = dto.City ?? "Charlotte Area",
            Notes = dto.Notes,
            Items = dto.Items,
            Value = dto.Value,
            DueAt = dto.DueAt,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _db.Requests.Add(row);
        await _db.SaveChangesAsync();

        _db.Threads.Add(new SafeThread
        {
            PublicId = $"THR-{row.PublicId}",
            RequestId = row.Id,
            CreatedAt = DateTime.UtcNow
        });
        await Log(row.SourceApp, "realtor_request.submitted", row.Id, null, row.Address);
        await _db.SaveChangesAsync();

        var wo = new SafeWorkOrder
        {
            PublicId = $"WO-{DateTime.UtcNow:yyyy}-{1040 + await _db.WorkOrders.CountAsync()}",
            RequestId = row.Id,
            Status = "pending",
            Scope = row.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _db.WorkOrders.Add(wo);
        if (row.Value > 0)
        {
            _db.Estimates.Add(new SafeEstimate
            {
                PublicId = $"EST-{DateTime.UtcNow:yyyy}-{row.Id:0000}",
                RequestId = row.Id,
                Status = "Draft",
                Total = row.Value,
                CreatedAt = DateTime.UtcNow
            });
        }
        await _db.SaveChangesAsync();
        var thread = await _db.Threads.FirstAsync(t => t.RequestId == row.Id);
        thread.WorkOrderId = wo.Id;
        await _db.SaveChangesAsync();
        return MapRequest(row);
    }

    public async Task<object?> UpdateRequest(string publicId, UpdateRequestDto dto)
    {
        var row = await _db.Requests.FirstOrDefaultAsync(x => x.PublicId == publicId || x.Id.ToString() == publicId);
        if (row is null) return null;
        if (!string.IsNullOrWhiteSpace(dto.Status)) row.Status = dto.Status!;
        if (!string.IsNullOrWhiteSpace(dto.Owner)) row.Owner = dto.Owner;
        if (!string.IsNullOrWhiteSpace(dto.Priority)) row.Priority = dto.Priority;
        if (dto.Value.HasValue) row.Value = dto.Value.Value;
        row.UpdatedAt = DateTime.UtcNow;
        await Log("Operations", "request.updated", row.Id, null, row.Status);
        await _db.SaveChangesAsync();
        return MapRequest(row);
    }

    public async Task<object> CreateWorkOrder(CreateWorkOrderDto dto)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(x => x.PublicId == dto.RequestId || x.Id.ToString() == dto.RequestId)
            ?? throw new InvalidOperationException("Request not found");
        var n = await _db.WorkOrders.CountAsync();
        var wo = new SafeWorkOrder
        {
            PublicId = $"WO-{DateTime.UtcNow:yyyy}-{1040 + n}",
            RequestId = request.Id,
            WorkerId = dto.WorkerId,
            Status = "assigned",
            Scope = dto.Scope ?? request.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _db.WorkOrders.Add(wo);
        request.Status = "Ready to assign";
        request.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.RequestId == request.Id);
        if (thread is not null)
        {
            thread.WorkOrderId = wo.Id;
        }
        await Log("Operations", "work_order.created", request.Id, wo.Id, wo.PublicId);
        await _db.SaveChangesAsync();
        return MapWorkOrder(wo, new[] { request });
    }

    public async Task<object?> CheckIn(string workOrderId)
    {
        var wo = await _db.WorkOrders.FirstOrDefaultAsync(x => x.PublicId == workOrderId || x.Id.ToString() == workOrderId);
        if (wo is null) return null;
        wo.CheckInAt = DateTime.UtcNow;
        wo.Status = "arrived";
        wo.UpdatedAt = DateTime.UtcNow;
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.WorkOrderId == wo.Id || t.RequestId == wo.RequestId);
        if (thread is not null)
        {
            _db.Messages.Add(new SafeMessage
            {
                ThreadId = thread.Id,
                SenderRole = "Worker",
                SenderName = wo.WorkerId is int wid ? (await _db.Contacts.FindAsync(wid))?.Name ?? "Worker" : "Worker",
                Channel = "job",
                Body = "I arrived at the property. Please send the access instructions.",
                CreatedAt = DateTime.UtcNow
            });
        }
        await Log("Core", "worker.checked_in", wo.RequestId, wo.Id, wo.PublicId);
        await _db.SaveChangesAsync();
        var request = await _db.Requests.FindAsync(wo.RequestId);
        return MapWorkOrder(wo, request is null ? Array.Empty<SafeRequest>() : new[] { request });
    }

    public async Task<object?> UpdateWorkOrder(string workOrderId, UpdateWorkOrderDto dto)
    {
        var wo = await _db.WorkOrders.FirstOrDefaultAsync(x => x.PublicId == workOrderId || x.Id.ToString() == workOrderId);
        if (wo is null) return null;
        if (dto.WorkerId.HasValue) wo.WorkerId = dto.WorkerId;
        if (!string.IsNullOrWhiteSpace(dto.Status)) wo.Status = dto.Status!;
        if (dto.Progress.HasValue) wo.Progress = Math.Clamp(dto.Progress.Value, 0, 100);
        if (!string.IsNullOrWhiteSpace(dto.Scope)) wo.Scope = dto.Scope;
        wo.UpdatedAt = DateTime.UtcNow;
        if (dto.WorkerId.HasValue && string.IsNullOrWhiteSpace(dto.Status)) wo.Status = "assigned";
        await Log("Operations", "work_order.updated", wo.RequestId, wo.Id, wo.Status);
        await _db.SaveChangesAsync();
        var request = await _db.Requests.FindAsync(wo.RequestId);
        return MapWorkOrder(wo, request is null ? Array.Empty<SafeRequest>() : new[] { request });
    }

    public async Task<object> PostMessage(CreateMessageDto dto)
    {
        var thread = await _db.Threads.FirstOrDefaultAsync(t =>
            t.PublicId == dto.ThreadId || t.Id.ToString() == dto.ThreadId);
        if (thread is null && !string.IsNullOrWhiteSpace(dto.RequestId))
        {
            var req = await _db.Requests.FirstOrDefaultAsync(x => x.PublicId == dto.RequestId || x.Id.ToString() == dto.RequestId);
            if (req is not null)
                thread = await _db.Threads.FirstOrDefaultAsync(t => t.RequestId == req.Id);
        }
        if (thread is null && !string.IsNullOrWhiteSpace(dto.RequestId))
        {
            var req = await _db.Requests.FirstOrDefaultAsync(x => x.PublicId == dto.RequestId || x.Id.ToString() == dto.RequestId);
            if (req is not null)
            {
                thread = new SafeThread { PublicId = $"THR-{req.PublicId}", RequestId = req.Id, CreatedAt = DateTime.UtcNow };
                _db.Threads.Add(thread);
                await _db.SaveChangesAsync();
            }
        }
        if (thread is null) throw new InvalidOperationException("Thread not found");
        var msg = new SafeMessage
        {
            ThreadId = thread.Id,
            SenderRole = dto.SenderRole ?? "Operations",
            SenderName = dto.SenderName ?? "SAFE",
            Channel = dto.Channel ?? "job",
            Body = dto.Body,
            CreatedAt = DateTime.UtcNow
        };
        _db.Messages.Add(msg);
        await Log(dto.SourceApp ?? "Operations", "job_message.sent", thread.RequestId, thread.WorkOrderId, dto.Body);
        await _db.SaveChangesAsync();
        return new { msg.Id, msg.ThreadId, msg.SenderRole, msg.SenderName, msg.Channel, msg.Body, created = msg.CreatedAt };
    }

    public async Task<object> CreatePromotion(CreatePromotionDto dto)
    {
        var row = new SafePromotion
        {
            PublicId = $"PROMO-{DateTime.UtcNow:HHmmss}",
            Title = dto.Title,
            Offer = dto.Offer,
            Body = dto.Body,
            Status = dto.Status ?? "Draft",
            CreatedAt = DateTime.UtcNow
        };
        _db.Promotions.Add(row);
        await Log("Operations", "promotion.created", null, null, row.Title);
        await _db.SaveChangesAsync();
        return new { row.Id, row.PublicId, row.Title, row.Offer, row.Body, row.Status };
    }

    public async Task<object> CreateCampaign(CreateCampaignDto dto)
    {
        var row = new SafeCampaign
        {
            PublicId = $"CAMP-{DateTime.UtcNow:HHmmss}",
            Title = dto.Title,
            Body = dto.Body,
            Status = dto.Status ?? "Draft",
            CreatedAt = DateTime.UtcNow
        };
        _db.Campaigns.Add(row);
        await Log("Ecosystem", "campaign.created", null, null, row.Title);
        await _db.SaveChangesAsync();
        return new { row.Id, row.PublicId, row.Title, row.Body, row.Status };
    }

    public async Task<object> CreateLead(CreateLeadDto dto)
    {
        var row = new SafeLead
        {
            PublicId = $"LEAD-{DateTime.UtcNow:HHmmss}",
            Name = dto.Name,
            Channel = dto.Channel,
            Stage = dto.Stage ?? "New",
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        _db.Leads.Add(row);
        await Log("Operations", "lead.created", null, null, row.Name);
        await _db.SaveChangesAsync();
        return new { row.Id, row.PublicId, row.Name, row.Channel, row.Stage, row.Notes };
    }

    public async Task<object> CreateClient(CreateClientDto dto)
    {
        var n = await _db.Contacts.CountAsync() + 1;
        var role = string.IsNullOrWhiteSpace(dto.Role) ? "Client" : dto.Role!;
        var prefix = role.Equals("Worker", StringComparison.OrdinalIgnoreCase) ? "WRK"
            : role.Equals("Realtor", StringComparison.OrdinalIgnoreCase) ? "REL"
            : role.Equals("Operations", StringComparison.OrdinalIgnoreCase) ? "OPS" : "CLI";
        var contact = new SafeContact
        {
            PublicId = $"CON-{prefix}-{n:00}",
            Role = role,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Firm = dto.Firm,
            CreatedAt = DateTime.UtcNow
        };
        _db.Contacts.Add(contact);
        await _db.SaveChangesAsync();
        if (!string.IsNullOrWhiteSpace(dto.Address))
        {
            _db.Properties.Add(new SafeProperty
            {
                PublicId = $"PROP-{n:0000}",
                ClientId = contact.Id,
                Address = dto.Address!,
                City = dto.City ?? "Charlotte, NC",
                Status = "New",
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }
        await Log("Core", "client.created", null, null, contact.Name);
        return new { contact.Id, contact.PublicId, contact.Role, contact.Name, contact.Email, contact.Phone, contact.Firm };
    }

    public async Task<object> CreateGiftCard(CreateGiftCardDto dto)
    {
        var row = new SafeGiftCard
        {
            PublicId = $"GIFT-{DateTime.UtcNow:HHmmss}",
            ClientName = dto.ClientName,
            Address = dto.Address,
            Firm = dto.Firm,
            Amount = dto.Amount,
            Code = string.IsNullOrWhiteSpace(dto.Code) ? $"SAFE-{dto.Amount}" : dto.Code,
            Status = dto.Status ?? "Issued",
            CreatedAt = DateTime.UtcNow
        };
        _db.GiftCards.Add(row);
        await Log("Ecosystem", "gift_card.created", null, null, row.ClientName);
        await _db.SaveChangesAsync();
        return new { row.Id, row.PublicId, row.ClientName, row.Address, row.Amount, row.Code, row.Status };
    }

    private async Task Log(string app, string name, int? requestId, int? workOrderId, string? payload)
    {
        _db.Events.Add(new SafeEvent
        {
            SourceApp = app,
            EventName = name,
            RequestId = requestId,
            WorkOrderId = workOrderId,
            Payload = payload,
            CreatedAt = DateTime.UtcNow
        });
        await Task.CompletedTask;
    }

    private static object MapRequest(SafeRequest r) => new
    {
        id = r.PublicId,
        dbId = r.Id,
        address = r.Address,
        city = r.City,
        contact = r.ContactName,
        client = r.ClientName,
        source = r.Source,
        sourceApp = r.SourceApp,
        type = r.Type,
        status = r.Status,
        priority = r.Priority ?? "medium",
        due = r.DueAt?.ToString("MMM d") ?? "",
        items = r.Items,
        value = r.Value,
        owner = r.Owner,
        notes = r.Notes
    };

    private static object MapWorkOrder(SafeWorkOrder w, IEnumerable<SafeRequest> requests)
    {
        var req = requests.FirstOrDefault(x => x.Id == w.RequestId);
        return new
        {
            id = w.PublicId,
            dbId = w.Id,
            requestId = req?.PublicId,
            address = req?.Address,
            status = w.Status,
            progress = w.Progress,
            scope = w.Scope,
            checkInAt = w.CheckInAt
        };
    }
}

public record CreateRequestDto(
    string Address,
    string? Id,
    string? SourceApp,
    string? Source,
    string? Type,
    string? Priority,
    string? Contact,
    string? Client,
    string? City,
    string? Notes,
    int Items,
    decimal Value,
    DateTime? DueAt);

public record UpdateRequestDto(string? Status, string? Owner, string? Priority, decimal? Value);
public record CreateWorkOrderDto(string RequestId, int? WorkerId, string? Scope);
public record UpdateWorkOrderDto(int? WorkerId, string? Status, int? Progress, string? Scope);
public record CreateMessageDto(string? ThreadId, string? RequestId, string? SenderRole, string? SenderName, string? Channel, string Body, string? SourceApp);
public record CreatePromotionDto(string Title, string? Offer, string? Body, string? Status);
public record CreateCampaignDto(string Title, string? Body, string? Status);
public record CreateLeadDto(string Name, string? Channel, string? Stage, string? Notes);
public record CreateClientDto(string Name, string? Email, string? Phone, string? Address, string? City, string? Role, string? Firm);
public record CreateGiftCardDto(string? ClientName, string? Address, string? Firm, decimal Amount, string? Code, string? Status);

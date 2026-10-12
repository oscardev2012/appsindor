using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeManagement.Data;

public abstract class SafeEntity
{
    public int Id { get; set; }
}

public class SafeContact : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    [MaxLength(30)] public string Role { get; set; } = "";
    [MaxLength(160)] public string Name { get; set; } = "";
    [MaxLength(160)] public string? Email { get; set; }
    [MaxLength(40)] public string? Phone { get; set; }
    [MaxLength(160)] public string? Firm { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SafeProperty : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    public int? ClientId { get; set; }
    public int? RealtorId { get; set; }
    [MaxLength(200)] public string Address { get; set; } = "";
    [MaxLength(120)] public string? City { get; set; }
    [MaxLength(40)] public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SafeRequest : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    public int? RealtorId { get; set; }
    public int? ClientId { get; set; }
    public int? PropertyId { get; set; }
    [MaxLength(30)] public string SourceApp { get; set; } = "";
    [MaxLength(80)] public string? Source { get; set; }
    [MaxLength(120)] public string? Type { get; set; }
    [MaxLength(60)] public string Status { get; set; } = "New";
    [MaxLength(20)] public string? Priority { get; set; }
    [MaxLength(80)] public string? Owner { get; set; }
    [MaxLength(160)] public string? ContactName { get; set; }
    [MaxLength(160)] public string? ClientName { get; set; }
    [MaxLength(200)] public string Address { get; set; } = "";
    [MaxLength(120)] public string? City { get; set; }
    public string? Notes { get; set; }
    public int Items { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Value { get; set; }
    public DateTime? DueAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SafeEstimate : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    public int RequestId { get; set; }
    [MaxLength(40)] public string Status { get; set; } = "Draft";
    [Column(TypeName = "decimal(18,2)")] public decimal Total { get; set; }
    [MaxLength(80)] public string? RangeText { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SafeWorkOrder : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    public int RequestId { get; set; }
    public int? EstimateId { get; set; }
    public int? WorkerId { get; set; }
    [MaxLength(40)] public string Status { get; set; } = "assigned";
    public int Progress { get; set; }
    [MaxLength(200)] public string? Scope { get; set; }
    public DateTime? CheckInAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SafeThread : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    public int RequestId { get; set; }
    public int? WorkOrderId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SafeMessage : SafeEntity
{
    public int ThreadId { get; set; }
    [MaxLength(30)] public string SenderRole { get; set; } = "";
    [MaxLength(160)] public string SenderName { get; set; } = "";
    [MaxLength(40)] public string Channel { get; set; } = "job";
    public string Body { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class SafePromotion : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    [MaxLength(160)] public string Title { get; set; } = "";
    [MaxLength(200)] public string? Offer { get; set; }
    public string? Body { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; }
}

public class SafeCampaign : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    public int? RealtorId { get; set; }
    [MaxLength(160)] public string Title { get; set; } = "";
    public string? Body { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; }
}

public class SafeLead : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    [MaxLength(160)] public string Name { get; set; } = "";
    [MaxLength(80)] public string? Channel { get; set; }
    [MaxLength(40)] public string Stage { get; set; } = "New";
    [MaxLength(400)] public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SafeInvoice : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    public int? RequestId { get; set; }
    public int? PropertyId { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Total { get; set; }
    [MaxLength(40)] public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; }
}

public class SafeGiftCard : SafeEntity
{
    [MaxLength(40)] public string PublicId { get; set; } = "";
    [MaxLength(160)] public string? ClientName { get; set; }
    [MaxLength(200)] public string? Address { get; set; }
    [MaxLength(160)] public string? Firm { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }
    [MaxLength(40)] public string? Code { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Issued";
    public DateTime CreatedAt { get; set; }
}

public class SafeEvent : SafeEntity
{
    [MaxLength(30)] public string SourceApp { get; set; } = "";
    [MaxLength(80)] public string EventName { get; set; } = "";
    public int? RequestId { get; set; }
    public int? WorkOrderId { get; set; }
    public string? Payload { get; set; }
    public DateTime CreatedAt { get; set; }
}

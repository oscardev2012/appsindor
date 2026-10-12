using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeHeaterSmsAgent;

public class Conversation
{
    public int Id { get; set; }

    [MaxLength(20)]
    public string CustomerPhone { get; set; } = string.Empty;

    public bool NeedsHuman { get; set; }

    [MaxLength(40)]
    public string ActiveBrand { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Stage { get; set; } = "NEW";

    public int StateVersion { get; set; }

    [MaxLength(80)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(120)]
    public string InstallWindow { get; set; } = string.Empty;

    [MaxLength(120)]
    public string ProductSummary { get; set; } = string.Empty;

    [MaxLength(64)]
    public string BookingId { get; set; } = string.Empty;

    public bool BookingNotified { get; set; }

    [MaxLength(20)]
    public string Channel { get; set; } = "SMS";

    [MaxLength(64)]
    public string VoiceCallSid { get; set; } = string.Empty;

    public DateTime VoiceStartedUtc { get; set; }

    [NotMapped]
    public bool VoiceSpanish { get; set; }

    public DateTime LastMessageAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<SmsMessage> Messages { get; set; } = new List<SmsMessage>();
}

public class SmsMessage
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    [MaxLength(20)]
    public string Direction { get; set; } = "in";

    [MaxLength(1600)]
    public string Body { get; set; } = string.Empty;

    public DateTime AtUtc { get; set; } = DateTime.UtcNow;
}

public class FieldCorrection
{
    public int Id { get; set; }
    public int ConversationId { get; set; }

    [MaxLength(40)]
    public string Field { get; set; } = string.Empty;

    [MaxLength(400)]
    public string OldValue { get; set; } = string.Empty;

    [MaxLength(400)]
    public string NewValue { get; set; } = string.Empty;

    [MaxLength(20)]
    public string AppliedStatus { get; set; } = "applied";

    public DateTime AtUtc { get; set; } = DateTime.UtcNow;
}

public class AlertOutbox
{
    public int Id { get; set; }
    public int ConversationId { get; set; }

    [MaxLength(40)]
    public string EventType { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Priority { get; set; } = "ALTA";

    [MaxLength(1600)]
    public string Body { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "queued";

    public DateTime AtUtc { get; set; } = DateTime.UtcNow;
}

public class ProcessedWebhook
{
    public int Id { get; set; }

    [MaxLength(64)]
    public string MessageSid { get; set; } = string.Empty;

    public DateTime AtUtc { get; set; } = DateTime.UtcNow;
}

using Microsoft.EntityFrameworkCore;

namespace SafeHeaterSmsAgent;

public class AgentDb : DbContext
{
    public AgentDb(DbContextOptions<AgentDb> options) : base(options) { }

    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<SmsMessage> Messages => Set<SmsMessage>();
    public DbSet<ProcessedWebhook> ProcessedWebhooks => Set<ProcessedWebhook>();
    public DbSet<FieldCorrection> FieldCorrections => Set<FieldCorrection>();
    public DbSet<AlertOutbox> AlertOutbox => Set<AlertOutbox>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Conversation>()
            .HasIndex(c => c.CustomerPhone)
            .IsUnique();

        model.Entity<ProcessedWebhook>()
            .HasIndex(p => p.MessageSid)
            .IsUnique();
    }
}

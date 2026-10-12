using Microsoft.EntityFrameworkCore;

namespace SafeManagement.Data;

public class SafeDbContext : DbContext
{
    public SafeDbContext(DbContextOptions<SafeDbContext> options) : base(options) { }

    public DbSet<SafeContact> Contacts => Set<SafeContact>();
    public DbSet<SafeProperty> Properties => Set<SafeProperty>();
    public DbSet<SafeRequest> Requests => Set<SafeRequest>();
    public DbSet<SafeEstimate> Estimates => Set<SafeEstimate>();
    public DbSet<SafeWorkOrder> WorkOrders => Set<SafeWorkOrder>();
    public DbSet<SafeThread> Threads => Set<SafeThread>();
    public DbSet<SafeMessage> Messages => Set<SafeMessage>();
    public DbSet<SafePromotion> Promotions => Set<SafePromotion>();
    public DbSet<SafeCampaign> Campaigns => Set<SafeCampaign>();
    public DbSet<SafeLead> Leads => Set<SafeLead>();
    public DbSet<SafeInvoice> Invoices => Set<SafeInvoice>();
    public DbSet<SafeGiftCard> GiftCards => Set<SafeGiftCard>();
    public DbSet<SafeEvent> Events => Set<SafeEvent>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<SafeContact>(e => e.ToTable("SafeContacts"));
        model.Entity<SafeProperty>(e => e.ToTable("SafeProperties"));
        model.Entity<SafeRequest>(e => e.ToTable("SafeRequests"));
        model.Entity<SafeEstimate>(e => e.ToTable("SafeEstimates"));
        model.Entity<SafeWorkOrder>(e => e.ToTable("SafeWorkOrders"));
        model.Entity<SafeThread>(e => e.ToTable("SafeThreads"));
        model.Entity<SafeMessage>(e => e.ToTable("SafeMessages"));
        model.Entity<SafePromotion>(e => e.ToTable("SafePromotions"));
        model.Entity<SafeCampaign>(e => e.ToTable("SafeCampaigns"));
        model.Entity<SafeLead>(e => e.ToTable("SafeLeads"));
        model.Entity<SafeInvoice>(e => e.ToTable("SafeInvoices"));
        model.Entity<SafeGiftCard>(e => e.ToTable("SafeGiftCards"));
        model.Entity<SafeEvent>(e => e.ToTable("SafeEvents"));
    }
}

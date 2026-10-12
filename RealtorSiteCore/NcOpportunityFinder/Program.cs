using NcOpportunityFinder;
using SafeManagement.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(SafeDbRegistration.ConnectionName)
    ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection (IndorDB).");

builder.Services.AddRazorPages();
builder.Services.AddControllers(o => o.Filters.Add<OpportunityDbErrorFilter>());
builder.Services.AddSingleton(new SafeSql(connectionString));
builder.Services.AddSingleton<OpportunityStore>();
builder.Services.Configure<ApifyOptions>(builder.Configuration.GetSection(ApifyOptions.SectionName));
builder.Services.AddSingleton(sp => new ApifyClient(new HttpClient(), sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApifyOptions>>()));
builder.Services.AddSingleton<GeoCatalog>();
builder.Services.AddSingleton<ListingSearchService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorPages().WithStaticAssets();

app.Run();

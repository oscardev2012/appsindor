using MarketplaceSafe.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataDirectory);
var databasePath = Path.Combine(dataDirectory, "safe.db");
builder.Services.AddDbContext<SafeDb>(options => options.UseSqlite($"Data Source={databasePath}"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    if (!string.IsNullOrEmpty(path)
        && path.Length > 1
        && !path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
        && !path.EndsWith('/')
        && !Path.HasExtension(path))
    {
        context.Response.Redirect(path + "/" + context.Request.QueryString, permanent: false);
        return;
    }

    if (!string.IsNullOrEmpty(path) && path.Length > 1 && path.EndsWith('/'))
    {
        context.Request.Path = path.TrimEnd('/');
    }

    await next();
});

app.UseRouting();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SafeDb>();
    db.Database.EnsureCreated();
    await WorkspaceApi.Seed(db);
}

app.MapWorkspaceApi();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();

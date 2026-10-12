var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IndorNews.Services.SitePostStore>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SitePosts", policy =>
        policy.AllowAnyOrigin().WithMethods("GET").WithHeaders("Accept"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapStaticAssets();

app.MapGet("/robots.txt", () => Results.Text(
    """
    User-agent: *
    Disallow: /
    Allow: /api/

    """, "text/plain; charset=utf-8"));

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

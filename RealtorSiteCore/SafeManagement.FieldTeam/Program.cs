using SafeManagement.Data;
using SafeManagement.FieldTeam;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSafeSharedDatabase(builder.Configuration);
builder.Services.AddSingleton<FieldRequestStore>();
builder.Services.AddSingleton<FieldActivityStore>();
builder.Services.AddSingleton<FieldProfileStore>();
builder.Services.AddControllersWithViews()
    .AddApplicationPart(typeof(SafeApiController).Assembly);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

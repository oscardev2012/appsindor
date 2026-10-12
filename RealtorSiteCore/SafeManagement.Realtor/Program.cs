using SafeManagement.Data;
using SafeManagement.Realtor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSafeSharedDatabase(builder.Configuration);
builder.Services.AddSingleton<RealtorTipStore>();
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

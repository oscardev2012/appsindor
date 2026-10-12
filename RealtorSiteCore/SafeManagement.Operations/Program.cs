using SafeManagement.Data;
using SafeManagement.Operations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSafeSharedDatabase(builder.Configuration);
builder.Services.AddSingleton<OperationsContactStore>();
builder.Services.AddSingleton<OperationsLeadStore>();
builder.Services.AddSingleton<OperationsRequestStore>();
builder.Services.AddSingleton<OperationsEstimateStore>();
builder.Services.AddSingleton<OperationsCatalogStore>();
builder.Services.AddSingleton<OperationsFieldStore>();
builder.Services.AddSingleton<OperationsInvoiceStore>();
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

using ITAssetTracking.App.Utility;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.MVC;

var builder = WebApplication.CreateBuilder(args);

var appConfig = new AppConfig(builder.Configuration);
// Add services to the container.
builder.Services.AddControllersWithViews();

// Instantiate Service Factory and provide services for injection
var sf = new ServiceFactory(appConfig);
builder.Services.AddScoped<IAssetAssignmentService>(_ => sf.GetAssetAssignmentService());
builder.Services.AddScoped<IAssetService>(_ => sf.GetAssetService());
builder.Services.AddScoped<IAssetRequestService>(_ => sf.GetAssetRequestService());
builder.Services.AddScoped<IDepartmentService>(_ => sf.GetDepartmentService());
builder.Services.AddScoped<IEmployeeService>(_ => sf.GetEmployeeService());
builder.Services.AddScoped<ISoftwareAssetAssignmentService>(_ => sf.GetSoftwareAssetAssignmentService());
builder.Services.AddScoped<ISoftwareAssetService>(_ => sf.GetSoftwareAssetService());
builder.Services.AddScoped<ISoftwareRequestService>(_ => sf.GetSoftwareRequestService());
builder.Services.AddScoped<ITicketService>(_ => sf.GetTicketService());

var app = builder.Build();

// Configure middleware pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
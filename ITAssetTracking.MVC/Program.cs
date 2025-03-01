using System.Data;
using ITAssetTracking.App.Utility;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.MVC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serilog;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

var appConfig = new AppConfig(builder.Configuration);

// Register Application Db Context and configure authentication
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration["ConnectionString"]));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        
        options.User.RequireUniqueEmail = false;
        
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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

// configure logger
builder.Logging.ClearProviders();
var loggerConfig = new LoggerConfiguration();
if (builder.Configuration.GetValue<bool>("Logging:DbLogging:Enabled"))
{
    var columnOptions = new ColumnOptions();
    columnOptions.AdditionalColumns = new List<SqlColumn>
    {
        new SqlColumn
        {
            ColumnName = "EventSourceID",
            DataType = SqlDbType.TinyInt,
            AllowNull = false,
        }
    };

    loggerConfig
        .Enrich.WithProperty("EventSourceID", 1)
        .WriteTo.MSSqlServer(
            connectionString: builder.Configuration["ConnectionString"],
            tableName: "LogEvent",
            autoCreateSqlTable: false,
            restrictedToMinimumLevel: appConfig.GetDbLogLevel(),
            columnOptions: columnOptions
    );

}
loggerConfig.MinimumLevel.Information()
    .WriteTo.Console();
Log.Logger = loggerConfig.CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

var app = builder.Build();

// Configure middleware pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
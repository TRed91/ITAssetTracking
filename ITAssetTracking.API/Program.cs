using System.Data;
using ITAssetTracking.API;
using ITAssetTracking.App.Utility;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

var appConfig = new AppConfig(builder.Configuration);

builder.Services.AddOpenApi();

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
builder.Services.AddScoped<IReportsService>(_ => sf.GetReportsService());

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
        .Enrich.WithProperty("EventSourceID", 2)
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

// configure cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AnyOrigins", policy =>
    {
        policy.AllowAnyOrigin();
    });
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(appConfig.AllowedOrigins());
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("AnyOrigins");
}
else
{
    app.UseCors("AllowedOrigins");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
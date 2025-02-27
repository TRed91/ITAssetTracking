using ITAssetTracking.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITAssetTracking.Data;

// Db Context
public class ITAssetTrackingContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<Asset> Asset { get; set; }
    public DbSet<AssetAssignment> AssetAssignment { get; set; }
    public DbSet<AssetRequest> AssetRequest { get; set; }
    public DbSet<AssetStatus> AssetStatus { get; set; }
    public DbSet<AssetType> AssetType { get; set; }
    public DbSet<Department> Department { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<LicenseType> LicenseType { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<Manufacturer> Manufacturer { get; set; }
    public DbSet<Model> AssetModel { get; set; }
    public DbSet<RequestResult> RequestResult { get; set; }
    public DbSet<SoftwareAsset> SoftwareAsset { get; set; }
    public DbSet<SoftwareAssetAssignment> SoftwareAssetAssignment { get; set; }
    public DbSet<SoftwareAssetRequest> SoftwareAssetRequest { get; set; }
    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<TicketNotes> TicketNotes { get; set; }
    public DbSet<TicketPriority> TicketPriority { get; set; }
    public DbSet<TicketResolution> TicketResolution { get; set; }
    public DbSet<TicketStatus> TicketStatus { get; set; }
    public DbSet<TicketType> TicketType { get; set; }

    public ITAssetTrackingContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
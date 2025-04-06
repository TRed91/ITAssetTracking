using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITAssetTracking.Data.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly ITAssetTrackingContext _context;

    public ReportsRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public List<AssetAssignment> GetAssetReports(DateTime startDate, DateTime endDate, byte departmentId, byte assetTypeId)
    {
        return _context.AssetAssignment
            .Include(aa => aa.Department)
            .Include(aa => aa.Asset)
            .ThenInclude(a => a.AssetType)
            .Include(aa => aa.Asset)
            .ThenInclude(a => a.AssetStatus)
            .Where(aa => (aa.AssignmentDate >= startDate && aa.AssignmentDate <= endDate) &&
                         (departmentId == 0 || aa.DepartmentID == departmentId) && 
                         (assetTypeId == 0 || (aa.Asset != null && aa.Asset.AssetTypeID == assetTypeId)))
            .ToList();
    }

    public List<Asset> GetAssetsInTimeFrame(DateTime startDate, DateTime endDate)
    {
        return _context.Asset
            .Include(a => a.AssetType)
            .Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetSoftwareAssetReports(DateTime startDate, DateTime endDate,
        byte departmentId, byte licenseTypeId)
    {
        return _context.SoftwareAssetAssignment
            .Include(s => s.SoftwareAsset)
            .Include(s => s.Asset)
            .ThenInclude(a => a.AssetAssignments)
            .Include(s => s.Employee)
            .ThenInclude(e => e.Department)
            .Where(s => (s.AssignmentDate >= startDate && s.AssignmentDate <= endDate) &&
                        (departmentId == 0 || (s.Employee != null && s.Employee.Department != null && s.Employee.Department.DepartmentID == departmentId)) &&
                        (licenseTypeId == 0 || s.SoftwareAsset.LicenseTypeID == licenseTypeId))
            .ToList();
    }
}
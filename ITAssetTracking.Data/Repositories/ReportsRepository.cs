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
            .Where(aa => (aa.AssignmentDate >= startDate || aa.ReturnDate == null) && 
                          aa.AssignmentDate <= endDate &&
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
            .ThenInclude(s => s.AssetStatus)
            .Include(s => s.SoftwareAsset)
            .ThenInclude(s => s.LicenseType)
            .Include(s => s.Asset)
            .ThenInclude(a => a.AssetAssignments.Where(ass =>
                (ass.AssignmentDate >= startDate || ass.ReturnDate == null) 
                && ass.AssignmentDate <= endDate &&
                (departmentId == 0 || ass.DepartmentID == departmentId)))
            .Include(s => s.Employee)
            .ThenInclude(e => e.Department)
            .Where(s =>
                (s.AssignmentDate >= startDate || s.ReturnDate == null) && 
                s.AssignmentDate <= endDate &&
                (departmentId == 0 || (s.EmployeeID == null || s.Employee.DepartmentID == departmentId) && 
                    (s.AssetID == null || s.Asset.AssetAssignments.Any(a => 
                        a.DepartmentID == departmentId &&
                        (a.AssignmentDate >= startDate || a.ReturnDate == null) 
                        && a.AssignmentDate <= endDate))) &&
                (licenseTypeId == 0 || s.SoftwareAsset.LicenseTypeID == licenseTypeId))
            .ToList();
    }
}
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
    
    public List<AssetAssignment> GetAssetReports(DateTime startDate, DateTime endDate, byte? departmentId, byte? assetTypeId)
    {
        return _context.AssetAssignment
            .Include(aa => aa.Department)
            .Include(aa => aa.Asset)
            .ThenInclude(a => a.AssetType)
            .Include(aa => aa.Asset)
            .ThenInclude(a => a.AssetStatus)
            .Where(aa => (aa.AssignmentDate >= startDate && aa.AssignmentDate <= endDate) &&
                         (departmentId == null || aa.DepartmentID == departmentId) && 
                         (assetTypeId == null || (aa.Asset != null && aa.Asset.AssetTypeID == assetTypeId)))
            .ToList();
    }

    public List<Asset> GetAssetsInTimeFrame(DateTime startDate, DateTime endDate)
    {
        return _context.Asset
            .Include(a => a.AssetType)
            .Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate)
            .ToList();
    }
}
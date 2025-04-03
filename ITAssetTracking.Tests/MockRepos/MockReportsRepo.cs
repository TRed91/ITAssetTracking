using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockReportsRepo : IReportsRepository
{
    private readonly MockDB _db;

    public MockReportsRepo()
    {
        _db = new MockDB();
    }
    
    public List<AssetAssignment> GetAssetReports(DateTime startDate, DateTime endDate, byte? departmentId, byte? assetTypeId)
    {
        var assignments = _db.AssetAssignments
            .Where(aa => (aa.AssignmentDate >= startDate && aa.AssignmentDate <= endDate) &&
                         (departmentId == null || aa.DepartmentID == departmentId))
            .ToList();
        foreach (var ass in assignments)
        {
            ass.Asset = _db.Assets.First(a => a.AssetID == ass.AssetID);
            ass.Asset.AssetType = _db.AssetTypes.First(a => a.AssetTypeID == ass.Asset.AssetTypeID);
            ass.Asset.AssetStatus = _db.AssetStatuses.First(a => a.AssetStatusID == ass.Asset.AssetStatusID);
            ass.Department = _db.Departments.First(d => d.DepartmentID == ass.DepartmentID);
        }

        if (assetTypeId.HasValue)
        {
            assignments = assignments
                .Where(a => a.Asset.AssetTypeID == assetTypeId)
                .ToList();
        }
        return assignments;
    }

    public List<Asset> GetAssetsInTimeFrame(DateTime startDate, DateTime endDate)
    {
        var assets = _db.Assets
            .Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate)
            .ToList();

        foreach (var a in assets)
        {
            a.AssetType = _db.AssetTypes.First(t => t.AssetTypeID == a.AssetTypeID);
        }
        return assets;
    }
}
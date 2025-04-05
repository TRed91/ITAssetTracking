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

    public List<SoftwareAssetAssignment> GetSoftwareAssetReports(DateTime startDate, DateTime endDate, byte? departmentId, byte? licenseTypeId)
    {
        var assignments = _db.SoftwareAssetAssignments
            .Where(aa => aa.AssignmentDate >= startDate && aa.AssignmentDate <= endDate)
            .ToList();
        foreach (var ass in assignments)
        {
            if (ass.AssetID != null)
            {
                ass.Asset = _db.Assets.First(a => a.AssetID == ass.AssetID);
            }
            ass.SoftwareAsset = _db.SoftwareAssets.First(a => a.SoftwareAssetID == ass.SoftwareAssetID);
            ass.SoftwareAsset.LicenseType = _db.LicenseTypes.First(l => l.LicenseTypeID == ass.SoftwareAsset.LicenseTypeID);
            if (ass.EmployeeID != null)
            {
                ass.Employee = _db.Employees.First(e => e.EmployeeID == ass.EmployeeID);
                ass.Employee.Department = _db.Departments.First(d => d.DepartmentID == ass.Employee.DepartmentID);
            }
        }

        if (licenseTypeId.HasValue)
        {
            assignments = assignments
                .Where(a => a.SoftwareAsset.LicenseTypeID == licenseTypeId)
                .ToList();
        }

        if (departmentId.HasValue)
        {
            assignments = assignments
                .Where(a => a.Employee.DepartmentID == departmentId)
                .ToList();
        }
        return assignments;
    }
}
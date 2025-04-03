using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Models;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class ReportsService : IReportsService
{
    private readonly IReportsRepository _repo;
    private readonly IDepartmentRepository _departmentRepo;
    private readonly IAssetRepository _assetRepo;

    public ReportsService(IReportsRepository reportsRepository, IDepartmentRepository departmentRepo, IAssetRepository assetRepo)
    {
        _repo = reportsRepository;
        _departmentRepo = departmentRepo;
        _assetRepo = assetRepo;
    }

    public Result<List<AssetDistributionReport>> GetAssetDistributionReport(DateTime startDate, DateTime endDate, byte? departmentId = null, byte? assetTypeId = null)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<List<AssetDistributionReport>>("Start date cannot be after end date");
        }
        try
        {
            var assignments = _repo.GetAssetReports(startDate, endDate, departmentId, assetTypeId);
            if (assignments.Count == 0)
            {
                return ResultFactory.Fail<List<AssetDistributionReport>>("No asset reports found");
            }
            var types = _assetRepo.GetAssetTypes();
            var departments = _departmentRepo.GetDepartments();
            if (departmentId.HasValue)
            {
                departments = departments
                    .Where(d => d.DepartmentID == departmentId.Value)
                    .ToList();
            }

            if (assetTypeId.HasValue)
            {
                types = types
                    .Where(t => t.AssetTypeID == assetTypeId)
                    .ToList();
            }
            var reports  = new List<AssetDistributionReport>();
            foreach (var d in departments)
            {
                var report = new AssetDistributionReport
                {
                    DepartmentName = d.DepartmentName,
                    Items = new List<AssetDistributionReportItem>()
                };
                foreach (var t in types)
                {
                    var item = new AssetDistributionReportItem
                    {
                        AssetTypeName = t.AssetTypeName,
                        NumberOfAssets = assignments.Count(a => a.Asset.AssetTypeID == t.AssetTypeID),
                    };
                    report.Items.Add(item);
                }
                reports.Add(report);
            }
            return ResultFactory.Success(reports);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetDistributionReport>>(ex.Message, ex);
        }
    }

    public Result<List<AssetStatusReport>> GetAssetStatusReport(DateTime startDate, DateTime endDate, byte? departmentId = null, byte? assetTypeId = null)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<List<AssetStatusReport>>("Start date cannot be after end date");
        }
        try
        {
            var assignments = _repo.GetAssetReports(startDate, endDate, departmentId, assetTypeId);
            if (assignments.Count == 0)
            {
                return ResultFactory.Fail<List<AssetStatusReport>>("No asset reports found");
            }

            var types = _assetRepo.GetAssetTypes();
            if (assetTypeId.HasValue)
            {
                types = types
                    .Where(t => t.AssetTypeID == assetTypeId)
                    .ToList();
            }
            var reports = types.Select(t => new AssetStatusReport
            {
                AssetTypeName = t.AssetTypeName,
                NumberOfAssetsTotal = assignments.Count(a => a.Asset.AssetTypeID == t.AssetTypeID),
                NumberOfAssetsStorage = assignments.Count(a => a.Asset.AssetTypeID == t.AssetTypeID &&
                                                               a.Asset.AssetStatus.AssetStatusName == "Storage"),
                NumberOfAssetsInUse = assignments.Count(a => a.Asset.AssetTypeID == t.AssetTypeID &&
                                                             a.Asset.AssetStatus.AssetStatusName == "In Use"),
                NumberOfAssetsRepair = assignments.Count(a => a.Asset.AssetTypeID == t.AssetTypeID &&
                                                              a.Asset.AssetStatus.AssetStatusName == "Repair"),
            }).ToList();

            return ResultFactory.Success(reports);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetStatusReport>>(ex.Message, ex);
        }
    }

    public Result<AssetValuesReport> GetAssetValuesReport(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<AssetValuesReport>("Start date cannot be after end date");
        }

        try
        {
            var assets = _repo.GetAssetsInTimeFrame(startDate, endDate);
            if (assets.Count == 0)
            {
                return ResultFactory.Fail<AssetValuesReport>("No assets found");
            }

            var types = _assetRepo.GetAssetTypes();

            var reportItems = types.Select(t => new AssetValuesReportItem
            {
                AssetTypeName = t.AssetTypeName,
                NumberOfAssets = assets.Count(a => a.AssetTypeID == t.AssetTypeID),
                AssetsValue = assets
                    .Where(a => a.AssetTypeID == t.AssetTypeID)
                    .Sum(a => a.PurchasePrice)
            }).ToList();

            var report = new AssetValuesReport
            {
                TotalValue = assets.Sum(a => a.PurchasePrice),
                Items = reportItems
            };

            return ResultFactory.Success(report);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<AssetValuesReport>(ex.Message, ex);
        }
    }
}
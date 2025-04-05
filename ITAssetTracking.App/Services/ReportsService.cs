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
    private readonly ISoftwareAssetRepository _softwareAssetRepo;
    private readonly ITicketRepository _ticketRepo;

    public ReportsService(
        IReportsRepository reportsRepository, 
        IDepartmentRepository departmentRepo, 
        IAssetRepository assetRepo, 
        ISoftwareAssetRepository softwareAssetRepo,
        ITicketRepository ticketRepo)
    {
        _repo = reportsRepository;
        _departmentRepo = departmentRepo;
        _assetRepo = assetRepo;
        _softwareAssetRepo = softwareAssetRepo;
        _ticketRepo = ticketRepo;
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

    public Result<List<SoftwareAssetDistributionReport>> GetSoftwareAssetDistributionReport(DateTime startDate,
        DateTime endDate, byte? departmentId = null, byte? licenseTypeId = null)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<List<SoftwareAssetDistributionReport>>("Start date cannot be after end date");
        }
        try
        {
            var assignments = _repo.GetSoftwareAssetReports(startDate, endDate, departmentId, licenseTypeId);
            if (assignments.Count == 0)
            {
                return ResultFactory.Fail<List<SoftwareAssetDistributionReport>>("No software asset reports found");
            }

            var types = _softwareAssetRepo.GetLicenseTypes();
            var departments = _departmentRepo.GetDepartments();
            if (departmentId.HasValue)
            {
                departments = departments
                    .Where(d => d.DepartmentID == departmentId.Value)
                    .ToList();
            }

            if (licenseTypeId.HasValue)
            {
                types = types
                    .Where(t => t.LicenseTypeID == licenseTypeId)
                    .ToList();
            }
            var reports  = new List<SoftwareAssetDistributionReport>();
            foreach (var d in departments)
            {
                var report = new SoftwareAssetDistributionReport
                {
                    DepartmentName = d.DepartmentName,
                    Items = new List<SoftwareAssetDistributionReportItem>()
                };
                foreach (var t in types)
                {
                    var item = new SoftwareAssetDistributionReportItem
                    {
                        LicenseTypeName = t.LicenseTypeName,
                        NumberOfLicenses = assignments
                            .Where(s => s.SoftwareAsset.LicenseTypeID == t.LicenseTypeID)
                            .Sum(s => s.SoftwareAsset.NumberOfLicenses),
                    };
                    report.Items.Add(item);
                }
                reports.Add(report);
            }
            return ResultFactory.Success(reports);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetDistributionReport>>(ex.Message, ex);
        }
    }

    public Result<List<AssetStatusReport>> GetSoftwareAssetStatusReport(DateTime startDate, DateTime endDate, byte? departmentId = null, byte? licenseTypeId = null)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<List<AssetStatusReport>>("Start date cannot be after end date");
        }
        try
        {
            var assignments = _repo.GetSoftwareAssetReports(startDate, endDate, departmentId, licenseTypeId);
            if (assignments.Count == 0)
            {
                return ResultFactory.Fail<List<AssetStatusReport>>("No asset reports found");
            }

            var types = _softwareAssetRepo.GetLicenseTypes();
            if (licenseTypeId.HasValue)
            {
                types = types
                    .Where(t => t.LicenseTypeID == licenseTypeId)
                    .ToList();
            }
            var reports = types.Select(t => new AssetStatusReport
            {
                AssetTypeName = t.LicenseTypeName,
                NumberOfAssetsTotal = assignments.Count(a => a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID),
                NumberOfAssetsStorage = assignments.Count(a => a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID &&
                                                               a.Asset.AssetStatus.AssetStatusName == "Storage"),
                NumberOfAssetsInUse = assignments.Count(a => a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID &&
                                                             a.Asset.AssetStatus.AssetStatusName == "In Use"),
                NumberOfAssetsRepair = assignments.Count(a => a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID &&
                                                              a.Asset.AssetStatus.AssetStatusName == "Repair"),
            }).ToList();

            return ResultFactory.Success(reports);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetStatusReport>>(ex.Message, ex);
        }
    }

    public Result<TicketsReport> GetTicketsReport(DateTime startDate, DateTime endDate, byte? ticketTypeId = null)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<TicketsReport>("Start date cannot be after end date");
        }

        try
        {
            var tickets = _ticketRepo.GetTicketsInDateRange(startDate, endDate);
            if (tickets.Count == 0)
            {
                return ResultFactory.Fail<TicketsReport>("No tickets found");
            }
            var types = _ticketRepo.GetTicketTypes();
            if (ticketTypeId.HasValue)
            {
                types = types
                    .Where(t => t.TicketTypeID == ticketTypeId)
                    .ToList();
                tickets = tickets
                    .Where(t => t.TicketType.TicketTypeID == ticketTypeId)
                    .ToList();
            }
            tickets = tickets
                .Where(t => t.TicketResolutionID != null && t.DateClosed != null)
                .ToList();
            
            var report = new TicketsReport
            {
                TotalTickets = tickets.Count,
                ReportsList = new List<TicketTypeReport>(),
            };
            
            foreach (var type in types)
            {
                var typeReport = new TicketTypeReport
                {
                    TicketTypeName = type.TicketTypeName,
                    NumberOfTickets = tickets.Count(t => t.TicketTypeID == type.TicketTypeID),
                    AvgResolutionTimeInDays = (int)tickets.Average(t => t.DateClosed.Value.Subtract(t.DateReported).Days),
                    CancelledTickets = tickets
                        .Count(t => t.TicketTypeID == type.TicketTypeID && 
                                    t.TicketResolution.TicketResolutionName == "Cancelled"),
                    CompletedTickets = tickets
                        .Count(t => t.TicketTypeID == type.TicketTypeID && 
                                    t.TicketResolution.TicketResolutionName == "Completed"),
                    UserErrorTickets = tickets
                        .Count(t => t.TicketTypeID == type.TicketTypeID && 
                                    t.TicketResolution.TicketResolutionName == "User Error"),
                    OtherTickets = tickets.
                        Count(t => t.TicketTypeID == type.TicketTypeID && 
                                   t.TicketResolution.TicketResolutionName == "Other"),
                };
                report.ReportsList.Add(typeReport);
            }
            return ResultFactory.Success(report);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<TicketsReport>(ex.Message, ex);
        }
    }
}
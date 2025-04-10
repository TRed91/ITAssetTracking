using ITAssetTracking.Core.Entities;
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

    public Result<List<AssetDistributionReport>> GetAssetDistributionReport(DateTime startDate, DateTime endDate, byte departmentId = 0, byte assetTypeId = 0)
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
                return ResultFactory.Success(new List<AssetDistributionReport>());
            }
            var types = _assetRepo.GetAssetTypes();
            var departments = _departmentRepo.GetDepartments();
            if (departmentId > 0)
            {
                departments = departments
                    .Where(d => d.DepartmentID == departmentId)
                    .ToList();
            }

            if (assetTypeId > 0)
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
                        NumberOfAssets = assignments.Count(a => 
                            a.Asset.AssetTypeID == t.AssetTypeID && 
                            a.DepartmentID == d.DepartmentID),
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

    public Result<List<AssetStatusReport>> GetAssetStatusReport(DateTime startDate, DateTime endDate, byte departmentId = 0, byte assetTypeId = 0)
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
                return ResultFactory.Success(new List<AssetStatusReport>());
            }

            var types = _assetRepo.GetAssetTypes();
            if (assetTypeId > 0)
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
                var emptyReport = new AssetValuesReport();
                emptyReport.Items = new List<AssetValuesReportItem>();
                emptyReport.TotalValue = 0;
                return ResultFactory.Success(emptyReport);
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
        DateTime endDate, byte departmentId = 0, byte licenseTypeId = 0)
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
                return ResultFactory.Success(new List<SoftwareAssetDistributionReport>());
            }

            var types = _softwareAssetRepo.GetLicenseTypes();
            var departments = _departmentRepo.GetDepartments();
            if (departmentId > 0)
            {
                departments = departments
                    .Where(d => d.DepartmentID == departmentId)
                    .ToList();
            }

            if (licenseTypeId > 0)
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
                            .Where(s => 
                                s.SoftwareAsset.LicenseTypeID == t.LicenseTypeID && 
                                    (s.EmployeeID == null || s.Employee.DepartmentID == d.DepartmentID) &&
                                    (s.AssetID == null || s.Asset.AssetAssignments.Any(a =>
                                        a.DepartmentID == d.DepartmentID)))
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

    public Result<List<AssetStatusReport>> GetSoftwareAssetStatusReport(DateTime startDate, DateTime endDate, byte departmentId = 0, byte licenseTypeId = 0)
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
                return ResultFactory.Success(new List<AssetStatusReport>());
            }

            var types = _softwareAssetRepo.GetLicenseTypes();
            if (licenseTypeId > 0)
            {
                types = types
                    .Where(t => t.LicenseTypeID == licenseTypeId)
                    .ToList();
            }
            var reports = types.Select(t => new AssetStatusReport
            {
                AssetTypeName = t.LicenseTypeName,
                
                NumberOfAssetsTotal = assignments.Where(a => 
                        a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID)
                    .Sum(a => a.SoftwareAsset.NumberOfLicenses),
                
                NumberOfAssetsStorage = assignments.Where(a => 
                    a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID &&
                    a.SoftwareAsset.AssetStatus.AssetStatusName == "Storage")
                    .Sum(a => a.SoftwareAsset.NumberOfLicenses),
                
                NumberOfAssetsInUse = assignments.Where(a => 
                    a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID &&
                    a.SoftwareAsset.AssetStatus.AssetStatusName == "In Use")
                    .Sum(a => a.SoftwareAsset.NumberOfLicenses),
                
                NumberOfAssetsRepair = assignments.Where(a => 
                    a.SoftwareAsset.LicenseTypeID == t.LicenseTypeID &&
                    a.SoftwareAsset.AssetStatus.AssetStatusName == "Repair")
                    .Sum(a => a.SoftwareAsset.NumberOfLicenses),
            }).ToList();

            return ResultFactory.Success(reports);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetStatusReport>>(ex.Message, ex);
        }
    }

    public Result<TicketsReport> GetTicketsReport(DateTime startDate, DateTime endDate, byte ticketTypeId = 0)
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
                return ResultFactory.Success(new TicketsReport());
            }
            var types = _ticketRepo.GetTicketTypes();
            if (ticketTypeId > 0)
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
                    AvgResolutionTimeInDays = CalculateAverageTicketResolutionTime(tickets
                            .Where(t => t.TicketTypeID == type.TicketTypeID)
                            .ToList()),
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

    private int CalculateAverageTicketResolutionTime(List<Ticket> tickets)
    {
        if (tickets.Count == 0)
        {
            return 0;
        }

        return (int)tickets
            .Average(t => t.DateClosed.Value.Subtract(t.DateReported).Days);
    }
}
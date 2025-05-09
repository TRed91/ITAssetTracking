using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class ReportsController : Controller
{
    private readonly IReportsService _reportsService;
    private readonly IDepartmentService _departmentService;
    private readonly IAssetService _assetService;
    private readonly ISoftwareAssetService _softwareAssetService;
    private readonly ITicketService _ticketService;
    private readonly Serilog.ILogger _logger;

    public ReportsController(
        IReportsService reportsService, 
        IDepartmentService departmentService,
        IAssetService assetService,
        ISoftwareAssetService softwareAssetService,
        ITicketService ticketService,
        Serilog.ILogger logger)
    {
        _reportsService = reportsService;
        _departmentService = departmentService;
        _assetService = assetService;
        _softwareAssetService = softwareAssetService;
        _ticketService = ticketService;
        _logger = logger;
    }
    
    [Authorize(Roles = "Admin, AssetManager, SoftwareLicenseManager, Auditor, HelpDescTechnician")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin, AssetManager, Auditor")]
    public IActionResult HardwareDistribution(AssetDistributionReportModel model)
    {
        var reportData = _reportsService.GetAssetDistributionReport(
            model.FromDate, model.ToDate, model.DepartmentId, model.AssetTypeId);

        if (!reportData.Ok)
        {
            _logger.Error(reportData.Exception, $"Failed to fetch report data: {reportData.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, reportData.Message));
            return RedirectToAction("Index");
        }

        var assetTypes = _assetService.GetAssetTypes().Data;
        var departments = _departmentService.GetDepartments().Data;

        var reports = reportData.Data;
        switch (model.Order)
        {
            case AssetDistributionOrder.NumberOfAssets:
                foreach (var r in reports)
                {
                    r.Items = r.Items
                        .OrderByDescending(i => i.NumberOfAssets)
                        .ToList();
                }
                break;
            default:
                foreach (var r in reports)
                {
                    r.Items = r.Items
                        .OrderBy(i => i.AssetTypeName)
                        .ToList();
                }
                break;
        }
        
        model.AssetDistributionReports = reports;
        model.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
        model.AssetTypes = new SelectList(assetTypes, "AssetTypeID", "AssetTypeName");
        
        return View(model);
    }

    [Authorize(Roles = "Admin, AssetManager, Auditor")]
    public IActionResult HardwareStatus(AssetStatusReportModel model)
    {
        var reportData = _reportsService.GetAssetStatusReport(
            model.FromDate, model.ToDate, model.DepartmentId, model.AssetTypeId);

        if (!reportData.Ok)
        {
            _logger.Error(reportData.Exception, $"Failed to fetch report data: {reportData.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, reportData.Message));
            return RedirectToAction("Index");
        }

        var assetTypes = _assetService.GetAssetTypes().Data;
        var departments = _departmentService.GetDepartments().Data;

        var reports = reportData.Data;
        switch (model.Order)
        {
            case AssetStatusOrder.TotalAssets:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsTotal)
                    .ToList();
                break;
            case AssetStatusOrder.Storage:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsStorage)
                    .ToList();
                break;
            case AssetStatusOrder.Repair:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsRepair)
                    .ToList();
                break;
            case AssetStatusOrder.InUse:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsInUse)
                    .ToList();
                break;
            default:
                reports = reports
                    .OrderBy(r => r.AssetTypeName)
                    .ToList();
                break;
        }
        
        model.AssetStatusReports = reports;
        model.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
        model.AssetTypes = new SelectList(assetTypes, "AssetTypeID", "AssetTypeName");
        
        return View(model);
    }

    [Authorize(Roles = "Admin, AssetManager, Auditor")]
    public IActionResult HardwareValue(AssetValueReportModel model)
    {
        var reportData = _reportsService.GetAssetValuesReport(model.FromDate, model.ToDate);

        if (!reportData.Ok)
        {
            _logger.Error(reportData.Exception, $"Failed to fetch report data: {reportData.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, reportData.Message));
            return RedirectToAction("Index");
        }

        var report = reportData.Data;
        if (report.Items.Count > 1)
        {
            switch (model.Order)
            {
                case AssetValueOrder.NumberOfAssets:
                    report.Items = report.Items
                        .OrderByDescending(r => r.NumberOfAssets)
                        .ToList();
                    break;
                case AssetValueOrder.Value:
                    report.Items = report.Items
                        .OrderByDescending(r => r.AssetsValue)
                        .ToList();
                    break;
                default:
                    report.Items = report.Items
                        .OrderBy(r => r.AssetTypeName)
                        .ToList();
                    break;
            }   
        }
        
        model.AssetValuesReport = report;
        
        return View(model);
    }

    [Authorize(Roles = "Admin, SoftwareLicenseManager, Auditor")]
    public IActionResult SoftwareDistribution(SoftwareDistributionReportModel model)
    {
        var reportData = _reportsService.GetSoftwareAssetDistributionReport(
            model.FromDate, model.ToDate, model.DepartmentId, model.LicenseTypeId);

        if (!reportData.Ok)
        {
            _logger.Error(reportData.Exception, $"Failed to fetch report data: {reportData.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, reportData.Message));
            return RedirectToAction("Index");
        }

        var licenseTypes = _softwareAssetService.GetLicenseTypes().Data;
        var departments = _departmentService.GetDepartments().Data;

        var reports = reportData.Data;
        switch (model.Order)
        {
            case SoftwareDistributionOrder.NumberOfLicenses:
                foreach (var r in reports)
                {
                    r.Items = r.Items
                        .OrderByDescending(i => i.NumberOfLicenses)
                        .ToList();
                }
                break;
            default:
                foreach (var r in reports)
                {
                    r.Items = r.Items
                        .OrderBy(i => i.LicenseTypeName)
                        .ToList();
                }
                break;
        }
        
        model.SoftwareDistributionReports = reports;
        model.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
        model.LicenseTypes = new SelectList(licenseTypes, "LicenseTypeID", "LicenseTypeName");
        
        return View(model);
    }

    [Authorize(Roles = "Admin, SoftwareLicenseManager, Auditor")]
    public IActionResult SoftwareStatus(SoftwareStatusReportModel model)
    {
        var reportData = _reportsService.GetSoftwareAssetStatusReport(
            model.FromDate, model.ToDate, model.DepartmentId, model.LicenseTypeId);

        if (!reportData.Ok)
        {
            _logger.Error(reportData.Exception, $"Failed to fetch report data: {reportData.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, reportData.Message));
            return RedirectToAction("Index");
        }

        var licenseTypes = _softwareAssetService.GetLicenseTypes().Data;
        var departments = _departmentService.GetDepartments().Data;

        var reports = reportData.Data;
        switch (model.Order)
        {
            case SoftwareStatusOrder.TotalLicenses:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsTotal)
                    .ToList();
                break;
            case SoftwareStatusOrder.Storage:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsStorage)
                    .ToList();
                break;
            case SoftwareStatusOrder.Repair:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsRepair)
                    .ToList();
                break;
            case SoftwareStatusOrder.InUse:
                reports = reports
                    .OrderByDescending(r => r.NumberOfAssetsInUse)
                    .ToList();
                break;
            default:
                reports = reports
                    .OrderBy(r => r.AssetTypeName)
                    .ToList();
                break;
        }
        
        model.SoftwareStatusReports = reports;
        model.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
        model.LicenseTypes = new SelectList(licenseTypes, "LicenseTypeID", "LicenseTypeName");
        
        return View(model);
    }

    [Authorize(Roles = "Admin, HelpDescTechnician, Auditor")]
    public IActionResult Tickets(TicketReportModel model)
    {
        var reportData = _reportsService.GetTicketsReport(
            model.FromDate, model.ToDate, model.TicketTypeId);
        
        if (!reportData.Ok)
        {
            _logger.Error(reportData.Exception, $"Failed to fetch report data: {reportData.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, reportData.Message));
            return RedirectToAction("Index");
        }
        
        var ticketTypes = _ticketService.GetTicketTypes().Data;
        
        model.TicketTypes = new SelectList(ticketTypes, "TicketTypeID", "TicketTypeName");
        model.Report = reportData.Data;
        
        return View(model);
    }
}
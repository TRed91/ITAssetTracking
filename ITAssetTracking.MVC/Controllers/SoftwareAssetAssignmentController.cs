using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class SoftwareAssetAssignmentController : Controller
{
    private readonly ISoftwareAssetAssignmentService _swAssignmentService;
    private readonly ISoftwareAssetService _swService;
    private readonly IEmployeeService _employeeService;
    private readonly IAssetService _assetService;
    private readonly IDepartmentService _departmentService;
    private readonly Serilog.ILogger _logger;

    public SoftwareAssetAssignmentController(
        ISoftwareAssetAssignmentService softwareAssetAssignmentService,
        ISoftwareAssetService softwareAssetService,
        IEmployeeService employeeService,
        IAssetService assetService,
        IDepartmentService departmentService,
        Serilog.ILogger logger)
    {
        _swAssignmentService = softwareAssetAssignmentService;
        _swService = softwareAssetService;
        _employeeService = employeeService;
        _assetService = assetService;
        _departmentService = departmentService;
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult AssignEmployee(int assetId, AssignEmployeeModel model)
    {
        Result<List<Employee>> employeesResult;
        if (model.DepartmentId.HasValue && model.DepartmentId > 0)
        {
            employeesResult = _employeeService.GetEmployeesByDepartment((int)model.DepartmentId);
        }
        else
        {
            employeesResult = _employeeService.GetEmployees();
        }
        if (!employeesResult.Ok)
        {
            _logger.Error("Error retrieving employees list: " + employeesResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , employeesResult.Message));
            return RedirectToAction("Index", "Home");
        }
        var employees = employeesResult.Data;
        var departments = _departmentService.GetDepartments().Data;
        
        if (model.StartsWith.HasValue && char.IsLetter((char)model.StartsWith))
        {
            employees = employees
                .Where(e => e.LastName[0] == model.StartsWith)
                .ToList();
        }
        if (!string.IsNullOrEmpty(model.Search))
        {
            employees = employees
                .Where(e => e.LastName.ToLower().Contains(model.Search.ToLower()) ||
                            e.FirstName.ToLower().Contains(model.Search.ToLower()))
                .ToList();
        }
        var assetResult = _swService.GetSoftwareAsset((int)model.AssetID);
        if (!assetResult.Ok)
        {
            _logger.Error("Error retrieving asset: " + assetResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , assetResult.Message));
            return RedirectToAction("Index", "Home");
        }
        model.AssetID = assetId;
        model.LicenseTypeName = assetResult.Data.LicenseType.LicenseTypeName;
        model.Employees = employees;
        model.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AssignEmployee(AssignEmployeeModel model)
    {
        var employeeResult = _employeeService.GetEmployeeById(model.EmployeeID);
        if (!employeeResult.Ok)
        {
            _logger.Error("Error retrieving employee record: " + employeeResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false , employeeResult.Message));
            return RedirectToAction("Index", "Home");
        }
        var assetAssignment = new SoftwareAssetAssignment
        {
            SoftwareAssetID = (int)model.AssetID,
            EmployeeID = model.EmployeeID,
        };
        var assignmentResult = _swAssignmentService.AddSoftwareAssetAssignment(assetAssignment);
        if (!assignmentResult.Ok)
        {
            _logger.Error($"Error assigning asset: {assignmentResult.Message} => {assignmentResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false , assignmentResult.Message));
            return RedirectToAction("AssignEmployee",  new { assetId = model.AssetID });
        }
        var msg = $"Assigned Software Asset with ID {model.AssetID} to Employee with ID {model.EmployeeID}";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true , msg));
        return RedirectToAction("Details", "SoftwareAsset", new { assetId = model.AssetID });
    }
    
    public IActionResult FilterAssets(int softwareAssetId)
    {
        var assetTypesRes = _assetService.GetAssetTypes();
        var manufacturersRes = _assetService.GetManufacturers();
        var locationsRes = _assetService.GetLocations();
        var statusesRes = _assetService.GetAssetStatuses();
        if (!assetTypesRes.Ok || !manufacturersRes.Ok || !locationsRes.Ok || !statusesRes.Ok)
        {
            string errMsg = assetTypesRes.Message ?? 
                            manufacturersRes.Message ?? 
                            locationsRes.Message ?? 
                            statusesRes.Message ?? "Unknown Error";
            var ex = assetTypesRes.Exception ?? manufacturersRes.Exception ?? locationsRes.Exception ?? statusesRes.Exception;
            _logger.Error($"Error retrieving data: {errMsg} => {ex}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, "There was an error retrieving data"));
            return RedirectToAction("Details", "SoftwareAsset", new { assetId = softwareAssetId });
        }
        
        var assetTypes = FilterValidAssetTypes(assetTypesRes.Data);

        var model = new AssetFilterModel
        {
            AssetTypes = new SelectList(assetTypes, "AssetTypeID", "AssetTypeName"),
            Manufacturers = new SelectList(manufacturersRes.Data, "ManufacturerID", "ManufacturerName"),
            Locations = new SelectList(locationsRes.Data, "LocationID", "LocationName"),
            AssetStatuses = new SelectList(statusesRes.Data, "AssetStatusID", "AssetStatusName"),
            SoftwareAssetId = softwareAssetId,
            AssetTypeId = 0
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult AssignAsset(AssetFilterModel model)
    {
        // fetch data
        var assetsResult = _assetService.GetAllAssets(
            model.AssetTypeId, 
            model.LocationId, 
            model.AssetStatusId, 
            model.ManufacturerId);
        
        if (!assetsResult.Ok)
        {
            _logger.Error("Error retrieving data: " + assetsResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, "There was an error retrieving data"));
            return RedirectToAction("Details", "SoftwareAsset",  new { assetId = model.SoftwareAssetId });
        }
        
        var assetTypesRes = _assetService.GetAssetTypes();
        var manufacturersRes = _assetService.GetManufacturers();
        var locationsRes = _assetService.GetLocations();
        var statusesRes = _assetService.GetAssetStatuses();
        if (!assetTypesRes.Ok || !manufacturersRes.Ok || !locationsRes.Ok || !statusesRes.Ok)
        {
            string errMsg = assetTypesRes.Message ?? 
                            manufacturersRes.Message ?? 
                            locationsRes.Message ?? 
                            statusesRes.Message ?? "Unknown Error";
            var ex = assetTypesRes.Exception ?? manufacturersRes.Exception ?? locationsRes.Exception ?? statusesRes.Exception;
            _logger.Error($"Error retrieving data: {errMsg} => {ex}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, "There was an error retrieving data"));
            return RedirectToAction("Details", "SoftwareAsset",  new { assetId = model.SoftwareAssetId });
        }

        var assetTypes = FilterValidAssetTypes(assetTypesRes.Data);
        var assets = assetsResult.Data;
        
        // remove retired assets
        assets = assets
            .Where(a => a.AssetStatus.AssetStatusName != "Retired")
            .ToList();
        
        
        
        // filter by search string
        if (!string.IsNullOrEmpty(model.Search))
        {
            assets = assets
                .Where(a => a.SerialNumber.ToLower().Contains(model.Search.ToLower()) || 
                            a.Model.ModelNumber.ToLower().Contains(model.Search.ToLower()))
                .ToList();
        }

        // order the result
        switch (model.Order)
        {
            case AssetsOrder.Location:
                assets = assets.OrderBy(a => a.Location.LocationName).ToList();
                break;
            case AssetsOrder.Manufacturer:
                assets = assets.OrderBy(a => a.Manufacturer.ManufacturerName).ToList();
                break;
            case AssetsOrder.AssetStatus:
                assets = assets.OrderBy(a => a.AssetStatus.AssetStatusName).ToList();
                break;
            case AssetsOrder.AssetType:
                assets = assets.OrderBy(a => a.AssetType.AssetTypeName).ToList();
                break;
            case AssetsOrder.Model:
                assets = assets.OrderBy(a => a.Model.ModelNumber).ToList();
                break;
            default:
                assets = assets.OrderBy(a => a.SerialNumber).ToList();
                break;
        }
        
        model.Assets = assets;
        model.AssetTypes = new SelectList(assetTypes, "AssetTypeID", "AssetTypeName");
        model.Manufacturers = new SelectList(manufacturersRes.Data, "ManufacturerID", "ManufacturerName");
        model.Locations = new SelectList(locationsRes.Data, "LocationID", "LocationName");
        model.AssetStatuses = new SelectList(statusesRes.Data, "AssetStatusID", "AssetStatusName");
        
        return  View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AssignAsset(long assetId, AssetFilterModel model)
    {
        var assignment = new SoftwareAssetAssignment
        {
            SoftwareAssetID = (int)model.SoftwareAssetId,
            AssetID = assetId,
        };
        var result = _swAssignmentService.AddSoftwareAssetAssignment(assignment);
        if (!result.Ok)
        {
            _logger.Error($"Error assigning software asset: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new  TempDataMsg(false, result.Message));
            return RedirectToAction("FilterAssets", new { softwareAssetId = model.SoftwareAssetId });
        }
        
        var successMsg = $"Software Asset with id {model.SoftwareAssetId} assigned to asset with ID {assetId}";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new  TempDataMsg(true, successMsg));
        return RedirectToAction("Details", "SoftwareAsset",  new { assetId = model.SoftwareAssetId });
    }

    private List<AssetType> FilterValidAssetTypes(List<AssetType> assetTypes)
    {
        return assetTypes
            .Where(a =>
                a.AssetTypeName == "PC" ||
                a.AssetTypeName == "Notebook" ||
                a.AssetTypeName == "Graphic Display" ||
                a.AssetTypeName == "Mobile Phone" ||
                a.AssetTypeName == "Tablet")
            .ToList();
    }
}
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Enums;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class LicenseRequestController : Controller
{
    private readonly ISoftwareRequestService _softwareRequestService;
    private readonly IAssetRequestService _assetRequestService;
    private readonly ISoftwareAssetService _softwareAssetService;
    private readonly IAssetService _assetService;
    private readonly IEmployeeService _employeeService;
    private readonly IAssetAssignmentService _assetAssignmentService;
    private readonly IDepartmentService _departmentService;
    private readonly ISoftwareAssetAssignmentService _swAssignmentService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Serilog.ILogger _logger;

    public LicenseRequestController(
        ISoftwareRequestService softwareRequestService,
        IAssetRequestService assetRequestService,
        ISoftwareAssetService softwareAssetService,
        IAssetService assetService,
        IEmployeeService employeeService,
        IAssetAssignmentService assetAssignmentService,
        IDepartmentService departmentService,
        ISoftwareAssetAssignmentService softwareAssetAssignmentService,
        UserManager<ApplicationUser> userManager,
        Serilog.ILogger logger)
    {
        _softwareRequestService = softwareRequestService;
        _assetRequestService = assetRequestService;
        _softwareAssetService = softwareAssetService;
        _assetService = assetService;
        _employeeService = employeeService;
        _assetAssignmentService = assetAssignmentService;
        _departmentService = departmentService;
        _swAssignmentService = softwareAssetAssignmentService;
        _userManager = userManager;
        _logger = logger;
    }
    
    [Authorize(Roles = "Admin, SoftwareLicenseManager, DepartmentManager")]
    public IActionResult Index()
    {
        var requestsResult = _assetRequestService.GetOpenAssetRequests();
        var swRequestsResult = _softwareRequestService.GetOpenSoftwareRequests();
        if (!requestsResult.Ok || !swRequestsResult.Ok)
        {
            var errMsg = requestsResult.Message ?? swRequestsResult.Message ?? "Unknown Error";
            var ex = requestsResult.Exception ?? swRequestsResult.Exception;
            _logger.Error(ex, $"Failed to get open asset requests: {errMsg}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            return RedirectToAction("Index", "Home");
        }

        var model = new SwRequestsIndexModel
        {
            Requests = swRequestsResult.Data,
            AssetRequestsCount = requestsResult.Data.Count,
            SoftwareAssetRequestsCount = swRequestsResult.Data.Count,
        };
        
        return View(model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Assign(int requestId)
    {
        var requestResult = _softwareRequestService.GetSoftwareRequestById(requestId);
        var openAssignmentsResult = _swAssignmentService
            .GetAssignmentsBySoftwareAssetId(requestResult.Data.SoftwareAssetID, false);
        if (!requestResult.Ok || !openAssignmentsResult.Ok)
        {
            var errMsg = requestResult.Message ?? openAssignmentsResult.Message ?? "Unknown Error";
            var ex = requestResult.Exception ?? openAssignmentsResult.Exception;
            _logger.Error(ex, $"Failed to fetch Data: {requestId}:  {errMsg}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            return RedirectToAction("Index");
        }

        var model = new SwAssetRequestModel(requestResult.Data);
        
        if (openAssignmentsResult.Data.Count > 0)
        {
            model.CurrentAssignment = openAssignmentsResult.Data[0];
        }
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Assign(SwAssetRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            var msg = "Invalid model state while confirming software asset assignment request";
            _logger.Warning(msg);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, msg));
            return RedirectToAction("Assign", new { requestId = model.SoftwareAssetRequestId });
        }
        
        var result = _softwareRequestService.ResolveRequest(model.SoftwareAssetRequestId, RequestResultEnum.Confirmed, model.Note);
        if (!result.Ok)
        {
            _logger.Error(result.Exception, $"Failed to add software asset assignment: {result.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Assign", new { requestId = model.SoftwareAssetRequestId });
        }
        
        var successMsg =  $"Software Asset with Id {model.SoftwareAssetId} assigned to {(model.EmployeeId != null 
            ? $"Employee with Id {model.EmployeeId}" 
            : $"Asset with Id {model.AssetId}")}";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Deny(int requestId)
    {
        var requestResult = _softwareRequestService.GetSoftwareRequestById(requestId);
        if (!requestResult.Ok)
        {
            _logger.Error(requestResult.Exception, 
                $"Failed to get asset request with id: {requestId}:  {requestResult.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, requestResult.Message));
            return RedirectToAction("Index");
        }

        var model = new SwAssetRequestModel(requestResult.Data);
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Deny(SwAssetRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            var msg = "Invalid model state while denying software asset assignment request";
            _logger.Warning(msg);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, msg));
            return RedirectToAction("Deny", new { requestId = model.SoftwareAssetRequestId });
        }
        
        var result = _softwareRequestService.ResolveRequest(model.SoftwareAssetRequestId, RequestResultEnum.Denied, model.Note);
        if (!result.Ok)
        {
            _logger.Error(result.Exception, $"Failed to deny request: {result.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Deny", new { requestId = model.SoftwareAssetRequestId });
        }
        
        var successMsg =  $"Assignment for Software Asset with Id {model.SoftwareAssetId} to {(model.EmployeeId != null 
            ? $"Employee with Id {model.EmployeeId}" 
            : $"Asset with Id {model.AssetId}")} was denied";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Index");
    }
    
    [Authorize(Roles = "Admin, SoftwareLicenseManager, DepartmentManager")]
    public IActionResult AvailableLicenses(int departmentId, AvailableLicensesModel model)
    {
        var assetsResult = _softwareRequestService.GetAvailableAssets();
        if (!assetsResult.Ok)
        {
            _logger.Error(assetsResult.Exception, $"Failed to get available assets: {assetsResult.Message}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, assetsResult.Message));
            return RedirectToAction("Index", "Asset");
        }

        var licenseTypes = assetsResult.Data;
        model.LicenseTypesSelectList = new SelectList(licenseTypes, "LicenseTypeID", "LicenseTypeName");

        if (model.SelectedLicenseTypeId != null && model.SelectedLicenseTypeId > 0)
        {
            model.AvailableLicenses = licenseTypes
                .Where(a => a.LicenseTypeID == model.SelectedLicenseTypeId)
                .ToList();
        }
        else
        {
            model.AvailableLicenses = licenseTypes;
        }

        model.RequestingDepartmentId = departmentId;
        
        return View(model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, DepartmentManager")]
    public IActionResult RequestLicense(int departmentId, int softwareAssetId, SwAssetRequestAssignmentModel assignmentModel)
    {
        var departmentResult = _departmentService.GetDepartmentById(departmentId);
        var softwareResult = _softwareAssetService.GetSoftwareAsset(softwareAssetId);
        var assetsResult = _assetAssignmentService.GetAssetAssignmentsByDepartment(departmentId, false);
        
        if (!departmentResult.Ok || !softwareResult.Ok || !assetsResult.Ok)
        {
            var ex = departmentResult.Exception ?? softwareResult.Exception ?? assetsResult.Exception;
            string errMsg = "There was an error retrieving data: " + 
                            (departmentResult.Message ?? softwareResult.Message ?? assetsResult.Message ?? "Unknown Error");
            _logger.Error(ex, $"Failed to get employees for department {departmentId}: {errMsg}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            return RedirectToAction("AvailableLicenses", new { departmentId });
        }
        
        assignmentModel.DepartmentID = departmentId;
        assignmentModel.DepartmentName = departmentResult.Data.DepartmentName;
        assignmentModel.SoftwareAssetId = softwareAssetId;
        assignmentModel.LicenseTypeName = softwareResult.Data.LicenseType.LicenseTypeName;

        if (assignmentModel.StartingLetter == null || !char.IsLetter((char)assignmentModel.StartingLetter))
        {
            assignmentModel.Employees = departmentResult.Data.Employees;
        }
        else
        {
            assignmentModel.Employees = departmentResult.Data.Employees
                .Where(e => e.LastName.StartsWith((char)assignmentModel.StartingLetter))
                .ToList();
        }
        
        assignmentModel.Assets = assetsResult.Data.Select(a => a.Asset).ToList();
        
        return View(assignmentModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, DepartmentManager")]
    public IActionResult RequestLicense(SwAssetRequestAssignmentModel assignmentModel)
    {
        var request = new SoftwareAssetRequest
        {
            SoftwareAssetID = assignmentModel.SoftwareAssetId,
            EmployeeID = assignmentModel.EmployeeId,
            AssetID = assignmentModel.AssetId,
        };
        var addRequestResult = _softwareRequestService.AddSoftwareRequest(request);
        if (!addRequestResult.Ok)
        {
            _logger.Error(addRequestResult.Exception, $"Failed to create asset request: {addRequestResult.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, addRequestResult.Message));
            return RedirectToAction("AvailableLicenses", new { departmentId = assignmentModel.DepartmentID });
        }

        var msg = "Successfully created asset request";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, msg));
        return RedirectToAction("DepartmentAssets", "Asset");
    }
    
    [Authorize(Roles = "Admin, DepartmentManager")]
    public async Task<IActionResult> RequestReassignment(int softwareAssetId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var employee = _employeeService.GetEmployeeById(user.EmployeeID);
        if (!employee.Ok)
        {
            _logger.Error(employee.Exception, $"Failed to get employee with id {user.EmployeeID}: {employee.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, employee.Message));
            return RedirectToAction("Details", "SoftwareAsset", new { assetId = softwareAssetId });
        }

        return RedirectToAction("RequestLicense", new { departmentId = employee.Data.DepartmentID, softwareAssetId });
    }
}
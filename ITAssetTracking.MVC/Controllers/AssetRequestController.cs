using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Enums;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class AssetRequestController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IEmployeeService _employeeService;
    private readonly IAssetService _assetService;
    private readonly IAssetRequestService _assetRequestService;
    private readonly Serilog.ILogger _logger;

    public AssetRequestController(
        IDepartmentService departmentService,
        IEmployeeService employeeService,
        IAssetService assetService,
        IAssetRequestService assetRequestService,
        IAssetAssignmentService assetAssignmentService,
        Serilog.ILogger logger)
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
        _assetService = assetService;
        _assetRequestService = assetRequestService;
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        var requestsResult = _assetRequestService.GetOpenAssetRequests();
        if (!requestsResult.Ok)
        {
            _logger.Error($"Failed to get open asset requests: {requestsResult.Message} => {requestsResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, requestsResult.Message));
            return RedirectToAction("Index", "Home");
        }

        var model = new RequestsIndexModel
        {
            Requests = requestsResult.Data
        };
        
        return View(model);
    }

    [HttpGet]
    public IActionResult Assign(int requestId)
    {
        var requestResult = _assetRequestService.GetAssetRequestById(requestId);
        if (!requestResult.Ok)
        {
            _logger.Error($"Failed to get asset request with id: {requestId}:  {requestResult.Message} => {requestResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, requestResult.Message));
            return RedirectToAction("Index");
        }

        var model = new AssetRequestModel(requestResult.Data);
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Assign(AssetRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            var msg = "Invalid model state while confirming asset assignment request";
            _logger.Warning(msg);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, msg));
            return RedirectToAction("Assign", new { requestId = model.AssetRequestId });
        }
        
        var result = _assetRequestService.ResolveRequest(model.AssetRequestId, RequestResultEnum.Confirmed, model.Note);
        if (!result.Ok)
        {
            _logger.Error($"Failed to add asset assignment: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Assign", new { requestId = model.AssetRequestId });
        }
        
        var successMsg =  $"Asset with Id {model.AssetId} assigned to {(model.EmployeeId != null 
            ? $"Employee with Id {model.EmployeeId}" 
            : $"Department with Id {model.DepartmentId}")}";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Deny(int requestId)
    {
        var requestResult = _assetRequestService.GetAssetRequestById(requestId);
        if (!requestResult.Ok)
        {
            _logger.Error($"Failed to get asset request with id: {requestId}:  {requestResult.Message} => {requestResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, requestResult.Message));
            return RedirectToAction("Index");
        }

        var model = new AssetRequestModel(requestResult.Data);
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Deny(AssetRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            var msg = "Invalid model state while confirming asset assignment request";
            _logger.Warning(msg);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, msg));
            return RedirectToAction("Deny", new { requestId = model.AssetRequestId });
        }
        
        var result = _assetRequestService.ResolveRequest(model.AssetRequestId, RequestResultEnum.Denied, model.Note);
        if (!result.Ok)
        {
            _logger.Error($"Failed to deny request: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Deny", new { requestId = model.AssetRequestId });
        }
        
        var successMsg =  $"Assignment for Asset with Id {model.AssetId} to {(model.EmployeeId != null 
            ? $"Employee with Id {model.EmployeeId}" 
            : $"Department with Id {model.DepartmentId}")} was denied";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Index");
    }

    public IActionResult AvailableAssets(int departmentId, AvailableAssetsModel model)
    {
        var assetsResult = _assetRequestService.GetAvailableAssets();
        if (!assetsResult.Ok)
        {
            _logger.Error($"Failed to get available assets: {assetsResult.Message} => {assetsResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, assetsResult.Message));
            return RedirectToAction("Index", "Asset");
        }

        var assetTypes = assetsResult.Data;
        model.AssetTypesSelectList = new SelectList(assetTypes, "AssetTypeID", "AssetTypeName");

        if (model.SelectedAssetTypeId != null && model.SelectedAssetTypeId > 0)
        {
            model.AvailableAssets = assetTypes
                .Where(a => a.AssetTypeID == model.SelectedAssetTypeId)
                .ToList();
        }
        else
        {
            model.AvailableAssets = assetTypes;
        }

        model.RequestingDepartmentId = departmentId;
        
        return View(model);
    }

    [HttpGet]
    public IActionResult RequestAsset(int departmentId, long assetId, AssetRequestEmployeesModel employeesModel)
    {
        var departmentResult = _departmentService.GetDepartmentById(departmentId);
        var assetResult = _assetService.GetAssetById(assetId);
        
        if (!departmentResult.Ok || !assetResult.Ok)
        {
            var ex = departmentResult.Exception ?? assetResult.Exception;
            string errMsg = "There was an error retrieving data: " + 
                            (departmentResult.Message ?? assetResult.Message ?? "Unknown Error");
            _logger.Error($"Failed to get employees for department {departmentId}: {errMsg} => {ex}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            return RedirectToAction("AvailableAssets", new { departmentId });
        }
        
        employeesModel.DepartmentID = departmentId;
        employeesModel.DepartmentName = departmentResult.Data.DepartmentName;
        employeesModel.AssetId = assetId;
        employeesModel.SerialNumber = assetResult.Data.SerialNumber;

        if (employeesModel.StartingLetter == null || !char.IsLetter((char)employeesModel.StartingLetter))
        {
            employeesModel.Employees = departmentResult.Data.Employees;
        }
        else
        {
            employeesModel.Employees = departmentResult.Data.Employees
                .Where(e => e.LastName.StartsWith((char)employeesModel.StartingLetter))
                .ToList();
        }
        
        return View(employeesModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RequestAsset(AssetRequestEmployeesModel employeesModel)
    {
        var request = new AssetRequest
        {
            AssetID = employeesModel.AssetId,
            DepartmentID = (byte)employeesModel.DepartmentID,
            EmployeeID = employeesModel.EmployeeId,
        };
        var addRequestResult = _assetRequestService.AddAssetRequest(request);
        if (!addRequestResult.Ok)
        {
            _logger.Error($"Failed to create asset request: {addRequestResult.Message} => {addRequestResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, addRequestResult.Message));
            return RedirectToAction("AvailableAssets", new { departmentId = employeesModel.DepartmentID });
        }

        var msg = "Successfully created asset request";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, msg));
        return RedirectToAction("DepartmentAssets", "Asset");
    }
}

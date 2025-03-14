using ITAssetTracking.Core.Entities;
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
        Serilog.ILogger logger)
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
        _assetService = assetService;
        _assetRequestService = assetRequestService;
        _logger = logger;
    }

    public IActionResult AvailableAssets(int departmentId, AvailableAssetsModel model)
    {
        var assetsResult = _assetRequestService.GetAvailableAssets();
        if (!assetsResult.Ok)
        {
            _logger.Error($"Failed to get available assets: " + assetsResult.Exception);
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
    public IActionResult RequestAsset(int departmentId, long assetId, AssetRequestModel model)
    {
        var departmentResult = _departmentService.GetDepartmentById(departmentId);
        var assetResult = _assetService.GetAssetById(assetId);
        
        if (!departmentResult.Ok || !assetResult.Ok)
        {
            var ex = departmentResult.Exception ?? assetResult.Exception;
            string errMsg = "There was an error retrieving data: " + 
                            (departmentResult.Message ?? assetResult.Message ?? "Unknown Error");
            _logger.Error($"Failed to get employees for department {departmentId}: {ex}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            return RedirectToAction("AvailableAssets", new { departmentId });
        }
        
        model.DepartmentID = departmentId;
        model.DepartmentName = departmentResult.Data.DepartmentName;
        model.AssetId = assetId;
        model.SerialNumber = assetResult.Data.SerialNumber;

        if (model.StartingLetter == null || !char.IsLetter((char)model.StartingLetter))
        {
            model.Employees = departmentResult.Data.Employees;
        }
        else
        {
            model.Employees = departmentResult.Data.Employees
                .Where(e => e.LastName.StartsWith((char)model.StartingLetter))
                .ToList();
        }
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RequestAsset(AssetRequestModel model)
    {
        var request = new AssetRequest
        {
            AssetID = model.AssetId,
            DepartmentID = (byte)model.DepartmentID,
            EmployeeID = model.EmployeeId,
        };
        var addRequestResult = _assetRequestService.AddAssetRequest(request);
        if (!addRequestResult.Ok)
        {
            _logger.Error($"Failed to create asset request: {addRequestResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, addRequestResult.Message));
            return RedirectToAction("AvailableAssets", new { departmentId = model.DepartmentID });
        }

        var msg = "Successfully created asset request";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, msg));
        return RedirectToAction("DepartmentAssets", "Asset");
    }
}

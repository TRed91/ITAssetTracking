using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class AssetController : Controller
{
    private readonly IAssetService _assetService;
    private readonly IAssetAssignmentService _assetAssignmentService;
    private readonly Serilog.ILogger _logger;

    public AssetController(
        IAssetService assetService, 
        IAssetAssignmentService assetAssignmentService,
        Serilog.ILogger logger)
    {
        _assetService = assetService;
        _assetAssignmentService = assetAssignmentService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Filter()
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
            
            _logger.Error("Error retrieving data: " + errMsg);
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, "There was an error retrieving data"));
            return RedirectToAction("Index", "Home");
        }

        var model = new AssetFilterModel
        {
            AssetTypes = new SelectList(assetTypesRes.Data, "AssetTypeID", "AssetTypeName"),
            Manufacturers = new SelectList(manufacturersRes.Data, "ManufacturerID", "ManufacturerName"),
            Locations = new SelectList(locationsRes.Data, "LocationID", "LocationName"),
            AssetStatuses = new SelectList(statusesRes.Data, "AssetStatusID", "AssetStatusName")
        };
        return View(model);
    }

    public IActionResult All(AssetFilterModel model)
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
            return RedirectToAction("Index", "Home");
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
            
            _logger.Error("Error retrieving data: " + errMsg);
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, "There was an error retrieving data"));
            return RedirectToAction("Index", "Home");
        }

        var assets = assetsResult.Data;
        
        // filter retired assets
        if (!model.IncludeRetired)
        {
            assets = assets
                .Where(a => a.AssetStatus.AssetStatusName != "Retired")
                .ToList();
        }

        // filter by search string
        if (!string.IsNullOrEmpty(model.Search))
        {
            assets = assets
                .Where(a => a.SerialNumber.Contains(model.Search) || a.Model.ModelNumber.Contains(model.Search))
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
        model.AssetTypes = new SelectList(assetTypesRes.Data, "AssetTypeID", "AssetTypeName");
        model.Manufacturers = new SelectList(manufacturersRes.Data, "ManufacturerID", "ManufacturerName");
        model.Locations = new SelectList(locationsRes.Data, "LocationID", "LocationName");
        model.AssetStatuses = new SelectList(statusesRes.Data, "AssetStatusID", "AssetStatusName");
        
        return  View(model);
    }
}
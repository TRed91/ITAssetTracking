using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class AssetController : Controller
{
    private readonly IAssetService _assetService;
    private readonly Serilog.ILogger _logger;

    public AssetController(
        IAssetService assetService, 
        Serilog.ILogger logger)
    {
        _assetService = assetService;
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

    public IActionResult Manufacturers(ManufacturersModel model)
    {
        var manufacturersResult = _assetService.GetManufacturers();
        if (!manufacturersResult.Ok)
        {
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, manufacturersResult.Message));
            _logger.Error("Error retrieving data: " + manufacturersResult.Exception);
            return RedirectToAction("Index", "Home");
        }
        var manufacturers = manufacturersResult.Data;
        if (!string.IsNullOrEmpty(model.Search))
        {
            manufacturers = manufacturers
                .Where(m => m.ManufacturerName.Contains(model.Search))
                .ToList();
        }
        model.Manufacturers = manufacturers;
        return View(model);
    }

    [HttpGet]
    public IActionResult Add(int manufacturerId)
    {
        string? manufacturerName = null;
        var models = new List<Model>();
        var locationsResult = _assetService.GetLocations();
        var assetTypesResult = _assetService.GetAssetTypes();
        if (!assetTypesResult.Ok || !locationsResult.Ok)
        {
            _logger.Error("Error retrieving data: " + (assetTypesResult.Exception ?? locationsResult.Exception));
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, assetTypesResult.Message ?? locationsResult.Message ?? "Unknown Error"));
        }
        
        models = _assetService.GetModelsByManufacturer(manufacturerId).Data;
        manufacturerName = _assetService.GetManufacturerById(manufacturerId).Data.ManufacturerName;

        var model = new AssetFormModel
        {
            Manufacturer = manufacturerName,
            ManufacturerId = manufacturerId,
            Models = new SelectList(models, "ModelID", "ModelNumber"),
            AssetTypes = new SelectList(assetTypesResult.Data, "AssetTypeID", "AssetTypeName"),
            Locations = new SelectList(locationsResult.Data, "LocationID", "LocationName")
        };
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(AssetFormModel formModel)
    {
        if (!ModelState.IsValid)
        {
            if (formModel.ManufacturerId != null && formModel.ManufacturerId != 0)
            {
                var models = _assetService.GetModelsByManufacturer((int)formModel.ManufacturerId).Data;
                formModel.Models = new SelectList(models, "ModelID", "ModelNumber");
            }
            
            var locations = _assetService.GetLocations().Data;
            var assetTypes = _assetService.GetAssetTypes().Data;
            formModel.AssetTypes = new SelectList(assetTypes, "AssetTypeID", "AssetTypeName");
            formModel.Locations = new SelectList(locations, "LocationID", "LocationName");
            
            return View(formModel);
        }

        Asset asset = formModel.ToEntity();
        var result = _assetService.AddAsset(asset);
        if (!result.Ok)
        {
            _logger.Error("Error adding asset: " + result.Exception);
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, result.Message));
            
            RedirectToAction("Add", new { manufacturerId = formModel.ManufacturerId });
        }
        
        _logger.Information($"Asset with id {asset.AssetID} added successfully");
        TempData["msg"] = TempDataExtension.Serialize(
            new TempDataMsg(true, $"Asset with id {asset.AssetID} added successfully"));
       
        //TODO redirect to details instead
        return RedirectToAction("Details", new { assetId = asset.AssetID });
    }

    [HttpGet]
    public IActionResult Edit(long assetId)
    {
        var assetResult = _assetService.GetAssetById(assetId);
        if (!assetResult.Ok)
        {
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, assetResult.Message));
            return RedirectToAction("Details", new { assetId });
        }
        
        var model = new AssetFormModel(assetResult.Data);
        
        //populate select lists
        var modelsResult = _assetService.GetModelsByManufacturer(assetResult.Data.ManufacturerID);
        var assetTypesResult = _assetService.GetAssetTypes();
        var locationsResult = _assetService.GetLocations();
        model.Locations = new SelectList(locationsResult.Data, "LocationID", "LocationName");
        model.AssetTypes = new SelectList(assetTypesResult.Data, "AssetTypeID", "AssetTypeName");
        model.Models = new SelectList(modelsResult.Data, "ModelID", "ModelNumber");
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(long assetId, AssetFormModel model)
    {
        if (ModelState.IsValid)
        {
            var originalAsset = _assetService.GetAssetById(assetId).Data;
            var asset = model.ToEntity();
            asset.AssetID = assetId;
            asset.AssetStatusID = originalAsset.AssetStatusID;
            var updateResult = _assetService.UpdateAsset(asset);
            if (updateResult.Ok)
            {
                var msg = $"Asset with id {assetId} updated successfully";
                _logger.Information(msg);
                TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, msg));
            }
            else
            {
                _logger.Error("Error updating asset: " + updateResult.Exception);
                TempData["msg"] = TempDataExtension.Serialize(
                    new TempDataMsg(false, updateResult.Message));
            }
            return RedirectToAction("Details", new { assetId });
        }
        //populate select lists
        var models = _assetService.GetModelsByManufacturer(model.ManufacturerId).Data;
        var locations = _assetService.GetLocations().Data;
        var assetTypes = _assetService.GetAssetTypes().Data;
        model.AssetTypes = new SelectList(assetTypes, "AssetTypeID", "AssetTypeName");
        model.Locations = new SelectList(locations, "LocationID", "LocationName");
        model.Models = new SelectList(models, "ModelID", "ModelNumber");
        
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(long assetId)
    {
        var assetResult = _assetService.GetAssetById(assetId);
        if (!assetResult.Ok)
        {
            _logger.Error("Error retrieving asset: " + assetResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, assetResult.Message));
            return RedirectToAction("Details", new { assetId });
        }
        return View(assetResult.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteAsset(long assetId)
    {
        var deleteResult = _assetService.DeleteAsset(assetId);
        if (!deleteResult.Ok)
        {
            _logger.Error("Error deleting asset: " + deleteResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, deleteResult.Message));
            return RedirectToAction("Details", new { assetId });
        }
        
        var msg = $"Asset with id {assetId} deleted successfully";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, msg));
        return RedirectToAction("Index");
    }
    
    public IActionResult Details(long assetId)
    {
        var assetResult = _assetService.GetAssetById(assetId);
        
        if (!assetResult.Ok)
        {
            _logger.Error("Error retrieving data: " + assetResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, assetResult.Message));
            return RedirectToAction("Index", "Asset");
        }
        
        var model = new AssetDetailsModel(assetResult.Data);
        return View(model);
    }
}
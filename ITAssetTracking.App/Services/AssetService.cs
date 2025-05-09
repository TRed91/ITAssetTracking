﻿using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetRepo;
    private readonly IAssetAssignmentRepository _assignmentRepo;

    public AssetService(IAssetRepository assetRepository, IAssetAssignmentRepository assignmentRepository)
    {
        _assetRepo = assetRepository;
        _assignmentRepo = assignmentRepository;
    }
    
    public Result<Asset> GetAssetById(long assetId)
    {
        try
        {
            var asset = _assetRepo.GetAssetById(assetId);
            if (asset == null)
            {
                return ResultFactory.Fail<Asset>("Asset not found");
            }

            return ResultFactory.Success(asset);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Asset>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetAllAssets(int assetTypeId = 0, int locationId = 0, int assetStatusId = 0, int manufacturerId = 0)
    {
        try
        {
            var assets = _assetRepo.GetAssets(assetTypeId, locationId, assetStatusId, manufacturerId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetDepartmentAssets(int departmentId)
    {
        try
        {
            var assets = _assetRepo.GetDepartmentAssets(departmentId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetEmployeeAssets(int employeeId)
    {
        try
        {
            var assets = _assetRepo.GetEmployeeAssets(employeeId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetAvailableAssets()
    {
        try
        {
            var assets = _assetRepo.GetAvailableAssets();
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        } 
    }

    public Result<List<Asset>> GetAssetsByType(int assetTypeId)
    {
        try
        {
            var assets = _assetRepo.GetAssetsByType(assetTypeId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetAssetsByLocation(int locationId)
    {
        try
        {
            var assets = _assetRepo.GetAssetsByLocation(locationId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetAssetsByModel(int modelId)
    {
        try
        {
            var assets = _assetRepo.GetAssetsByModel(modelId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetAssetsByManufacturer(int manufacturerId)
    {
        try
        {
            var assets = _assetRepo.GetAssetsByManufacturer(manufacturerId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetAssetsByStatus(int statusId)
    {
        try
        {
            var assets = _assetRepo.GetAssetsByStatus(statusId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result<List<Asset>> GetAssetsInDateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<List<Asset>>("'Start date' cannot be after 'end date'");
        }
        try
        {
            var assets = _assetRepo.GetAssetsInDateRange(startDate, endDate);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Asset>>(ex.Message, ex);
        }
    }

    public Result AddAsset(Asset asset)
    {
        try
        {
            var duplicate = _assetRepo.GetAssetBySerialNumber(asset.SerialNumber);
            if (duplicate != null)
            {
                return ResultFactory.Fail($"Asset with Serial Number {asset.SerialNumber} already exists");
            }

            // Default asset status to storage
            var assetStatus = _assetRepo.GetAssetStatusByName("Storage");
            if (assetStatus == null)
            {
                return ResultFactory.Fail($"Storage Status ID not found");
            }
            asset.AssetStatusID = assetStatus.AssetStatusID;
            
            _assetRepo.AddAsset(asset);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateAsset(Asset asset)
    {
        try
        {
            var originalAsset = _assetRepo.GetAssetById(asset.AssetID);
            if (originalAsset == null)
            {
                return ResultFactory.Fail($"Asset with id {asset.AssetID} not found");
            }
            
            _assetRepo.UpdateAsset(asset);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteAsset(long assetId)
    {
        try
        {
            var asset = _assetRepo.GetAssetById(assetId);
            if (asset == null)
            {
                return ResultFactory.Fail($"Asset with id {assetId} not found");
            }

            _assetRepo.DeleteAsset(assetId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result<List<Manufacturer>> GetManufacturers()
    {
        try
        {
            var manufacturers = _assetRepo.GetManufacturers();
            return ResultFactory.Success(manufacturers);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Manufacturer>>(ex.Message, ex);
        }
    }

    public Result<Manufacturer> GetManufacturerById(int manufacturerId)
    {
        try
        {
            var manufacturer = _assetRepo.GetManufacturerById(manufacturerId);
            if (manufacturer == null)
            {
                return ResultFactory.Fail<Manufacturer>($"Manufacturer with id {manufacturerId} not found");
            }

            return ResultFactory.Success(manufacturer);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Manufacturer>(ex.Message, ex);
        }
    }

    public Result<List<Model>> GetModels()
    {
        try
        {
            var models = _assetRepo.GetModels();
            return ResultFactory.Success(models);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Model>>(ex.Message, ex);
        }
    }

    public Result<List<Model>> GetModelsByManufacturer(int manufacturerId)
    {
        try
        {
            var models = _assetRepo.GetModelsByManufacturer(manufacturerId);
            return ResultFactory.Success(models);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Model>>(ex.Message, ex);
        }
    }

    public Result<List<Model>> GetModelsWithNoManufacturer()
    {
        try
        {
            var models = _assetRepo.GetModelsWithNoManufacturer();
            return ResultFactory.Success(models);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Model>>(ex.Message, ex);
        }
    }

    public Result<List<AssetType>> GetAssetTypes()
    {
        try
        {
            var assetTypes = _assetRepo.GetAssetTypes();
            return ResultFactory.Success(assetTypes);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetType>>(ex.Message, ex);
        }
    }

    public Result<List<Location>> GetLocations()
    {
        try
        {
            var locations = _assetRepo.GetLocations();
            return ResultFactory.Success(locations);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Location>>(ex.Message, ex);
        }
    }

    public Result<List<AssetStatus>> GetAssetStatuses()
    {
        try
        {
            var assetStatuses = _assetRepo.GetAssetStatuses();
            return ResultFactory.Success(assetStatuses);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetStatus>>(ex.Message, ex);
        }
    }
}
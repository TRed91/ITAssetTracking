using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetRepo;

    public AssetService(IAssetRepository assetRepository)
    {
        _assetRepo = assetRepository;
    }
    
    public Result<Asset> GetAssetById(int assetId)
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

    public Result<List<Asset>> GetAllAssets(int assetTypeId = 0, int locationId = 0, int modelId = 0, int manufacturerId = 0)
    {
        try
        {
            var assets = _assetRepo.GetAssets(assetTypeId, locationId, modelId, manufacturerId);
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

            originalAsset.AssetStatusID = asset.AssetStatusID;
            originalAsset.ManufacturerID = asset.ManufacturerID;
            originalAsset.ModelID = asset.ModelID;
            originalAsset.LocationID = asset.LocationID;
            originalAsset.SerialNumber = asset.SerialNumber;
            originalAsset.AssetTypeID = asset.AssetTypeID;
            originalAsset.PurchasePrice = asset.PurchasePrice;

            _assetRepo.UpdateAsset(originalAsset);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteAsset(int assetId)
    {
        try
        {
            var asset = _assetRepo.GetAssetById(assetId);
            if (asset == null)
            {
                return ResultFactory.Fail($"Asset with id {assetId} not found");
            }

            _assetRepo.DeleteAsset(asset);
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
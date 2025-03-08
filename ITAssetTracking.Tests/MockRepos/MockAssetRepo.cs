using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockAssetRepo : IAssetRepository
{
    private readonly MockDB _db;

    public MockAssetRepo()
    {
        _db = new MockDB();
    }
    
    public Asset? GetAssetById(long assetId)
    {
        return _db.Assets.FirstOrDefault(a => a.AssetID == assetId);
    }

    public Asset? GetAssetBySerialNumber(string serialNumber)
    {
        return _db.Assets.FirstOrDefault(a => a.SerialNumber == serialNumber);
    }

    public List<Asset> GetAssets(int assetTypeId = 0, int locationId = 0, int modelId = 0, int manufacturerId = 0)
    {
        return _db.Assets.Where(a => 
            (assetTypeId != 0 ? a.AssetTypeID == assetTypeId : a.AssetTypeID > 0) &&
            (locationId != 0 ? a.LocationID == locationId : a.LocationID > 0) && 
            (modelId != 0 ? a.ModelID == modelId : a.ModelID > 0) &&
            (manufacturerId != 0 ? a.ManufacturerID == manufacturerId : a.ManufacturerID > 0))
            .ToList();
    }

    public List<Asset> GetAssetsByType(int assetTypeId)
    {
        return _db.Assets.Where(a => a.AssetTypeID == assetTypeId).ToList();
    }

    public List<Asset> GetAssetsByLocation(int locationId)
    {
        return _db.Assets.Where(a => a.LocationID == locationId).ToList();
    }

    public List<Asset> GetAssetsByModel(int modelId)
    {
        return _db.Assets.Where(a => a.ModelID == modelId).ToList();
    }

    public List<Asset> GetAssetsByManufacturer(int manufacturerId)
    {
        return _db.Assets.Where(a => a.ManufacturerID == manufacturerId).ToList();
    }

    public List<Asset> GetAssetsByStatus(int assetStatusId)
    {
        return _db.Assets.Where(a => a.AssetStatusID == assetStatusId).ToList();
    }

    public List<Asset> GetAssetsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _db.Assets.Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate).ToList();
    }

    public void AddAsset(Asset asset)
    {
        asset.AssetID = _db.Assets.Max(a => a.AssetID) + 1;
        _db.Assets.Add(asset);
    }

    public void UpdateAsset(Asset asset)
    {
        var assetToUpdate = _db.Assets.FirstOrDefault(a => a.AssetID == asset.AssetID);
        assetToUpdate.AssetStatusID = asset.AssetStatusID;
        assetToUpdate.PurchaseDate = asset.PurchaseDate;
        assetToUpdate.PurchasePrice = asset.PurchasePrice;
        assetToUpdate.AssetTypeID = asset.AssetTypeID;
        assetToUpdate.ModelID = asset.ModelID;
        assetToUpdate.ManufacturerID = asset.ManufacturerID;
        assetToUpdate.LocationID = asset.LocationID;
        assetToUpdate.SerialNumber = asset.SerialNumber;
    }

    public void DeleteAsset(long assetId)
    {
        var asset = _db.Assets.FirstOrDefault(a => a.AssetID == assetId);
        _db.Assets.Remove(asset);
    }

    public List<Manufacturer> GetManufacturers()
    {
        throw new NotImplementedException();
    }

    public Manufacturer? GetManufacturerById(int manufacturerId)
    {
        throw new NotImplementedException();
    }

    public List<Model> GetModels()
    {
        throw new NotImplementedException();
    }

    public List<Model> GetModelsByManufacturer(int manufacturerId)
    {
        throw new NotImplementedException();
    }

    public List<Model> GetModelsWithNoManufacturer()
    {
        throw new NotImplementedException();
    }

    public List<AssetType> GetAssetTypes()
    {
        throw new NotImplementedException();
    }

    public List<Location> GetLocations()
    {
        return _db.Locations.ToList();
    }

    public List<AssetStatus> GetAssetStatuses()
    {
        throw new NotImplementedException();
    }

    public AssetStatus? GetAssetStatusByName(string assetStatusName)
    {
        return new AssetStatus
        {
            AssetStatusID = 1,
            AssetStatusName = "Storage"
        };
    }
}
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IAssetRepository
{
    Asset? GetAssetById(long assetId);
    Asset? GetAssetBySerialNumber(string serialNumber);
    
    List<Asset> GetAssets(int assetTypeId = 0, int locationId = 0, int modelId = 0, int manufacturerId = 0);
    List<Asset> GetAssetsByType(int assetTypeId);
    List<Asset> GetAssetsByLocation(int locationId);
    List<Asset> GetAssetsByModel(int modelId);
    List<Asset> GetAssetsByManufacturer(int manufacturerId);
    List<Asset> GetAssetsByStatus(int assetStatusId);
    List<Asset> GetAssetsInDateRange(DateTime startDate, DateTime endDate);
    
    void AddAsset(Asset asset);
    void UpdateAsset(Asset asset);
    void DeleteAsset(Asset asset);
    
    List<Manufacturer> GetManufacturers();
    List<Model> GetModels();
    List<Model> GetModelsByManufacturer(int manufacturerId);
    List<AssetType> GetAssetTypes();
    List<Location> GetLocations();
    List<AssetStatus> GetAssetStatuses();
}
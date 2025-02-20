using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IAssetRepository
{
    Asset? GetAssetById(int assetId);
    
    List<Asset> GetAssets();
    List<Asset> GetAssetsByType(int assetTypeId);
    List<Asset> GetAssetsByLocation(int locationId);
    List<Asset> GetAssetsByModel(int modelId);
    List<Asset> GetAssetsByManufacturer(int manufacturerId);
    List<Asset> GetAssetsByStatus(int assetStatusId);
    List<Asset> GetAssetsInDateRange(DateTime startDate, DateTime endDate);
    
    void AddAsset(Asset asset);
    void UpdateAsset(Asset asset);
    void DeleteAsset(Asset asset);
}
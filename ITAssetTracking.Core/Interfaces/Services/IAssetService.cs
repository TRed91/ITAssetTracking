using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface IAssetService
{
    Result<Asset> GetAssetById(long assetId);
    
    Result<List<Asset>> GetAllAssets(int assetTypeId, int locationId, int assetStatusId, int manufacturerId);
    Result<List<Asset>> GetDepartmentAssets(int departmentId);
    Result<List<Asset>> GetEmployeeAssets(int employeeId);
    Result<List<Asset>> GetAvailableAssets();
    Result<List<Asset>> GetAssetsByType(int assetTypeId);
    Result<List<Asset>> GetAssetsByLocation(int locationId);
    Result<List<Asset>> GetAssetsByModel(int modelId);
    Result<List<Asset>> GetAssetsByManufacturer(int manufacturerId);
    Result<List<Asset>> GetAssetsByStatus(int statusId);
    Result<List<Asset>> GetAssetsInDateRange(DateTime startDate, DateTime endDate);
    
    Result AddAsset(Asset asset);
    Result UpdateAsset(Asset asset);
    Result DeleteAsset(long assetId);
    
    Result<List<Manufacturer>> GetManufacturers();
    Result<Manufacturer> GetManufacturerById(int manufacturerId);
    Result<List<Model>> GetModels();
    Result<List<Model>> GetModelsByManufacturer(int manufacturerId);
    Result<List<Model>> GetModelsWithNoManufacturer();
    Result<List<AssetType>> GetAssetTypes();
    Result<List<Location>> GetLocations();
    Result<List<AssetStatus>> GetAssetStatuses();
}
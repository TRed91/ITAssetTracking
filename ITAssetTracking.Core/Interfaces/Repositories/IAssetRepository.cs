﻿using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IAssetRepository
{
    Asset? GetAssetById(long assetId);
    Asset? GetAssetBySerialNumber(string serialNumber);
    
    List<Asset> GetAssets(int assetTypeId = 0, int locationId = 0, int assetStatusId = 0, int manufacturerId = 0);
    List<Asset> GetDepartmentAssets(int departmentId);
    List<Asset> GetEmployeeAssets(int employeeId);
    List<Asset> GetAssetsByType(int assetTypeId);
    List<Asset> GetAssetsByLocation(int locationId);
    List<Asset> GetAssetsByModel(int modelId);
    List<Asset> GetAssetsByManufacturer(int manufacturerId);
    List<Asset> GetAssetsByStatus(int assetStatusId);
    List<Asset> GetAssetsInDateRange(DateTime startDate, DateTime endDate);
    
    void AddAsset(Asset asset);
    void UpdateAsset(Asset asset);
    void DeleteAsset(long assetId);
    
    List<Manufacturer> GetManufacturers();
    Manufacturer? GetManufacturerById(int manufacturerId);
    List<Model> GetModels();
    List<Model> GetModelsByManufacturer(int manufacturerId);
    List<Model> GetModelsWithNoManufacturer();
    List<AssetType> GetAssetTypes();
    List<Location> GetLocations();
    List<AssetStatus> GetAssetStatuses();
    AssetStatus? GetAssetStatusByName(string assetStatusName);
    List<Asset> GetAvailableAssets();
}
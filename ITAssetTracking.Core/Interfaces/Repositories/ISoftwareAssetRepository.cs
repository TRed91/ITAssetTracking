﻿using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ISoftwareAssetRepository
{
    SoftwareAsset? GetSoftwareAsset(int softwareAssetId);
    
    List<SoftwareAsset> GetSoftwareAssets(int licenseTypeId, int manufacturerId, int assetStatusId, bool includeExpired);
    List<SoftwareAsset> GetSoftwareAssetsByManufacturer(int manufacturerId, bool includeExpired);
    List<SoftwareAsset> GetSoftwareAssetsByType(int licenseTypeId, bool includeExpired);
    List<SoftwareAsset> GetSoftwareAssetsByStatus(int assetStatusId);
    List<SoftwareAsset> GetSoftwareAssetsByEmployeeId(int employeeId);
    
    void AddSoftwareAsset(SoftwareAsset softwareAsset);
    void UpdateSoftwareAsset(SoftwareAsset softwareAsset);
    void DeleteSoftwareAsset(int softwareAssetId);
    
    List<LicenseType> GetLicenseTypes();
    List<LicenseType> GetLicenseTypesByManufacturer(int manufacturerId);
    LicenseType? GetLicenseTypeById(int licenseTypeId);
    List<Manufacturer> GetSoftwareManufacturers();
    
    List<SoftwareAsset> GetAvailableAssets();
}
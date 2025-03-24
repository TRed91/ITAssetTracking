using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface ISoftwareAssetService
{
    Result<SoftwareAsset> GetSoftwareAsset(int softwareAssetId);
    
    Result<List<SoftwareAsset>> GetSoftwareAssets(int licenseTypeId, int assetStatusId, int manufacturerId, bool includeExpired = true);
    Result<List<SoftwareAsset>> GetSoftwareAssetsByManufacturer(int manufacturerId, bool includeExpired = true);
    Result<List<SoftwareAsset>> GetSoftwareAssetsByType(int licenseTypeId, bool includeExpired = true);
    Result<List<SoftwareAsset>> GetSoftwareAssetsByStatus(int statusId);
    
    Result AddSoftwareAsset(SoftwareAsset softwareAsset);
    Result UpdateSoftwareAsset(SoftwareAsset softwareAsset);
    Result DeleteSoftwareAsset(int softwareAssetId);
    
    Result<List<LicenseType>> GetLicenseTypes();
    Result<List<LicenseType>> GetLicenseTypesByManufacturer(int manufacturerId);
    Result<LicenseType> GetLicenseTypeById(int licenseTypeId);
    Result<List<Manufacturer>> GetLicenseTypesByManufacturers();
}
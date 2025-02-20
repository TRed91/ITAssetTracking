using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ISoftwareAssetRepository
{
    SoftwareAsset? GetSoftwareAsset(int softwareAssetId);
    
    List<SoftwareAsset> GetSoftwareAssets(bool includeExpired);
    List<SoftwareAsset> GetSoftwareAssetsByManufacturer(int manufacturerId, bool includeExpired);
    List<SoftwareAsset> GetSoftwareAssetsByType(int licenseTypeId, bool includeExpired);
    List<SoftwareAsset> GetSoftwareAssetsByStatus(int assetStatusId);
    
    void AddSoftwareAsset(SoftwareAsset softwareAsset);
    void UpdateSoftwareAsset(SoftwareAsset softwareAsset);
    void DeleteSoftwareAsset(SoftwareAsset softwareAsset);
    
    List<LicenseType> GetLicenseTypes();
    List<LicenseType> GetLicenseTypesByManufacturer(int manufacturerId);
}
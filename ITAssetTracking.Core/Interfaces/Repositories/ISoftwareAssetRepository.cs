using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ISoftwareAssetRepository
{
    SoftwareAsset? GetSoftwareAsset(int softwareAssetId);
    
    List<SoftwareAsset> GetSoftwareAssets();
    List<SoftwareAsset> GetSoftwareAssetsByManufacturer(int manufacturerId);
    List<SoftwareAsset> GetSoftwareAssetsByType(int licenseTypeId);
    List<SoftwareAsset> GetSoftwareAssetsByStatus(int assetStatusId);
    
    void AddSoftwareAsset(SoftwareAsset softwareAsset);
    void UpdateSoftwareAsset(SoftwareAsset softwareAsset);
    void DeleteSoftwareAsset(SoftwareAsset softwareAsset);
    
    List<LicenseType> GetLicenseTypes();
    List<LicenseType> GetLicenseTypesByManufacturer(int manufacturerId);
}
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class SoftwareAssetModel
{
    public int SoftwareAssetId { get; set; }
    public string Manufacturer { get; set; }
    public string LicenseType { get; set; }
    public string AssetStatus { get; set; }
    public string LicenseKey { get; set; }
    public int NumberOfLicenses { get; set; }
    public string Version { get; set; }
    public DateTime ExpirationDate { get; set; }

    public SoftwareAssetModel(SoftwareAsset entity)
    {
        SoftwareAssetId = entity.SoftwareAssetID;
        Manufacturer = entity.Manufacturer.ManufacturerName;
        LicenseType = entity.LicenseType.LicenseTypeName;
        AssetStatus = entity.AssetStatus.AssetStatusName;
        LicenseKey = entity.LicenseKey;
        NumberOfLicenses = entity.NumberOfLicenses;
        Version = entity.Version;
        ExpirationDate = entity.ExpirationDate;
    }
}
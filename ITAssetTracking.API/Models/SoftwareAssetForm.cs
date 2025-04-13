using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class SoftwareAssetForm
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ManufacturerId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int LicenseTypeId { get; set; }
    public byte? AssetStatusId { get; set; }
    [Required]
    [MaxLength(50)]
    public string LicenseKey { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int NumberOfLicenses { get; set; }
    [Required]
    [MaxLength(10)]
    public string Version { get; set; }
    [Required]
    public DateTime ExpirationDate { get; set; }

    public SoftwareAssetForm() { }

    public SoftwareAssetForm(SoftwareAsset entity)
    {
        ManufacturerId = entity.ManufacturerID;
        LicenseTypeId = entity.LicenseTypeID;
        AssetStatusId = entity.AssetStatusID;
        LicenseKey = entity.LicenseKey;
        NumberOfLicenses = entity.NumberOfLicenses;
        Version = entity.Version;
        ExpirationDate = entity.ExpirationDate;
    }

    public SoftwareAsset ToEntity()
    {
        var asset = new SoftwareAsset
        {
            ManufacturerID = ManufacturerId,
            LicenseTypeID = LicenseTypeId,
            LicenseKey = LicenseKey,
            NumberOfLicenses = NumberOfLicenses,
            Version = Version,
            ExpirationDate = ExpirationDate
        };
        if (AssetStatusId.HasValue)
        {
            asset.AssetStatusID = AssetStatusId.Value;
        }
        return asset;
    }
}
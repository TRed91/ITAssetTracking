using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class SoftwareAssetFormModel
{
    public int? SoftwareAssetId { get; set; }
    public int ManufacturerId { get; set; }
    public int LicenseTypeId { get; set; }
    
    [Required]
    [Display(Name = "License Key")]
    public string LicenseKey { get; set; }
    
    [Required]
    [Display(Name = "Number of Licenses")]
    [Range(1, int.MaxValue)]
    public int NumberOfLicenses { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string Version { get; set; }
    
    [Required]
    [Display(Name = "Expiration Date")]
    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; } = DateTime.Today;

    [Display(Name = "Asset Status")] 
    public byte AssetStatusId { get; set; } = 0;
    
    public string ManufacturerName { get; set; }
    public string LicenseName { get; set; }
    
    public SelectList? AssetStatuses { get; set; }

    public SoftwareAssetFormModel() { }

    public SoftwareAssetFormModel(SoftwareAsset entity)
    {
        SoftwareAssetId = entity.SoftwareAssetID;
        ManufacturerId = entity.ManufacturerID;
        LicenseTypeId = entity.LicenseTypeID;
        AssetStatusId = entity.AssetStatusID;
        LicenseKey = entity.LicenseKey;
        NumberOfLicenses = entity.NumberOfLicenses;
        Version = entity.Version;
        ExpirationDate = entity.ExpirationDate;
        ManufacturerName = entity.Manufacturer.ManufacturerName;
        LicenseName = entity.LicenseType.LicenseTypeName;
    }

    public SoftwareAsset ToEntity()
    {
        return new SoftwareAsset
        {
            ManufacturerID = ManufacturerId,
            LicenseTypeID = LicenseTypeId,
            AssetStatusID = AssetStatusId,
            LicenseKey = LicenseKey,
            NumberOfLicenses = NumberOfLicenses,
            Version = Version,
            ExpirationDate = ExpirationDate,
        };
    }
}
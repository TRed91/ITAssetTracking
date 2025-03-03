using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class Manufacturer
{
    [Key]
    public int ManufacturerID { get; set; }
    public string ManufacturerName { get; set; }
    
    public List<Model> Models { get; set; } = new List<Model>();
    public List<LicenseType> LicenseTypes { get; set; } = new List<LicenseType>();
    public List<Asset> Assets { get; set; } = new List<Asset>();
    public List<SoftwareAsset> SoftwareAssets { get; set; } = new List<SoftwareAsset>();
}
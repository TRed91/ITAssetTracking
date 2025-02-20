namespace ITAssetTracking.Core.Entities;

public class Manufacturer
{
    public int ManufacturerID { get; set; }
    public string ManufacturerName { get; set; }
    
    List<Model> Models { get; set; } = new List<Model>();
    List<LicenseType> LicenseTypes { get; set; } = new List<LicenseType>();
    List<Asset> Assets { get; set; } = new List<Asset>();
    List<SoftwareAsset> SoftwareAssets { get; set; } = new List<SoftwareAsset>();
}
namespace ITAssetTracking.Core.Entities;

public class SoftwareAsset
{
    public int SoftwareAssetID { get; set; }
    
    public int ManufacturerID { get; set; }
    public int LicenseTypeID { get; set; }
    public byte AssetStatusID { get; set; }
    
    public string LicenseKey { get; set; }
    public int NumberOfLicenses { get; set; }
    public string Version { get; set; }
    public DateTime ExpirationDate { get; set; }
}
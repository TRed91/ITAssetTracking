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
    
    Manufacturer Manufacturer { get; set; }
    LicenseType LicenseType { get; set; }
    AssetStatus AssetStatus { get; set; }
    
    List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; }
    List<SoftwareAssetAssignment> SoftwareAssetAssignments { get; set; }
}
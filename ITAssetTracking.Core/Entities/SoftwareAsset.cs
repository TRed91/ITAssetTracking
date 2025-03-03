using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class SoftwareAsset
{
    [Key]
    public int SoftwareAssetID { get; set; }
    
    public int ManufacturerID { get; set; }
    public int LicenseTypeID { get; set; }
    public byte AssetStatusID { get; set; }
    
    public string LicenseKey { get; set; }
    public int NumberOfLicenses { get; set; }
    public string Version { get; set; }
    public DateTime ExpirationDate { get; set; }
    
    public Manufacturer Manufacturer { get; set; }
    public LicenseType LicenseType { get; set; }
    public AssetStatus AssetStatus { get; set; }
    
    public List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; }
    public List<SoftwareAssetAssignment> SoftwareAssetAssignments { get; set; }
}
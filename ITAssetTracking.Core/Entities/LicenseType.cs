using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class LicenseType
{
    [Key]
    public int LicenseTypeID { get; set; }
    public int ManufacturerID { get; set; }
    public string LicenseTypeName { get; set; }
    
    public Manufacturer Manufacturer { get; set; }
    public List<SoftwareAsset> SoftwareAssets { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class AssetStatus
{
    [Key]
    public byte AssetStatusID { get; set; }
    public string AssetStatusName { get; set; }
    
    List<Asset> Assets { get; set; } = new List<Asset>();
    List<SoftwareAsset> SoftwareAssets { get; set; } = new List<SoftwareAsset>();
}
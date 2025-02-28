using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class AssetType
{
    [Key]
    public byte AssetTypeID { get; set; }
    public string AssetTypeName { get; set; }
    
    List<Asset> Assets = new List<Asset>();
}
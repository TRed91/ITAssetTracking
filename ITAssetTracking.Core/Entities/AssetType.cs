using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class AssetType
{
    [Key]
    public byte AssetTypeID { get; set; }
    public string AssetTypeName { get; set; }
    
    public List<Asset> Assets = new List<Asset>();
}
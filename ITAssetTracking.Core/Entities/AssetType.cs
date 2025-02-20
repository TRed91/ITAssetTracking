namespace ITAssetTracking.Core.Entities;

public class AssetType
{
    public byte AssetTypeID { get; set; }
    public string AssetTypeName { get; set; }
    
    List<Asset> Assets = new List<Asset>();
}
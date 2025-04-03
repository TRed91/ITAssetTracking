namespace ITAssetTracking.Core.Models;

public class AssetStatusReport
{
    public string AssetTypeName { get; set; }
    public int NumberOfAssetsTotal { get; set; }
    public int NumberOfAssetsInUse { get; set; }
    public int NumberOfAssetsStorage { get; set; }
    public int NumberOfAssetsRepair { get; set; }
}
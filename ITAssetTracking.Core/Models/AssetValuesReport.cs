namespace ITAssetTracking.Core.Models;

public class AssetValuesReport
{
    public decimal TotalValue { get; set; }
    public List<AssetValuesReportItem> Items { get; set; }
}

public class AssetValuesReportItem
{
    public string AssetTypeName { get; set; }
    public int NumberOfAssets { get; set; }
    public decimal AssetsValue { get; set; }
}
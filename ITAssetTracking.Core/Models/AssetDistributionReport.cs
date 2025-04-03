namespace ITAssetTracking.Core.Models;

public class AssetDistributionReport
{
    public List<AssetDistributionReportItem> Items { get; set; }
    public string DepartmentName { get; set; }
}

public class AssetDistributionReportItem
{
    public string AssetTypeName { get; set; }
    public int NumberOfAssets { get; set; }
}
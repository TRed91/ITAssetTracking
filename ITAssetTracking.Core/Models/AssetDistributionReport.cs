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

public class SoftwareAssetDistributionReport
{
    public string DepartmentName { get; set; }
    public List<SoftwareAssetDistributionReportItem> Items { get; set; }
}

public class SoftwareAssetDistributionReportItem
{
    public string LicenseTypeName { get; set; }
    public int NumberOfLicenses { get; set; }
}
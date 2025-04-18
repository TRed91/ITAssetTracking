using ITAssetTracking.Core.Models;

namespace ITAssetTracking.API.Controllers;

public class AssetReportsModel
{
    public List<AssetDistributionReport> AssetDistributionReports { get; set; }
    public List<AssetStatusReport> AssetStatusReports { get; set; }
    public AssetValuesReport AssetValuesReport { get; set; }
}
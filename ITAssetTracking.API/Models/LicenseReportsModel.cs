using ITAssetTracking.Core.Models;

namespace ITAssetTracking.API.Models;

public class LicenseReportsModel
{
    public List<SoftwareAssetDistributionReport> SoftwareAssetDistributionReports { get; set; }
    public List<AssetStatusReport> SoftwareAssetStatusReports { get; set; }
}
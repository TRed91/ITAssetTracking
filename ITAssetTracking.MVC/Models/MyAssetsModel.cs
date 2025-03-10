using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class MyAssetsModel
{
    public string EmployeeName { get; set; }
    public List<AssetAssignment> Assets { get; set; }
    public List<SoftwareAssetAssignment> SoftwareAssets { get; set; }
}
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class RequestsIndexModel
{
    public List<AssetRequest> Requests { get; set; }
    public int AssetRequestsCount { get; set; }
    public int SoftwareAssetRequestsCount { get; set; }
    public long AssetId { get; set; }
    public byte DepartmentId { get; set; }
    public int? EmployeeId { get; set; }

    public AssetAssignment ToAssignment()
    {
        return new AssetAssignment
        {
            AssetID = AssetId,
            DepartmentID = DepartmentId,
            EmployeeID = EmployeeId
        };
    }
}

public class SwRequestsIndexModel
{
    public List<SoftwareAssetRequest> Requests { get; set; }
    public int AssetRequestsCount { get; set; }
    public int SoftwareAssetRequestsCount { get; set; }
    public long? AssetId { get; set; }
    public int SoftwareAssetId { get; set; }
    public int? EmployeeId { get; set; }

    public SoftwareAssetAssignment ToAssignment()
    {
        return new SoftwareAssetAssignment
        {
            SoftwareAssetID = SoftwareAssetId,
            AssetID = AssetId,
            EmployeeID = EmployeeId,
        };
    }
}
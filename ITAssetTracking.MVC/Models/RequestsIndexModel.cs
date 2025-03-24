using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class RequestsIndexModel
{
    public List<AssetRequest> Requests { get; set; }
    
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
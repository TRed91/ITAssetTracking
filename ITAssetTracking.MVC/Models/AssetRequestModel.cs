using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class AssetRequestModel
{
    public int AssetRequestId { get; set; }
    public string? Note { get; set; }
    public long AssetId { get; set; }
    public byte DepartmentId { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? RequestDate { get; set; }
    
    public string SerialNumber { get; set; }
    public string ModelNumber { get; set; }
    public string AssetTypeName { get; set; }
    public string DepartmentName { get; set; }
    public string? EmployeeName { get; set; }

    public AssetRequestModel() { }

    public AssetRequestModel(AssetRequest entity)
    {
        AssetRequestId = entity.AssetRequestID;
        Note = entity.RequestNote;
        AssetId = entity.AssetID;
        DepartmentId = entity.DepartmentID;
        EmployeeId = entity.EmployeeID;
        RequestDate = entity.RequestDate;
        SerialNumber = entity.Asset.SerialNumber;
        ModelNumber = entity.Asset.Model.ModelNumber;
        AssetTypeName = entity.Asset.AssetType.AssetTypeName;
        DepartmentName = entity.Department.DepartmentName;
        EmployeeName = entity.EmployeeID != null ? (entity.Employee.LastName + ", " + entity.Employee.FirstName) : null;
    }

    public AssetRequest ToEntity()
    {
        return new AssetRequest
        {
            AssetRequestID = AssetRequestId,
            RequestNote = Note,
            AssetID = AssetId,
            DepartmentID = DepartmentId,
            EmployeeID = EmployeeId,
        };
    }
}
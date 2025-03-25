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

    public AssetAssignment? CurrentAssignment { get; set; }

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

public class SwAssetRequestModel
{
    public int SoftwareAssetRequestId { get; set; }
    public string? Note { get; set; }
    public int SoftwareAssetId { get; set; }
    public long? AssetId { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? RequestDate { get; set; }
    
    public string LicenseTypeName { get; set; }
    public string? SerialNumber { get; set; }
    public string? ModelNumber { get; set; }
    public string? AssetTypeName { get; set; }
    public string? EmployeeName { get; set; }

    public SoftwareAssetAssignment? CurrentAssignment { get; set; }

    public SwAssetRequestModel() { }

    public SwAssetRequestModel(SoftwareAssetRequest entity)
    {
        SoftwareAssetRequestId = entity.SoftwareAssetRequestID;
        SoftwareAssetId = entity.SoftwareAssetID;
        Note = entity.RequestNote;
        AssetId = entity.AssetID;
        EmployeeId = entity.EmployeeID;
        RequestDate = entity.RequestDate;
        LicenseTypeName = entity.SoftwareAsset.LicenseType.LicenseTypeName;
        SerialNumber = entity.AssetID != null ? entity.Asset.SerialNumber : null;
        ModelNumber = entity.AssetID != null ? entity.Asset.Model.ModelNumber : null;
        AssetTypeName = entity.AssetID != null ? entity.Asset.AssetType.AssetTypeName : null;
        EmployeeName = entity.EmployeeID != null ? (entity.Employee.LastName + ", " + entity.Employee.FirstName) : null;
    }

    public SoftwareAssetRequest ToEntity()
    {
        return new SoftwareAssetRequest
        {
            SoftwareAssetRequestID = SoftwareAssetRequestId,
            SoftwareAssetID = SoftwareAssetId,
            RequestNote = Note,
            AssetID = AssetId,
            EmployeeID = EmployeeId,
        };
    }
}
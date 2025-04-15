using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class AssetRequestModel
{
    public int AssetRequestId { get; set; }
    public long AssetId { get; set; }
    public string AssetSerialNumber { get; set; }
    public string? Employee { get; set; }
    public string Department { get; set; }
    public string? RequestResult { get; set; }
    public DateTime RequestDate { get; set; }
    public string? Note { get; set; }

    public AssetRequestModel() { }

    public AssetRequestModel(AssetRequest entity)
    {
        AssetRequestId = entity.AssetRequestID;
        AssetId = entity.AssetID;
        AssetSerialNumber = entity.Asset.SerialNumber;
        Employee = entity.EmployeeID != null ? 
            entity.Employee.LastName + ", " + entity.Employee.FirstName : 
            null;
        Department = entity.Department.DepartmentName;
        RequestResult = entity.RequestResultID != null ? 
            entity.RequestResult.RequestResultName : 
            null;
        RequestDate = entity.RequestDate;
        Note = entity.RequestNote;
    }
}

public class SoftwareRequestModel
{
    public int SoftwareAssetRequestId { get; set; }
    public int SoftwareAssetId { get; set; }
    public string LicenseType { get; set; }
    public string? Employee { get; set; }
    public long? AssetId { get; set; }
    public string? AssetSerialNumber { get; set; }
    public string? RequestResult { get; set; }
    public DateTime RequestDate { get; set; }
    public string? Note { get; set; }

    public SoftwareRequestModel() { }

    public SoftwareRequestModel(SoftwareAssetRequest entity)
    {
        SoftwareAssetRequestId = entity.SoftwareAssetRequestID;
        SoftwareAssetId = entity.SoftwareAssetID;
        LicenseType = entity.SoftwareAsset.LicenseType.LicenseTypeName;
        
        Employee = entity.EmployeeID != null ? 
            entity.Employee.LastName + ", " + entity.Employee.FirstName : 
            null;
        
        RequestResult = entity.RequestResultID != null ? 
            entity.RequestResult.RequestResultName : 
            null;
        
        if (entity.AssetID != null)
        {
            AssetId = entity.AssetID;
            AssetSerialNumber = entity.Asset.SerialNumber;
        }
        
        RequestDate = entity.RequestDate;
        Note = entity.RequestNote;
    }
}

public class RequestsModel
{
    public List<AssetRequestModel> AssetRequests { get; set; }
    public List<SoftwareRequestModel> SoftwareRequests { get; set; }
}
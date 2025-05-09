using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class SoftwareAssignmentModel
{
    public int AssetAssignmentId { get; set; }
    public long? AssetId { get; set; }
    public string? AssetSerialNumber { get; set; }
    public string? Employee { get; set; }
    public DateTime AssignmentDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public SoftwareAssignmentModel() { }

    public SoftwareAssignmentModel(SoftwareAssetAssignment entity)
    {
        AssetAssignmentId = entity.AssetAssignmentID;
        AssetId = entity.AssetID;
        AssetSerialNumber = entity.Asset?.SerialNumber ?? null;
        Employee = entity.EmployeeID != null ? 
            entity.Employee.LastName + ", " + entity.Employee.FirstName : 
            null;
        AssignmentDate = entity.AssignmentDate;
        ReturnDate = entity.ReturnDate;
    }
}
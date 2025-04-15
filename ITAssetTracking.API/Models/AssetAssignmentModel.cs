using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class AssetAssignmentModel
{
    public int AssetAssignmentId { get; set; }
    public long AssetId { get; set; }
    public string AssetSerialNumber { get; set; }
    public string Department { get; set; }
    public string Employee { get; set; }
    public DateTime AssignmentDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public AssetAssignmentModel() { }

    public AssetAssignmentModel(AssetAssignment entity)
    {
        AssetAssignmentId = entity.AssetAssignmentID;
        AssetId = entity.AssetID;
        AssetSerialNumber = entity.Asset.SerialNumber;
        Department = entity.Department.DepartmentName;
        Employee = entity.Employee.LastName + ", " + entity.Employee.FirstName;
        AssignmentDate = entity.AssignmentDate;
        ReturnDate = entity.ReturnDate;
    }
}
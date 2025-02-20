namespace ITAssetTracking.Core.Entities;

public class AssetAssignment
{
    public int AssetAssignmentID { get; set; }
    
    public long AssetID { get; set; }
    public byte DepartmentID { get; set; }
    public int EmployeeID { get; set; }
    
    public DateTime AssignmentDate { get; set; }
    public DateTime ReturnDate { get; set; }
    
    Asset Asset { get; set; }
    Department Department { get; set; }
    Employee Employee { get; set; }
}
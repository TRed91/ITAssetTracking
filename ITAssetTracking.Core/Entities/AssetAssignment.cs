using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class AssetAssignment
{
    [Key]
    public int AssetAssignmentID { get; set; }
    
    public long AssetID { get; set; }
    public byte DepartmentID { get; set; }
    public int? EmployeeID { get; set; }
    
    public DateTime AssignmentDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    
    public Asset Asset { get; set; }
    public Department Department { get; set; }
    public Employee Employee { get; set; }
}
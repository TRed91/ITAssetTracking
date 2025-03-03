using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class Department
{
    [Key]
    public byte DepartmentID { get; set; }
    public string DepartmentName { get; set; }
    
    public List<AssetAssignment> AssetAssignments { get; set; } = new List<AssetAssignment>();
    public List<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
    public List<Employee> Employees { get; set; } = new List<Employee>();
}
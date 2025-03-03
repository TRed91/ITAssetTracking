using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class Employee
{
    [Key]
    public int EmployeeID { get; set; }
    public byte DepartmentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public Department Department { get; set; }
    public List<AssetAssignment> AssetAssignments { get; set; } = new List<AssetAssignment>();
    public List<SoftwareAssetAssignment> SoftwareAssetAssignments { get; set; } = new List<SoftwareAssetAssignment>();
    public List<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
    public List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; } = new List<SoftwareAssetRequest>();
    public List<TicketNotes> TicketNotes { get; set; } = new List<TicketNotes>();
}
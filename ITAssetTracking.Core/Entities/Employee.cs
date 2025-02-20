namespace ITAssetTracking.Core.Entities;

public class Employee
{
    public int EmployeeID { get; set; }
    public byte DepartmentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    Department Department { get; set; }
    List<AssetAssignment> AssetAssignments { get; set; } = new List<AssetAssignment>();
    List<SoftwareAssetAssignment> SoftwareAssetAssignments { get; set; } = new List<SoftwareAssetAssignment>();
    List<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
    List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; } = new List<SoftwareAssetRequest>();
    List<TicketNotes> TicketNotes { get; set; } = new List<TicketNotes>();
}
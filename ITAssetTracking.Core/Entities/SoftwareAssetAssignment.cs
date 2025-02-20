namespace ITAssetTracking.Core.Entities;

public class SoftwareAssetAssignment
{
    public int AssetAssignmentID { get; set; }
    
    public int SoftwareAssetID { get; set; }
    public long AssetID { get; set; }
    public int EmployeeID { get; set; }
    
    public DateTime AssignmentDate { get; set; }
    public DateTime ReturnDate { get; set; }
    
    SoftwareAsset SoftwareAsset { get; set; }
    Employee Employee { get; set; }
    Asset Asset { get; set; }
}
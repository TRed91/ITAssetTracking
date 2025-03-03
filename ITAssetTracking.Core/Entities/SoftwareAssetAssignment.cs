using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class SoftwareAssetAssignment
{
    [Key]
    public int AssetAssignmentID { get; set; }
    
    public int SoftwareAssetID { get; set; }
    public long? AssetID { get; set; }
    public int? EmployeeID { get; set; }
    
    public DateTime AssignmentDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    
    public SoftwareAsset SoftwareAsset { get; set; }
    public Employee Employee { get; set; }
    public Asset Asset { get; set; }
}
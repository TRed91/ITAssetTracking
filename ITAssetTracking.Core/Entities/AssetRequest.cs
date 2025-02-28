using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class AssetRequest
{
    [Key]
    public int AssetRequestID { get; set; }
    
    public long AssetID { get; set; }
    public int? EmployeeID { get; set; }
    public byte DepartmentID { get; set; }
    public byte? RequestResultID { get; set; }
    
    public DateTime RequestDate { get; set; }
    public string? RequestNote { get; set; }
    
    Asset Asset { get; set; }
    Employee Employee { get; set; }
    Department Department { get; set; }
    RequestResult RequestResult { get; set; }
}
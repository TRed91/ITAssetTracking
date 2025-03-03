using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class SoftwareAssetRequest
{
    [Key]
    public int SoftwareAssetRequestID { get; set; }
    
    public int SoftwareAssetID { get; set; }
    public int? EmployeeID { get; set; }
    public long? AssetID { get; set; }
    public byte? RequestResultID { get; set; }
    
    public DateTime RequestDate { get; set; }
    public string? RequestNote { get; set; }
    
    public SoftwareAsset SoftwareAsset { get; set; }
    public Employee Employee { get; set; }
    public Asset Asset { get; set; }
    public RequestResult RequestResult { get; set; }
}
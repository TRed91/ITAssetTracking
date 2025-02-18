namespace ITAssetTracking.Core.Entities;

public class SoftwareAssetRequest
{
    public int SoftwareAssetRequestID { get; set; }
    
    public int SoftwareAssetID { get; set; }
    public int EmployeeID { get; set; }
    public long AssetID { get; set; }
    public byte RequestResultID { get; set; }
    
    public DateTime RequestDate { get; set; }
    public string RequestNote { get; set; }
}
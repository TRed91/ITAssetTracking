namespace ITAssetTracking.Core.Entities;

public class RequestResult
{
    public byte RequestResultID { get; set; }
    public string RequestResultName { get; set; }
    
    List<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
    List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; } = new List<SoftwareAssetRequest>();
}
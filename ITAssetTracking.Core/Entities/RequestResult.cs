using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class RequestResult
{
    [Key]
    public byte RequestResultID { get; set; }
    public string RequestResultName { get; set; }
    
    public List<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
    public List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; } = new List<SoftwareAssetRequest>();
}
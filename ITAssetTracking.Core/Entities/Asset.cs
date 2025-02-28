using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class Asset
{
    [Key]
    public long AssetID { get; set; }
    
    public byte AssetTypeID { get; set; }
    public int ManufacturerID { get; set; }
    public int ModelID { get; set; }
    public byte AssetStatusID { get; set; }
    public int LocationID { get; set; }
    
    public string SerialNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    
    AssetType AssetType { get; set; }
    Manufacturer Manufacturer { get; set; }
    Model Model { get; set; }
    AssetStatus AssetStatus { get; set; }
    Location Location { get; set; }
    
    List<AssetAssignment> AssetAssignments { get; set; }
    List<SoftwareAssetAssignment> SoftwareAssetAssignments { get; set; }
    List<AssetRequest> AssetRequests { get; set; }
    List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; }
}
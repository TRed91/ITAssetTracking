using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    public AssetType AssetType { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public Model Model { get; set; }
    public AssetStatus AssetStatus { get; set; }
    public Location Location { get; set; }
    
    public List<AssetAssignment> AssetAssignments { get; set; }
    public List<SoftwareAssetAssignment> SoftwareAssetAssignments { get; set; }
    public List<AssetRequest> AssetRequests { get; set; }
    public List<SoftwareAssetRequest> SoftwareAssetRequests { get; set; }
    public List<Ticket> Tickets { get; set; }
}
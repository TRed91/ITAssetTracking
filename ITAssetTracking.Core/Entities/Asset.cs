namespace ITAssetTracking.Core.Entities;

public class Asset
{
    public long AssetID { get; set; }
    
    public byte AssetTypeID { get; set; }
    public int ManufacturerID { get; set; }
    public int ModelID { get; set; }
    public byte AssetStatusID { get; set; }
    public int LocationID { get; set; }
    
    public string SerialNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
}
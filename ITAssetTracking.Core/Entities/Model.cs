namespace ITAssetTracking.Core.Entities;

public class Model
{
    public int ModelID { get; set; }
    public int ManufacturerID { get; set; }
    public string ModelNumber { get; set; }
    
    Manufacturer Manufacturer { get; set; }
    List<Asset> Assets { get; set; }
}
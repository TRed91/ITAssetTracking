namespace ITAssetTracking.Core.Entities;

public class Location
{
    public int LocationID { get; set; }
    public string LocationName { get; set; }
    
    List<Asset> Assets { get; set; } = new List<Asset>();
}
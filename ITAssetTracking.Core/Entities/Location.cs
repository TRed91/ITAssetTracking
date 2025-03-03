using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class Location
{
    [Key]
    public int LocationID { get; set; }
    public string LocationName { get; set; }
    
    public List<Asset> Assets { get; set; } = new List<Asset>();
}
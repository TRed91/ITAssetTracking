using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetTracking.Core.Entities;

public class Model
{
    [Key]
    public int ModelID { get; set; }
    public int ManufacturerID { get; set; }
    public string ModelNumber { get; set; }
    
    public Manufacturer Manufacturer { get; set; }
    public List<Asset> Assets { get; set; }
}
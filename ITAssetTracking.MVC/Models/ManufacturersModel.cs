using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class ManufacturersModel
{
    public List<Manufacturer>? Manufacturers { get; set; }
    public string? Search { get; set; }
}
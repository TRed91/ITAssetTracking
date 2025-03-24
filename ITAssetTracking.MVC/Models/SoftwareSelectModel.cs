using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class SoftwareSelectModel
{
    public List<Manufacturer> Manufacturers { get; set; } = new();
    public string? Search { get; set; }
}
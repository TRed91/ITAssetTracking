using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class AssetFilterModel
{
    public List<Asset> Assets { get; set; } = new List<Asset>();
    
    public int AssetTypeId { get; set; }
    public int ManufacturerId { get; set; }
    public int LocationId { get; set; }
    public int AssetStatusId { get; set; }
    public bool IncludeRetired { get; set; } = false;
    public AssetsOrder Order { get; set; } = AssetsOrder.SerialNumber;
    public string? Search { get; set; }

    public SelectList OrderOptions { get; set; } = GetOrderOptions();

    public SelectList? AssetTypes { get; set; }
    public SelectList? Manufacturers { get; set; }
    public SelectList? Locations { get; set; }
    public SelectList? AssetStatuses { get; set; }

    private static SelectList GetOrderOptions()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Text = "Serial Number", Value = "1" },
            new SelectListItem { Text = "Model", Value = "2" },
            new SelectListItem { Text = "Type", Value = "3" },
            new SelectListItem { Text = "Manufacturer", Value = "4" },
            new SelectListItem { Text = "Status", Value = "5" },
            new SelectListItem { Text = "Location", Value = "6" },
        };
        return new SelectList(items,  "Value", "Text");
    }
}

public enum AssetsOrder
{
    SerialNumber = 1,
    Model,
    AssetType,
    Manufacturer,
    AssetStatus,
    Location,
}
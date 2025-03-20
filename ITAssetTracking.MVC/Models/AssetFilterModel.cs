using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class AssetFilterModel
{
    public List<Asset> Assets { get; set; } = new List<Asset>();
    
    [Display(Name = "Type")]
    public int AssetTypeId { get; set; }
    [Display(Name = "Manufacturer")]
    public int ManufacturerId { get; set; }
    [Display(Name = "Location")]
    public int LocationId { get; set; }
    [Display(Name = "Status")]
    public int AssetStatusId { get; set; }
    [Display(Name = "Include Retired")]
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

public class SoftwareFilterModel
{
    public List<SoftwareAsset> Assets { get; set; } = new List<SoftwareAsset>();
    
    [Display(Name = "Type")]
    public int LicenseTypeId { get; set; }
    [Display(Name = "Manufacturer")]
    public int ManufacturerId { get; set; }
    [Display(Name = "Status")]
    public int AssetStatusId { get; set; }
    [Display(Name = "Include Expired")]
    public bool IncludeExpired { get; set; } = false;

    public SoftwareAssetsOrder Order { get; set; } = SoftwareAssetsOrder.LicenseType;
    public string? Search { get; set; }

    public SelectList OrderOptions { get; set; } = GetOrderOptions();

    public SelectList? LicenseTypes { get; set; }
    public SelectList? Manufacturers { get; set; }
    public SelectList? AssetStatuses { get; set; }

    private static SelectList GetOrderOptions()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Text = "Type", Value = "1" },
            new SelectListItem { Text = "Manufacturer", Value = "2" },
            new SelectListItem { Text = "Status", Value = "3" },
            new SelectListItem { Text = "Expiration Date", Value = "5"}
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

public enum SoftwareAssetsOrder
{
    LicenseType = 1,
    Manufacturer,
    AssetStatus,
    ExpirationDate,
}
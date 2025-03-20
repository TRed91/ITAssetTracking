using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class DepartmentAssetsModel
{
    public List<AssetAssignment> AssignedAssets { get; set; } = new List<AssetAssignment>();

    public AssetsOrder Order { get; set; } = AssetsOrder.SerialNumber;
    public bool EnableDepSelectList { get; set; } = false;
    public SelectList? DepartmentSelectList { get; set; }
    public SelectList? ManufacturerSelectList { get; set; }
    public SelectList? AssetTypeSelectList { get; set; }
    public SelectList? AssetStatusSelectList { get; set; }
    public SelectList OrderOptions { get; set; } = GetOrderOptions();

    [Display(Name = "Department")]
    public int DepartmentId { get; set; } = 1;
    [Display(Name = "Manufacturer")]
    public int ManufacturerId { get; set; } = 0;
    [Display(Name = "Asset Type")]
    public int AssetTypeId { get; set; } = 0;
    [Display(Name = "Asset Status")]
    public int AssetStatusId { get; set; } = 0;
    
    public string? SearchString { get; set; }
    
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

public class DepartmentSoftwareAssetsModel
{
    public List<SoftwareAssetAssignment> AssignedAssets { get; set; } = new List<SoftwareAssetAssignment>();

    public SoftwareAssetsOrder Order { get; set; } = SoftwareAssetsOrder.LicenseType;
    public bool EnableDepSelectList { get; set; } = false;
    public SelectList? DepartmentSelectList { get; set; }
    public SelectList? ManufacturerSelectList { get; set; }
    public SelectList? LicenseTypeSelectList { get; set; }
    public SelectList? AssetStatusSelectList { get; set; }
    public SelectList OrderOptions { get; set; } = GetOrderOptions();

    [Display(Name = "Department")]
    public int DepartmentId { get; set; } = 1;
    [Display(Name = "Manufacturer")]
    public int ManufacturerId { get; set; } = 0;
    [Display(Name = "License Type")]
    public int LicenseTypeId { get; set; } = 0;
    [Display(Name = "Asset Status")]
    public int AssetStatusId { get; set; } = 0;
    
    public string? SearchString { get; set; }
    
    private static SelectList GetOrderOptions()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Text = "Type", Value = "1" },
            new SelectListItem { Text = "Manufacturer", Value = "2" },
            new SelectListItem { Text = "Status", Value = "3" },
            new SelectListItem { Text = "Expiration Date", Value = "4" },
        };
        return new SelectList(items,  "Value", "Text");
    }
}
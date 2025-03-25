using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class AvailableAssetsModel
{
    public int? SelectedAssetTypeId { get; set; }
    
    public int RequestingDepartmentId { get; set; }
    public SelectList? AssetTypesSelectList { get; set; }
    public List<AssetType> AvailableAssets { get; set; } = new List<AssetType>();
}

public class AvailableLicensesModel
{
    public int? SelectedLicenseTypeId { get; set; }
    
    public int RequestingDepartmentId { get; set; }
    public SelectList? LicenseTypesSelectList { get; set; }
    public List<LicenseType> AvailableLicenses { get; set; } = new List<LicenseType>();
}
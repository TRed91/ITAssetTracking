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
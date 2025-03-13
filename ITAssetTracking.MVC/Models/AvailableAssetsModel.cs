using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class AvailableAssetsModel
{
    public int? AssetTypeId { get; set; }
    public SelectList? AssetTypesSelectList { get; set; }
    public List<AssetType> AssetTypes { get; set; } = new List<AssetType>();
}
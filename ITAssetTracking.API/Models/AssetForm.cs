using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class AssetForm
{
    [Required]
    [Range(1, byte.MaxValue)]
    public byte AssetTypeId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ManufacturerId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ModelId { get; set; }
    
    public byte? AssetStatusId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int LocationId { get; set; }
    [Required]
    public string SerialNumber { get; set; }
    [Required]
    public DateTime PurchaseDate { get; set; }
    [Required]
    public decimal PurchasePrice { get; set; }

    public AssetForm() { }

    public AssetForm(Asset entity)
    {
        AssetTypeId = entity.AssetTypeID;
        ManufacturerId = entity.ManufacturerID;
        ModelId = entity.ModelID;
        LocationId = entity.LocationID;
        SerialNumber = entity.SerialNumber;
        PurchaseDate = entity.PurchaseDate;
        PurchasePrice = entity.PurchasePrice;
        AssetStatusId = entity.AssetStatusID;
    }

    public Asset ToEntity()
    {
        var asset = new Asset
        {
            AssetTypeID = AssetTypeId,
            ManufacturerID = ManufacturerId,
            ModelID = ModelId,
            LocationID = LocationId,
            SerialNumber = SerialNumber,
            PurchaseDate = PurchaseDate,
            PurchasePrice = PurchasePrice,
        };
        if (AssetStatusId.HasValue)
        {
            asset.AssetStatusID = AssetStatusId.Value;
        }
        return asset;
    }
}
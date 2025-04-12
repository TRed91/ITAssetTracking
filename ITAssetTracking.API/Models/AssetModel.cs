using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class AssetModel
{
    public long AssetId { get; set; }
    public string AssetType { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public string Status { get; set; }
    public string Location { get; set; }
    public string SerialNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }

    public AssetModel(Asset entity)
    {
        AssetId = entity.AssetID;
        AssetType = entity.AssetType.AssetTypeName;
        Manufacturer = entity.Manufacturer.ManufacturerName;
        Model = entity.Model.ModelNumber;
        Status = entity.AssetStatus.AssetStatusName;
        Location = entity.Location.LocationName;
        SerialNumber = entity.SerialNumber;
        PurchaseDate = entity.PurchaseDate;
        PurchasePrice = entity.PurchasePrice;
    }
}
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class AssetDetailsModel
{
    public long AssetId { get; set; }
    public string SerialNumber { get; set; }
    public string AssetType { get; set; }
    public string ModelNumber { get; set; }
    public string Manufacturer { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    
    public List<AssetAssignment> AssetAssignments { get; set; }
    public List<Ticket> SupportTickets { get; set; }

    public AssetDetailsModel(){}

    public AssetDetailsModel(Asset asset)
    {
        AssetId = asset.AssetID;
        SerialNumber = asset.SerialNumber;
        AssetType = asset.AssetType.AssetTypeName;
        ModelNumber = asset.Model.ModelNumber;
        Manufacturer = asset.Manufacturer.ManufacturerName;
        Location = asset.Location.LocationName;
        Status = asset.AssetStatus.AssetStatusName;
        PurchaseDate = asset.PurchaseDate;
        PurchasePrice = asset.PurchasePrice;
        AssetAssignments = asset.AssetAssignments;
        SupportTickets = asset.Tickets;
    }
}

public class SoftwareAssetDetailsModel
{
    public int SoftwareAssetId { get; set; }
    public string LicenseType { get; set; }
    public string Manufacturer { get; set; }
    public string Status { get; set; }
    public string LicenseKey { get; set; }
    public int NumberOfLicenses { get; set; }
    public string Version { get; set; }
    public DateTime ExpirationDate { get; set; }
    
    public List<SoftwareAssetAssignment> Assignments { get; set; }

    public SoftwareAssetDetailsModel(){}

    public SoftwareAssetDetailsModel(SoftwareAsset asset)
    {
        SoftwareAssetId = asset.SoftwareAssetID;
        LicenseType = asset.LicenseType.LicenseTypeName;
        Manufacturer = asset.Manufacturer.ManufacturerName;
        Status = asset.AssetStatus.AssetStatusName;
        LicenseKey = asset.LicenseKey;
        NumberOfLicenses = asset.NumberOfLicenses;
        Version = asset.Version;
        ExpirationDate = asset.ExpirationDate;
        Assignments = asset.SoftwareAssetAssignments;
    }
}
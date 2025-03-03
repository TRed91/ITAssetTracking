using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITAssetTracking.Data.Repositories;

public class AssetRepository : IAssetRepository
{
    private ITAssetTrackingContext _context;

    public AssetRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public Asset? GetAssetById(long assetId)
    {
        return _context.Asset.FirstOrDefault(a => a.AssetID == assetId);
    }

    public List<Asset> GetAssets(int assetTypeId = 0, int locationId = 0, int assetStatusId = 0, int manufacturerId = 0)
    {
        return _context.Asset
            .Include(a => a.Model)
            .Where(a => 
                (assetTypeId != 0 ? a.AssetTypeID == assetTypeId : a.AssetTypeID > 0) &&
                (locationId != 0 ? a.LocationID == locationId : a.LocationID > 0) &&
                (assetStatusId != 0 ? a.AssetStatusID == assetStatusId : a.AssetStatusID > 0) &&
                (manufacturerId != 0 ? a.ManufacturerID == manufacturerId : a.ManufacturerID > 0))
            .ToList();
    }

    public Asset? GetAssetBySerialNumber(string serialNumber)
    {
        return _context.Asset.FirstOrDefault(a => a.SerialNumber == serialNumber);
    }

    public List<Asset> GetAssetsByType(int assetTypeId)
    {
        return _context.Asset
            .Where(a => a.AssetTypeID == assetTypeId)
            .ToList();
    }

    public List<Asset> GetAssetsByLocation(int locationId)
    {
        return _context.Asset
            .Where(a => a.LocationID == locationId)
            .ToList();
    }

    public List<Asset> GetAssetsByModel(int modelId)
    {
        return _context.Asset
            .Where(a => a.ModelID == modelId)
            .ToList();
    }

    public List<Asset> GetAssetsByManufacturer(int manufacturerId)
    {
        return _context.Asset
            .Where(a => a.ManufacturerID == manufacturerId)
            .ToList();
    }

    public List<Asset> GetAssetsByStatus(int assetStatusId)
    {
        return _context.Asset
            .Where(a => a.AssetStatusID == assetStatusId)
            .ToList();
    }

    public List<Asset> GetAssetsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.Asset
            .Where(a => a.PurchaseDate >= startDate && a.PurchaseDate <= endDate)
            .ToList();
    }

    public void AddAsset(Asset asset)
    {
        _context.Asset.Add(asset);
        _context.SaveChanges();
    }

    public void UpdateAsset(Asset asset)
    {
        _context.Asset.Update(asset);
        _context.SaveChanges();
    }

    public void DeleteAsset(Asset asset)
    {
        _context.Asset.Remove(asset);
        _context.SaveChanges();
    }

    public List<Manufacturer> GetManufacturers()
    {
        return _context.Manufacturer.ToList();
    }

    public List<Model> GetModels()
    {
        return _context.Model.ToList();
    }

    public List<Model> GetModelsByManufacturer(int manufacturerId)
    {
        return _context.Model
            .Where(m => m.ManufacturerID == manufacturerId)
            .ToList();
    }

    public List<AssetType> GetAssetTypes()
    {
        return _context.AssetType.ToList();
    }

    public List<Location> GetLocations()
    {
        return _context.Location.ToList();
    }

    public List<AssetStatus> GetAssetStatuses()
    {
        return _context.AssetStatus.ToList();
    }
}
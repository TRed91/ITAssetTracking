using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITAssetTracking.Data.Repositories;

public class SoftwareAssetRepository : ISoftwareAssetRepository
{
    private ITAssetTrackingContext _context;

    public SoftwareAssetRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public SoftwareAsset? GetSoftwareAsset(int softwareAssetId)
    {
        return _context.SoftwareAsset
            .Include(s => s.Manufacturer)
            .Include(s => s.AssetStatus)
            .Include(s => s.LicenseType)
            .Include(s => s.SoftwareAssetAssignments)
            .ThenInclude(sa => sa.Asset)
            .Include(s => s.SoftwareAssetAssignments)
            .ThenInclude(sa => sa.Employee)
            .FirstOrDefault(s => s.SoftwareAssetID == softwareAssetId);
    }

    public List<SoftwareAsset> GetSoftwareAssets(int licenseTypeId, int manufacturerId, int assetStatusId, bool includeExpired)
    {
        return _context.SoftwareAsset
            .Include(s => s.Manufacturer)
            .Include(s => s.AssetStatus)
            .Include(s => s.LicenseType)
            .Where(s => (includeExpired || s.ExpirationDate >= DateTime.Now) &&
                        (licenseTypeId == 0 || s.LicenseTypeID == licenseTypeId) &&
                        (assetStatusId == 0 || s.AssetStatusID == assetStatusId) &&
                        (manufacturerId == 0 || s.ManufacturerID == manufacturerId))
            .ToList();
    }

    public List<SoftwareAsset> GetSoftwareAssetsByManufacturer(int manufacturerId, bool includeExpired)
    {
        if (includeExpired)
        {
            return _context.SoftwareAsset
                .Where(s => s.ManufacturerID == manufacturerId)
                .ToList();
        }
        return _context.SoftwareAsset
            .Where(s => s.ManufacturerID == manufacturerId && s.ExpirationDate >= DateTime.Now)
            .ToList();
    }

    public List<SoftwareAsset> GetSoftwareAssetsByType(int licenseTypeId, bool includeExpired)
    {
        if (includeExpired)
        {
            return _context.SoftwareAsset
                .Where(s => s.LicenseTypeID == licenseTypeId)
                .ToList();
        }
        return _context
            .SoftwareAsset
            .Where(s => s.LicenseTypeID == licenseTypeId && s.ExpirationDate >= DateTime.Now)
            .ToList();
    }

    public List<SoftwareAsset> GetSoftwareAssetsByStatus(int assetStatusId)
    {
        return _context.SoftwareAsset
            .Where(s => s.AssetStatusID == assetStatusId)
            .ToList();
    }

    public void AddSoftwareAsset(SoftwareAsset softwareAsset)
    {
        _context.SoftwareAsset.Add(softwareAsset);
        _context.SaveChanges();
    }

    public void UpdateSoftwareAsset(SoftwareAsset softwareAsset)
    {
        var assetToUpdate = _context.SoftwareAsset
            .FirstOrDefault(s => s.SoftwareAssetID == softwareAsset.SoftwareAssetID);
        if (assetToUpdate != null)
        {
            assetToUpdate.AssetStatusID = softwareAsset.AssetStatusID;
            assetToUpdate.LicenseTypeID = softwareAsset.LicenseTypeID;
            assetToUpdate.ExpirationDate = softwareAsset.ExpirationDate;
            assetToUpdate.ManufacturerID = softwareAsset.ManufacturerID;
            assetToUpdate.Version = softwareAsset.Version;
            assetToUpdate.LicenseKey = softwareAsset.LicenseKey;
            assetToUpdate.NumberOfLicenses = softwareAsset.NumberOfLicenses;
            _context.SaveChanges();
        }
    }

    public void DeleteSoftwareAsset(int softwareAssetId)
    {
        var asset = _context.SoftwareAsset.FirstOrDefault(s => s.SoftwareAssetID == softwareAssetId);
        if (asset != null)
        {
            _context.SoftwareAsset.Remove(asset);
            _context.SaveChanges();
        }
    }

    public List<LicenseType> GetLicenseTypes()
    {
        return _context.LicenseType.ToList();
    }

    public List<LicenseType> GetLicenseTypesByManufacturer(int manufacturerId)
    {
        return _context.LicenseType.Where(l => l.ManufacturerID == manufacturerId).ToList();
    }
}
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

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
        return _context.SoftwareAsset.FirstOrDefault(s => s.SoftwareAssetID == softwareAssetId);
    }

    public List<SoftwareAsset> GetSoftwareAssets(bool includeExpired)
    {
        if (includeExpired)
        {
            return _context.SoftwareAsset.ToList();
        }
        return _context.SoftwareAsset
            .Where(s => s.ExpirationDate >= DateTime.Now)
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
        _context.SoftwareAsset.Update(softwareAsset);
        _context.SaveChanges();
    }

    public void DeleteSoftwareAsset(SoftwareAsset softwareAsset)
    {
        _context.SoftwareAsset.Remove(softwareAsset);
        _context.SaveChanges();
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
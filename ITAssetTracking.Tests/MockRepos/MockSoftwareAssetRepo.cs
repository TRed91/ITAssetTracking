using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockSoftwareAssetRepo : ISoftwareAssetRepository
{
    private readonly MockDB _db;

    public MockSoftwareAssetRepo()
    {
        _db = new MockDB();
    }
    public SoftwareAsset? GetSoftwareAsset(int softwareAssetId)
    {
        return _db.SoftwareAssets.FirstOrDefault(a => a.SoftwareAssetID == softwareAssetId);
    }

    public List<SoftwareAsset> GetSoftwareAssets(int licenseTypeId, int manufacturerId, int assetStatusId, bool includeExpired)
    {
        return includeExpired ? _db.SoftwareAssets : _db.SoftwareAssets
            .Where(a => a.ExpirationDate < DateTime.Now)
            .ToList();
    }

    public List<SoftwareAsset> GetSoftwareAssetsByManufacturer(int manufacturerId, bool includeExpired)
    {
        if (includeExpired)
        {
            return _db.SoftwareAssets
                .Where(a => a.ManufacturerID == manufacturerId)
                .ToList();
        }
        return _db.SoftwareAssets
            .Where(a => a.ManufacturerID == manufacturerId && a.ExpirationDate < DateTime.Now)
            .ToList();
    }

    public List<SoftwareAsset> GetSoftwareAssetsByType(int licenseTypeId, bool includeExpired)
    {
        if (includeExpired)
        {
            return _db.SoftwareAssets
                .Where(a => a.LicenseTypeID == licenseTypeId)
                .ToList();
        }
        return _db.SoftwareAssets
            .Where(a => a.LicenseTypeID == licenseTypeId && a.ExpirationDate < DateTime.Now)
            .ToList();
    }

    public List<SoftwareAsset> GetSoftwareAssetsByStatus(int assetStatusId)
    {
        return _db.SoftwareAssets
            .Where(a => a.AssetStatusID == assetStatusId)
            .ToList();
    }

    public void AddSoftwareAsset(SoftwareAsset softwareAsset)
    {
        softwareAsset.SoftwareAssetID = _db.SoftwareAssets.Max(a => a.SoftwareAssetID) + 1;
        _db.SoftwareAssets.Add(softwareAsset);
    }

    public void UpdateSoftwareAsset(SoftwareAsset softwareAsset)
    {
        var asset = _db.SoftwareAssets
            .FirstOrDefault(a => a.SoftwareAssetID == softwareAsset.SoftwareAssetID);
        asset.AssetStatusID = softwareAsset.AssetStatusID;
        asset.ManufacturerID = softwareAsset.ManufacturerID;
        asset.LicenseTypeID = softwareAsset.LicenseTypeID;
        asset.LicenseKey = softwareAsset.LicenseKey;
        asset.ExpirationDate = softwareAsset.ExpirationDate;
        asset.Version = softwareAsset.Version;
        asset.NumberOfLicenses = softwareAsset.NumberOfLicenses;
    }

    public void DeleteSoftwareAsset(int softwareAssetId)
    {
        var softwareAsset = _db.SoftwareAssets.FirstOrDefault(a => a.SoftwareAssetID == softwareAssetId);
        _db.SoftwareAssets.Remove(softwareAsset);
    }

    public List<LicenseType> GetLicenseTypes()
    {
        return _db.LicenseTypes;
    }

    public List<LicenseType> GetLicenseTypesByManufacturer(int manufacturerId)
    {
        return _db.LicenseTypes
            .Where(a => a.ManufacturerID == manufacturerId)
            .ToList();
    }

    public LicenseType? GetLicenseTypeById(int licenseTypeId)
    {
        throw new NotImplementedException();
    }

    public List<Manufacturer> GetSoftwareManufacturers()
    {
        throw new NotImplementedException();
    }
}
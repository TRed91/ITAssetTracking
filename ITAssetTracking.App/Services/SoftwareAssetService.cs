using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class SoftwareAssetService : ISoftwareAssetService
{
    private readonly ISoftwareAssetRepository _softwareAssetRepo;

    public SoftwareAssetService(ISoftwareAssetRepository softwareAssetRepository)
    {
        _softwareAssetRepo = softwareAssetRepository;        
    }
    
    public Result<SoftwareAsset> GetSoftwareAsset(int softwareAssetId)
    {
        try
        {
            var asset = _softwareAssetRepo.GetSoftwareAsset(softwareAssetId);
            if (asset == null)
            {
                return ResultFactory.Fail<SoftwareAsset>("Software asset not found");
            }

            return ResultFactory.Success(asset);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<SoftwareAsset>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAsset>> GetSoftwareAssets(bool includeExpired = true)
    {
        try
        {
            var assets = _softwareAssetRepo.GetSoftwareAssets(includeExpired);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAsset>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAsset>> GetSoftwareAssetsByManufacturer(int manufacturerId, bool includeExpired = true)
    {
        try
        {
            var assets = _softwareAssetRepo.GetSoftwareAssetsByManufacturer(manufacturerId, includeExpired);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAsset>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAsset>> GetSoftwareAssetsByType(int licenseTypeId, bool includeExpired = true)
    {
        try
        {
            var assets = _softwareAssetRepo.GetSoftwareAssetsByType(licenseTypeId, includeExpired);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAsset>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAsset>> GetSoftwareAssetsByStatus(int statusId)
    {
        try
        {
            var assets = _softwareAssetRepo.GetSoftwareAssetsByStatus(statusId);
            return ResultFactory.Success(assets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAsset>>(ex.Message, ex);
        }
    }

    public Result AddSoftwareAsset(SoftwareAsset softwareAsset)
    {
        if (softwareAsset.ExpirationDate < DateTime.Today)
        {
            return ResultFactory.Fail("Software asset expired");
        }

        if (softwareAsset.NumberOfLicenses < 1)
        {
            return ResultFactory.Fail("Software asset has no licenses");
        }
        try
        {
            _softwareAssetRepo.AddSoftwareAsset(softwareAsset);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateSoftwareAsset(SoftwareAsset softwareAsset)
    {
        if (softwareAsset.ExpirationDate < DateTime.Today)
        {
            return ResultFactory.Fail("Software asset expired");
        }

        if (softwareAsset.NumberOfLicenses < 1)
        {
            return ResultFactory.Fail("Software asset has no licenses");
        }
        try
        {
            var asset = _softwareAssetRepo.GetSoftwareAsset(softwareAsset.SoftwareAssetID);
            if (asset == null)
            {
                return ResultFactory.Fail("Software asset not found");
            }

            asset.AssetStatusID = softwareAsset.AssetStatusID;
            asset.ManufacturerID = softwareAsset.ManufacturerID;
            asset.LicenseTypeID = softwareAsset.LicenseTypeID;
            asset.Version = softwareAsset.Version;
            asset.ExpirationDate = softwareAsset.ExpirationDate;
            asset.LicenseKey = softwareAsset.LicenseKey;
            asset.NumberOfLicenses = softwareAsset.NumberOfLicenses;
            _softwareAssetRepo.UpdateSoftwareAsset(asset);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteSoftwareAsset(int softwareAssetId)
    {
        try
        {
            var asset = _softwareAssetRepo.GetSoftwareAsset(softwareAssetId);
            if (asset == null)
            {
                return ResultFactory.Fail("Software asset not found");
            }

            _softwareAssetRepo.DeleteSoftwareAsset(softwareAssetId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result<List<LicenseType>> GetLicenseTypes()
    {
        try
        {
            var licenseTypes = _softwareAssetRepo.GetLicenseTypes();
            return ResultFactory.Success(licenseTypes);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<LicenseType>>(ex.Message, ex);
        }
    }

    public Result<List<LicenseType>> GetLicenseTypesByManufacturer(int manufacturerId)
    {
        try
        {
            var licenseTypes = _softwareAssetRepo.GetLicenseTypesByManufacturer(manufacturerId);
            return ResultFactory.Success(licenseTypes);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<LicenseType>>(ex.Message, ex);
        }
    }
}
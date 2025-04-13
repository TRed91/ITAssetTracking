using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class SoftwareAssetService : ISoftwareAssetService
{
    private readonly ISoftwareAssetRepository _softwareAssetRepo;
    private readonly IAssetRepository _assetRepo;
    private readonly IEmployeeRepository _employeeRepo;

    public SoftwareAssetService(
        ISoftwareAssetRepository softwareAssetRepository,  
        IAssetRepository assetRepository, 
        IEmployeeRepository employeeRepository)
    {
        _softwareAssetRepo = softwareAssetRepository;        
        _assetRepo = assetRepository;
        _employeeRepo = employeeRepository;
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

    public Result<List<SoftwareAsset>> GetSoftwareAssets(int licenseTypeId = 0, int assetStatusId = 0, int manufacturerId = 0, bool includeExpired = false)
    {
        try
        {
            var assets = _softwareAssetRepo.GetSoftwareAssets(licenseTypeId, manufacturerId, assetStatusId, includeExpired);
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

    public Result<List<SoftwareAsset>> GetSoftwareAssetsByEmployeeId(int employeeId)
    {
        try
        {
            var employee = _employeeRepo.GetEmployee(employeeId);
            if (employee == null)
            {
                return ResultFactory.Fail<List<SoftwareAsset>>("Employee not found");
            }
            var assets = _softwareAssetRepo.GetSoftwareAssetsByEmployeeId(employeeId);
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
            // default asset status to 'Storage'
            var status = _assetRepo.GetAssetStatusByName("Storage");
            softwareAsset.AssetStatusID = status.AssetStatusID;
            
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

            if (softwareAsset.AssetStatusID == 0)
            {
                softwareAsset.AssetStatusID = asset.AssetStatusID;
            }
            
            _softwareAssetRepo.UpdateSoftwareAsset(softwareAsset);
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

    public Result<LicenseType> GetLicenseTypeById(int licenseTypeId)
    {
        try
        {
            var licenseType = _softwareAssetRepo.GetLicenseTypeById(licenseTypeId);
            if (licenseType == null)
            {
                return ResultFactory.Fail<LicenseType>("License type not found");
            }

            return ResultFactory.Success(licenseType);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<LicenseType>(ex.Message, ex);
        }
    }

    public Result<List<Manufacturer>> GetLicenseTypesByManufacturers()
    {
        try
        {
            var manufacturers = _softwareAssetRepo.GetSoftwareManufacturers();
            return ResultFactory.Success(manufacturers);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Manufacturer>>(ex.Message, ex);
        }
    }
}
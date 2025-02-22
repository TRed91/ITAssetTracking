using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockAssetRepo : IAssetRepository
{
    public Asset? GetAssetById(long assetId)
    {
        throw new NotImplementedException();
    }

    public Asset? GetAssetBySerialNumber(string serialNumber)
    {
        throw new NotImplementedException();
    }

    public List<Asset> GetAssets(int assetTypeId = 0, int locationId = 0, int modelId = 0, int manufacturerId = 0)
    {
        throw new NotImplementedException();
    }

    public List<Asset> GetAssetsByType(int assetTypeId)
    {
        throw new NotImplementedException();
    }

    public List<Asset> GetAssetsByLocation(int locationId)
    {
        throw new NotImplementedException();
    }

    public List<Asset> GetAssetsByModel(int modelId)
    {
        throw new NotImplementedException();
    }

    public List<Asset> GetAssetsByManufacturer(int manufacturerId)
    {
        throw new NotImplementedException();
    }

    public List<Asset> GetAssetsByStatus(int assetStatusId)
    {
        throw new NotImplementedException();
    }

    public List<Asset> GetAssetsInDateRange(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public void AddAsset(Asset asset)
    {
        throw new NotImplementedException();
    }

    public void UpdateAsset(Asset asset)
    {
        throw new NotImplementedException();
    }

    public void DeleteAsset(Asset asset)
    {
        throw new NotImplementedException();
    }

    public List<Manufacturer> GetManufacturers()
    {
        throw new NotImplementedException();
    }

    public List<Model> GetModels()
    {
        throw new NotImplementedException();
    }

    public List<Model> GetModelsByManufacturer(int manufacturerId)
    {
        throw new NotImplementedException();
    }

    public List<AssetType> GetAssetTypes()
    {
        throw new NotImplementedException();
    }

    public List<Location> GetLocations()
    {
        throw new NotImplementedException();
    }

    public List<AssetStatus> GetAssetStatuses()
    {
        throw new NotImplementedException();
    }
}
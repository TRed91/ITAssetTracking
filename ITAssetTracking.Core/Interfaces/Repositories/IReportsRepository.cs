using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IReportsRepository
{
    List<AssetAssignment> GetAssetReports(
        DateTime startDate, 
        DateTime endDate,  
        byte? departmentId, 
        byte? assetTypeId);

    List<Asset> GetAssetsInTimeFrame(DateTime startDate, DateTime endDate);
}
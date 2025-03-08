using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IAssetRequestRepository
{
    AssetRequest? GetAssetRequestById(int assetRequestId);
    
    List<AssetRequest> GetAssetRequests();
    List<AssetRequest> GetOpenAssetRequests();
    List<AssetRequest> GetAssetRequestsByAssetId(long assetId, bool includeClosed);
    List<AssetRequest> GetAssetRequestsByEmployeeId(int employeeId, bool includeClosed);
    List<AssetRequest> GetAssetRequestsByDepartmentId(int departmentId, bool includeClosed);
    List<AssetRequest> GetAssetRequestsByResultId(int requestResultId);
    List<AssetRequest> GetAssetRequestInDateRange(DateTime startDate, DateTime endDate);
    
    void AddAssetRequest(AssetRequest assetRequest);
    void UpdateAssetRequest(AssetRequest assetRequest);
    void DeleteAssetRequest(int assetRequestId);
}
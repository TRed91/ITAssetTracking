using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IAssetRequestRepository
{
    AssetRequest? GetAssetRequestById(string assetRequestId);
    
    List<AssetRequest> GetAssetRequests();
    List<AssetRequest> GetOpenAssetRequests();
    List<AssetRequest> GetAssetRequestsByAssetId(int assetId);
    List<AssetRequest> GetAssetRequestsByEmployeeId(int employeeId);
    List<AssetRequest> GetAssetRequestsByDepartmentId(int departmentId);
    List<AssetRequest> GetAssetRequestsByResultId(int requestResultId);
    List<AssetRequest> GetAssetRequestInDateRange(DateTime startDate, DateTime endDate);
    
    void AddAssetRequest(AssetRequest assetRequest);
    void UpdateAssetRequest(AssetRequest assetRequest);
    void DeleteAssetRequest(AssetRequest assetRequest);
}
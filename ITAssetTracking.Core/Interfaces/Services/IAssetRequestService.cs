using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Enums;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface IAssetRequestService
{
    Result<AssetRequest> GetAssetRequestById(int assetRequestId);
    
    Result<List<AssetRequest>> GetAllAssetRequests();
    Result<List<AssetRequest>> GetOpenAssetRequests();
    Result<List<AssetRequest>> GetAssetRequestsByAsset(long assetId, bool includeClosed = false);
    Result<List<AssetRequest>> GetAssetRequestsByEmployee(int employeeId, bool includeClosed = false);
    Result<List<AssetRequest>> GetAssetRequestsByDepartment(int departmentId, bool includeClosed = false);
    Result<List<AssetRequest>> GetAssetRequestsByResult(int resultId);
    Result<List<AssetRequest>> GetAssetRequestsInDateRange(DateTime startDate, DateTime endDate);
    
    Result AddAssetRequest(AssetRequest assetRequest);
    Result UpdateAssetRequest(AssetRequest assetRequest);
    Result DeleteAssetRequest(int assetRequestId);
    Result<List<AssetType>> GetAvailableAssets();

    Result ResolveRequest(int assetRequestId, RequestResultEnum requestResult, string? note);
}
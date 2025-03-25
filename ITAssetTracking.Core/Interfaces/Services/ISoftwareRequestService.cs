using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Enums;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface ISoftwareRequestService
{
    Result<SoftwareAssetRequest> GetSoftwareRequestById(int softwareRequestId);
    
    Result<List<SoftwareAssetRequest>> GetSoftwareRequests();
    Result<List<SoftwareAssetRequest>> GetOpenSoftwareRequests();
    Result<List<SoftwareAssetRequest>> GetRequestsByEmployee(int employeeId, bool includeClosed = false);
    Result<List<SoftwareAssetRequest>> GetRequestsBySoftware(int softwareAssetId, bool includeClosed = false);
    Result<List<SoftwareAssetRequest>> GetRequestsByAsset(long assetId, bool includeClosed = false);
    Result<List<SoftwareAssetRequest>> GetRequestsByResult(int resultId);
    Result<List<SoftwareAssetRequest>> GetRequestsInDateRange(DateTime startDate, DateTime endDate);
    
    Result AddSoftwareRequest(SoftwareAssetRequest softwareAssetRequest);
    Result UpdateSoftwareRequest(SoftwareAssetRequest softwareAssetRequest);
    Result DeleteSoftwareRequest(int softwareRequestId);
    
    Result<List<LicenseType>> GetAvailableAssets();

    Result ResolveRequest(int softwareAssetRequestId, RequestResultEnum requestResult, string? note);
}
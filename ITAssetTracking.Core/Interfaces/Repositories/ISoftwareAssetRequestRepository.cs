using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ISoftwareAssetRequestRepository
{
    SoftwareAssetRequest? GetSoftwareAssetRequestById(int softwareAssetRequestId);
    
    List<SoftwareAssetRequest> GetSoftwareAssetRequests();
    List<SoftwareAssetRequest> GetOpenSoftwareAssetRequests();
    List<SoftwareAssetRequest> GetRequestsByEmployeeId(int employeeId, bool includeClosed);
    List<SoftwareAssetRequest> GetRequestsBySoftwareId(int softwareAssetId, bool includeClosed);
    List<SoftwareAssetRequest> GetRequestsByAssetId(long assetId, bool includeClosed);
    List<SoftwareAssetRequest> GetRequestsByResultId(int requestResultId);
    List<SoftwareAssetRequest> GetRequestsInDateRange(DateTime startDate, DateTime endDate);
    
    void AddRequest(SoftwareAssetRequest softwareAssetRequest);
    void UpdateRequest(SoftwareAssetRequest softwareAssetRequest);
    void DeleteRequest(int softwareAssetRequestId);
}
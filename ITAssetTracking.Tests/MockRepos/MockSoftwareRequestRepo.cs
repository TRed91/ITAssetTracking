using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockSoftwareRequestRepo : ISoftwareAssetRequestRepository
{
    private readonly MockDB _db;

    public MockSoftwareRequestRepo()
    {
        _db = new MockDB();
    }
    
    public SoftwareAssetRequest? GetSoftwareAssetRequestById(int softwareAssetRequestId)
    {
        return _db.SoftwareAssetRequests
            .FirstOrDefault(r => r.SoftwareAssetRequestID == softwareAssetRequestId);
    }

    public List<SoftwareAssetRequest> GetSoftwareAssetRequests()
    {
        return _db.SoftwareAssetRequests;
    }

    public List<SoftwareAssetRequest> GetOpenSoftwareAssetRequests()
    {
        return _db.SoftwareAssetRequests
            .Where(r => r.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsByEmployeeId(int employeeId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.SoftwareAssetRequests
                .Where(r => r.EmployeeID == employeeId)
                .ToList();
        }
        return _db.SoftwareAssetRequests
            .Where(r => r.EmployeeID == employeeId && r.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsBySoftwareId(int softwareAssetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.SoftwareAssetRequests
                .Where(r => r.SoftwareAssetID == softwareAssetId)
                .ToList();
        }
        return _db.SoftwareAssetRequests
            .Where(r => r.SoftwareAssetID == softwareAssetId && r.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsByAssetId(long assetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.SoftwareAssetRequests
                .Where(r => r.AssetID == assetId)
                .ToList();
        }
        return _db.SoftwareAssetRequests
            .Where(r => r.AssetID == assetId && r.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsByResultId(int requestResultId)
    {
        return _db.SoftwareAssetRequests
            .Where(r => r.RequestResultID == requestResultId)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _db.SoftwareAssetRequests
            .Where(r => r.RequestDate >= startDate && r.RequestDate <= endDate)
            .ToList();
    }

    public void AddRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        softwareAssetRequest.SoftwareAssetRequestID = _db.SoftwareAssetRequests.Max(a => a.SoftwareAssetRequestID) + 1;
        _db.SoftwareAssetRequests.Add(softwareAssetRequest);
    }

    public void UpdateRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        var request = _db.SoftwareAssetRequests
            .FirstOrDefault(r => r.SoftwareAssetRequestID == softwareAssetRequest.SoftwareAssetRequestID);
        request.RequestNote = softwareAssetRequest.RequestNote;
        request.EmployeeID = softwareAssetRequest.EmployeeID;
        request.SoftwareAssetID = softwareAssetRequest.SoftwareAssetID;
        request.AssetID = softwareAssetRequest.AssetID;
        request.RequestResultID = softwareAssetRequest.RequestResultID;
    }

    public void DeleteRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        _db.SoftwareAssetRequests.Remove(softwareAssetRequest);
    }
}
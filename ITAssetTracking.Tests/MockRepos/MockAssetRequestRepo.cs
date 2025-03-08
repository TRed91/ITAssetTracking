using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockAssetRequestRepo : IAssetRequestRepository
{
    private readonly MockDB _db;

    public MockAssetRequestRepo()
    {
        _db = new MockDB();
    }
    
    public AssetRequest? GetAssetRequestById(int assetRequestId)
    {
        return _db.AssetRequests.FirstOrDefault(a => a.AssetRequestID == assetRequestId);
    }

    public List<AssetRequest> GetAssetRequests()
    {
        return _db.AssetRequests;
    }

    public List<AssetRequest> GetOpenAssetRequests()
    {
        return _db.AssetRequests.Where(a => a.RequestResultID == null).ToList();
    }

    public List<AssetRequest> GetAssetRequestsByAssetId(long assetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.AssetRequests
                .Where(a => a.AssetID == assetId)
                .ToList();
        }
        return _db.AssetRequests
            .Where(a => a.AssetID == assetId && a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByEmployeeId(int employeeId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.AssetRequests
                .Where(a => a.EmployeeID == employeeId)
                .ToList();
        }
        return _db.AssetRequests
            .Where(a => a.EmployeeID == employeeId && a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByDepartmentId(int departmentId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.AssetRequests
                .Where(a => a.DepartmentID == departmentId)
                .ToList();
        }
        return _db.AssetRequests
            .Where(a => a.DepartmentID == departmentId && a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByResultId(int requestResultId)
    {
        return _db.AssetRequests
            .Where(a => a.RequestResultID == requestResultId)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestInDateRange(DateTime startDate, DateTime endDate)
    {
        return _db.AssetRequests
            .Where(a => a.RequestDate >= startDate && a.RequestDate <= endDate)
            .ToList();
    }

    public void AddAssetRequest(AssetRequest assetRequest)
    {
        _db.AssetRequests.Add(assetRequest);
    }

    public void UpdateAssetRequest(AssetRequest assetRequest)
    {
        var request = _db.AssetRequests.FirstOrDefault(a => a.AssetRequestID == assetRequest.AssetRequestID);
        request.RequestResultID = assetRequest.RequestResultID;
        request.AssetID = assetRequest.AssetID;
        request.EmployeeID = assetRequest.EmployeeID;
        request.RequestNote = assetRequest.RequestNote;
        request.DepartmentID = assetRequest.DepartmentID;
    }

    public void DeleteAssetRequest(int assetRequestId)
    {
        var request = _db.AssetRequests.FirstOrDefault(a => a.AssetRequestID == assetRequestId);
        _db.AssetRequests.Remove(request);
    }
}
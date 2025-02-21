using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Data.Repositories;

public class AssetRequestRepository : IAssetRequestRepository
{
    private ITAssetTrackingContext _context;

    public AssetRequestRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public AssetRequest? GetAssetRequestById(int assetRequestId)
    {
        return _context.AssetRequest.FirstOrDefault(a => a.AssetRequestID == assetRequestId);
    }

    public List<AssetRequest> GetAssetRequests()
    {
        return _context.AssetRequest.ToList();
    }

    public List<AssetRequest> GetOpenAssetRequests()
    {
        return _context.AssetRequest
            .Where(a => a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByAssetId(int assetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.AssetRequest
                .Where(a => a.AssetID == assetId)
                .ToList();
        }
        return _context.AssetRequest
            .Where(a => a.AssetID == assetId && a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByEmployeeId(int employeeId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.AssetRequest
                .Where(a => a.EmployeeID == employeeId)
                .ToList();
        }
        return _context.AssetRequest
            .Where(a => a.EmployeeID == employeeId && a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByDepartmentId(int departmentId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.AssetRequest
                .Where(a => a.DepartmentID == departmentId)
                .ToList();
        }
        return _context.AssetRequest
            .Where(a => a.DepartmentID == departmentId && a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByResultId(int requestResultId)
    {
        return _context.AssetRequest
            .Where(a => a.RequestResultID == requestResultId)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestInDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.AssetRequest
            .Where(a => a.RequestDate >= startDate && a.RequestDate <= endDate)
            .ToList();
    }

    public void AddAssetRequest(AssetRequest assetRequest)
    {
        _context.AssetRequest.Add(assetRequest);
        _context.SaveChanges();
    }

    public void UpdateAssetRequest(AssetRequest assetRequest)
    {
        _context.AssetRequest.Update(assetRequest);
        _context.SaveChanges();
    }

    public void DeleteAssetRequest(AssetRequest assetRequest)
    {
        _context.AssetRequest.Remove(assetRequest);
        _context.SaveChanges();
    }
}
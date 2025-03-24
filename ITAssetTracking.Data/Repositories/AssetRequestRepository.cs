using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
            .Include(a => a.Asset)
                .ThenInclude(a => a.Model)
            .Include(a => a.Asset)
                .ThenInclude(a => a.AssetType)
            .Include(a => a.Department)
            .Include(a => a.Employee)
            .Where(a => a.RequestResultID == null)
            .ToList();
    }

    public List<AssetRequest> GetAssetRequestsByAssetId(long assetId, bool includeClosed)
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
        var request = _context.AssetRequest.FirstOrDefault(a => a.AssetRequestID == assetRequest.AssetRequestID);
        if (request != null)
        {
            request.RequestDate = assetRequest.RequestDate;
            request.RequestResultID = assetRequest.RequestResultID;
            request.EmployeeID = assetRequest.EmployeeID;
            request.DepartmentID = assetRequest.DepartmentID;
            request.AssetID = assetRequest.AssetID;
            request.RequestNote = assetRequest.RequestNote;
            _context.SaveChanges();
        }
    }

    public void DeleteAssetRequest(int assetRequestId)
    {
        var request = _context.AssetRequest.FirstOrDefault(a => a.AssetRequestID == assetRequestId);
        if (request != null)
        {
            _context.AssetRequest.Remove(request);
            _context.SaveChanges();
        }
    }

    public RequestResult? GetAssetRequestResult(string resultName)
    {
        return _context.RequestResult.FirstOrDefault(r => r.RequestResultName == resultName);
    }
}
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Data.Repositories;

public class SoftwareAssetRequestRepository : ISoftwareAssetRequestRepository
{
    private ITAssetTrackingContext _context;

    public SoftwareAssetRequestRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public SoftwareAssetRequest? GetSoftwareAssetRequestById(int softwareAssetRequestId)
    {
        return _context.SoftwareAssetRequest
            .FirstOrDefault(s => s.SoftwareAssetRequestID == softwareAssetRequestId);
    }

    public List<SoftwareAssetRequest> GetSoftwareAssetRequests()
    {
        return _context.SoftwareAssetRequest.ToList();
    }

    public List<SoftwareAssetRequest> GetOpenSoftwareAssetRequests()
    {
        return _context.SoftwareAssetRequest
            .Where(s => s.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsByEmployeeId(int employeeId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.SoftwareAssetRequest
                .Where(s => s.EmployeeID == employeeId)
                .ToList();
        }
        return _context.SoftwareAssetRequest
            .Where(s => s.EmployeeID == employeeId && s.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsBySoftwareId(int softwareAssetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.SoftwareAssetRequest
                .Where(s => s.SoftwareAssetID == softwareAssetId)
                .ToList();
        }
        return _context.SoftwareAssetRequest
            .Where(s => s.SoftwareAssetID == softwareAssetId && s.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsByAssetId(long assetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.SoftwareAssetRequest
                .Where(s => s.AssetID == assetId)
                .ToList();
        }
        return _context.SoftwareAssetRequest
            .Where(s => s.AssetID == assetId && s.RequestResultID == null)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsByResultId(int requestResultId)
    {
        return _context.SoftwareAssetRequest
            .Where(s => s.RequestResultID == requestResultId)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetRequestsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.SoftwareAssetRequest
            .Where(s => s.RequestDate >= startDate && s.RequestDate <= endDate)
            .ToList();
    }

    public void AddRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        _context.SoftwareAssetRequest.Add(softwareAssetRequest);
        _context.SaveChanges();
    }

    public void UpdateRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        _context.SoftwareAssetRequest.Update(softwareAssetRequest);
        _context.SaveChanges();
    }

    public void DeleteRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        _context.SoftwareAssetRequest.Remove(softwareAssetRequest);
        _context.SaveChanges();
    }
}
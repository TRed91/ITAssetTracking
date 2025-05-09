﻿using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
            .Include(s => s.Asset)
            .ThenInclude(a => a.Model)
            .Include(s => s.Asset)
            .ThenInclude(a => a.AssetType)
            .Include(s => s.SoftwareAsset)
            .ThenInclude(sa => sa.LicenseType)
            .Include(s => s.Employee)
            .FirstOrDefault(s => s.SoftwareAssetRequestID == softwareAssetRequestId);
    }

    public List<SoftwareAssetRequest> GetSoftwareAssetRequests()
    {
        return _context.SoftwareAssetRequest
            .Include(r => r.Asset)
            .Include(r => r.SoftwareAsset)
            .ThenInclude(s => s.LicenseType)
            .Include(r => r.Employee)
            .Include(r => r.RequestResult)
            .ToList();
    }

    public List<SoftwareAssetRequest> GetOpenSoftwareAssetRequests()
    {
        return _context.SoftwareAssetRequest
            .Include(s => s.SoftwareAsset)
            .ThenInclude(sa => sa.LicenseType)
            .Include(s => s.Employee)
            .Include(s => s.Asset)
            .ThenInclude(a => a.Model)
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
        var requestToUpdate = _context.SoftwareAssetRequest
            .FirstOrDefault(s => s.SoftwareAssetRequestID == softwareAssetRequest.SoftwareAssetRequestID);
        if (requestToUpdate != null)
        {
            requestToUpdate.RequestDate = softwareAssetRequest.RequestDate;
            requestToUpdate.RequestResultID = softwareAssetRequest.RequestResultID;
            requestToUpdate.EmployeeID = softwareAssetRequest.EmployeeID;
            requestToUpdate.AssetID = softwareAssetRequest.AssetID;
            requestToUpdate.RequestNote = softwareAssetRequest.RequestNote;
            requestToUpdate.SoftwareAssetID = softwareAssetRequest.SoftwareAssetID;
            _context.SaveChanges();
        }
    }

    public void DeleteRequest(int softwareAssetRequestId)
    {
        var request = _context.SoftwareAssetRequest
            .FirstOrDefault(s => s.SoftwareAssetRequestID == softwareAssetRequestId);
        if (request != null)
        {
            _context.SoftwareAssetRequest.Remove(request);
            _context.SaveChanges();
        }
    }
}
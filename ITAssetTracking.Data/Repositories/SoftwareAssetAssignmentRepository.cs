using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITAssetTracking.Data.Repositories;

public class SoftwareAssetAssignmentRepository : ISoftwareAssetAssignmentRepository
{
    private ITAssetTrackingContext _context;

    public SoftwareAssetAssignmentRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public SoftwareAssetAssignment? GetSoftwareAssetAssignmentById(int softwareAssetAssignmentId)
    {
        return _context.SoftwareAssetAssignment.FirstOrDefault(s => s.AssetAssignmentID == softwareAssetAssignmentId);
    }

    public List<SoftwareAssetAssignment> GetSoftwareAssetAssignments(bool includeReturned)
    {
        if (includeReturned)
        {
            return _context.SoftwareAssetAssignment.ToList();
        }
        return _context.SoftwareAssetAssignment
            .Where(s => s.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsBySoftwareAssetId(int softwareAssetId, bool includeReturned)
    {
        return _context.SoftwareAssetAssignment
            .Include(s => s.SoftwareAsset)
            .ThenInclude(sa => sa.Manufacturer)
            .Include(s => s.SoftwareAsset)
            .ThenInclude(sa => sa.LicenseType)
            .Include(s => s.SoftwareAsset)
            .ThenInclude(sa => sa.AssetStatus)
            .Include(s => s.Asset)
            .ThenInclude(a => a.Model)
            .Include(s => s.Employee)
            .Where(s => (includeReturned || s.ReturnDate == null) && s.SoftwareAssetID == softwareAssetId)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsByEmployeeId(int employeeId, bool includeReturned)
    {
        return _context.SoftwareAssetAssignment
            .Include(s => s.SoftwareAsset)
                .ThenInclude(sa => sa.LicenseType)
            .Include(s => s.SoftwareAsset)
            .ThenInclude(sa => sa.AssetStatus)
            .Where(s => s.EmployeeID == employeeId && (includeReturned || s.ReturnDate == null))
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsByDepartmentId(int departmentId, bool includeReturned)
    {
        return  _context.SoftwareAssetAssignment
            .Include(a => a.Employee)
            .Include(a => a.Asset)
            .Include(a => a.SoftwareAsset)
                .ThenInclude(sa => sa.LicenseType)
            .Include(a => a.SoftwareAsset)
                .ThenInclude(sa => sa.AssetStatus)
            .Include(a => a.SoftwareAsset)
                .ThenInclude(sa => sa.Manufacturer)
            .Where(a => a.Employee.DepartmentID == departmentId && (includeReturned || a.ReturnDate == null))
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentByAssetId(long assetId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _context.SoftwareAssetAssignment
                .Where(s => s.AssetID == assetId)
                .ToList();
        }
        return _context.SoftwareAssetAssignment
            .Where(s => s.AssetID == assetId && s.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.SoftwareAssetAssignment
            .Where(s => s.AssignmentDate >= startDate && s.AssignmentDate <= endDate)
            .ToList();
    }

    public void AddSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        _context.SoftwareAssetAssignment.Add(softwareAssetAssignment);
        _context.SaveChanges();
    }

    public void UpdateSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        var assignmnent = _context.SoftwareAssetAssignment
            .FirstOrDefault(s => s.AssetAssignmentID == softwareAssetAssignment.AssetAssignmentID);
        if (assignmnent != null)
        {
            assignmnent.AssetID = softwareAssetAssignment.AssetID;
            assignmnent.AssignmentDate = softwareAssetAssignment.AssignmentDate;
            assignmnent.EmployeeID = softwareAssetAssignment.EmployeeID;
            assignmnent.ReturnDate = softwareAssetAssignment.ReturnDate;
            assignmnent.SoftwareAssetID = softwareAssetAssignment.SoftwareAssetID;
            _context.SaveChanges();
        }
    }

    public void DeleteSoftwareAssetAssignment(int softwareAssetAssignmentId)
    {
        var assignment = _context.SoftwareAssetAssignment
            .FirstOrDefault(s => s.AssetAssignmentID == softwareAssetAssignmentId);
        if (assignment != null)
        {
            _context.SoftwareAssetAssignment.Remove(assignment);
            _context.SaveChanges();
        }
    }
}
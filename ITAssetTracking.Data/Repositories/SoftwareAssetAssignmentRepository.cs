using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

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
        if (includeReturned)
        {
            return _context.SoftwareAssetAssignment
                .Where(s => s.SoftwareAssetID == softwareAssetId)
                .ToList();
        }
        return _context.SoftwareAssetAssignment
            .Where(s => s.SoftwareAssetID == softwareAssetId && s.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsByEmployeeId(int employeeId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _context.SoftwareAssetAssignment
                .Where(s => s.EmployeeID == employeeId)
                .ToList();
        }
        return _context.SoftwareAssetAssignment
            .Where(s => s.EmployeeID == employeeId && s.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentByAssetId(int assetId, bool includeReturned)
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

    public void AddSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        _context.SoftwareAssetAssignment.Add(softwareAssetAssignment);
        _context.SaveChanges();
    }

    public void UpdateSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        _context.SoftwareAssetAssignment.Update(softwareAssetAssignment);
        _context.SaveChanges();
    }

    public void DeleteSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        _context.SoftwareAssetAssignment.Remove(softwareAssetAssignment);
        _context.SaveChanges();
    }
}
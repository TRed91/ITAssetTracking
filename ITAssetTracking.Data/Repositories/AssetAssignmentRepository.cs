using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Data.Repositories;

public class AssetAssignmentRepository : IAssetAssignmentRepository
{
    private ITAssetTrackingContext _context;

    public AssetAssignmentRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public AssetAssignment? GetAssetAssignmentById(int assetAssignmentId)
    {
        return _context.AssetAssignment
            .FirstOrDefault(a => a.AssetAssignmentID == assetAssignmentId);
    }

    public List<AssetAssignment> GetAllAssetAssignments(bool includeReturned)
    {
        if (includeReturned)
        {
            return _context.AssetAssignment.ToList();
        }
        return _context.AssetAssignment
            .Where(a => a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByAssetId(int assetId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _context.AssetAssignment
                .Where(a => a.AssetID == assetId)
                .ToList();
        }
        return _context.AssetAssignment
            .Where(a => a.AssetID == assetId && a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByDepartmentId(int departmentId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _context.AssetAssignment
                .Where(a => a.DepartmentID == departmentId)
                .ToList();
        }
        return _context.AssetAssignment
            .Where(a => a.DepartmentID == departmentId && a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByEmployeeId(int employeeId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _context.AssetAssignment
                .Where(a => a.EmployeeID == employeeId)
                .ToList();
        }
        return _context.AssetAssignment
            .Where(a => a.EmployeeID == employeeId && a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.AssetAssignment
            .Where(a => a.AssignmentDate >= startDate && a.AssignmentDate <= endDate)
            .ToList();
    }

    public void AddAssetAssignment(AssetAssignment assetAssignment)
    {
        _context.AssetAssignment.Add(assetAssignment);
        _context.SaveChanges();
    }

    public void UpdateAssetAssignment(AssetAssignment assetAssignment)
    {
        _context.AssetAssignment.Update(assetAssignment);
        _context.SaveChanges();
    }

    public void DeleteAssetAssignment(AssetAssignment assetAssignment)
    {
        _context.AssetAssignment.Remove(assetAssignment);
        _context.SaveChanges();
    }
}
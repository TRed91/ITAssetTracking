using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
            .Include(a => a.Asset)
            .Include(a => a.Department)
            .Include(a => a.Employee)
            .FirstOrDefault(a => a.AssetAssignmentID == assetAssignmentId);
    }

    public List<AssetAssignment> GetAllAssetAssignments(bool includeReturned)
    {
        return _context.AssetAssignment
            .Include(a => a.Asset)
            .Include(a => a.Department)
            .Include(a => a.Employee)
            .Where(a => includeReturned || a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByAssetId(long assetId, bool includeReturned)
    {
        return _context.AssetAssignment
            .Include(a => a.Asset)
            .Include(a => a.Department)
            .Include(a => a.Employee)
            .Where(a => a.AssetID == assetId && (includeReturned || a.ReturnDate == null))
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByDepartmentId(int departmentId, bool includeReturned)
    {
        return _context.AssetAssignment
            .Include(a => a.Asset)
            .ThenInclude(a => a.AssetStatus)
            .Include(a => a.Asset)
            .ThenInclude(a => a.AssetType)
            .Include(a => a.Asset)
            .ThenInclude(a => a.Manufacturer)
            .Include(a => a.Asset)
            .ThenInclude(a => a.Model)
            .Include(a => a.Asset)
            .ThenInclude(a => a.Location)
            .Include(a => a.Department)
            .Include(a => a.Employee)
            .Where(a => a.DepartmentID == departmentId && (includeReturned || a.ReturnDate == null))
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByEmployeeId(int employeeId, bool includeReturned)
    {
        return _context.AssetAssignment
            .Include(a => a.Asset)
            .ThenInclude(a => a.Model)
            .Include(a => a.Asset)
            .ThenInclude(a => a.AssetType)
            .Include(a => a.Asset)
            .ThenInclude(a => a.AssetStatus)
            .Include(a => a.Department)
            .Include(a => a.Employee)
            .Where(a => a.EmployeeID == employeeId && (includeReturned || a.ReturnDate == null))
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.AssetAssignment
            .Include(a => a.Asset)
            .Include(a => a.Department)
            .Include(a => a.Employee)
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
        var assignment = _context.AssetAssignment
            .FirstOrDefault(a => a.AssetAssignmentID == assetAssignment.AssetAssignmentID);
        if (assignment != null)
        {
            assignment.AssignmentDate = assetAssignment.AssignmentDate;
            assignment.AssetID = assetAssignment.AssetID;
            assignment.ReturnDate = assetAssignment.ReturnDate;
            assignment.DepartmentID = assetAssignment.DepartmentID;
            assignment.EmployeeID = assetAssignment.EmployeeID;
            _context.SaveChanges();
        }
        
    }

    public void DeleteAssetAssignment(int assetAssignmentId)
    {
        var assignment = _context.AssetAssignment
            .FirstOrDefault(a => a.AssetAssignmentID == assetAssignmentId);
        if (assignment != null)
        {
            _context.AssetAssignment.Remove(assignment);
            _context.SaveChanges();
        }
    }
}
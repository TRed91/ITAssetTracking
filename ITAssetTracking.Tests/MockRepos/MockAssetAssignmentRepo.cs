using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockAssetAssignmentRepo : IAssetAssignmentRepository
{
    private readonly MockDB _db;

    public MockAssetAssignmentRepo()
    {
        _db = new MockDB();
    }
    
    public AssetAssignment? GetAssetAssignmentById(int assetAssignmentId)
    {
        return _db.AssetAssignments.FirstOrDefault(a => a.AssetAssignmentID == assetAssignmentId);
    }

    public List<AssetAssignment> GetAllAssetAssignments(bool includeReturned)
    {
        return _db.AssetAssignments;
    }

    public List<AssetAssignment> GetAssetAssignmentsByAssetId(long assetId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _db.AssetAssignments
                .Where(a => a.AssetID == assetId)
                .ToList();
        }
        return _db.AssetAssignments
            .Where(a => a.AssetID == assetId && a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByDepartmentId(int departmentId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _db.AssetAssignments
                .Where(a => a.DepartmentID == departmentId)
                .ToList();
        }
        return _db.AssetAssignments
            .Where(a => a.DepartmentID == departmentId && a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsByEmployeeId(int employeeId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _db.AssetAssignments
                .Where(a => a.EmployeeID == employeeId)
                .ToList();
        }
        return _db.AssetAssignments
            .Where(a => a.EmployeeID == employeeId && a.ReturnDate == null)
            .ToList();
    }

    public List<AssetAssignment> GetAssetAssignmentsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _db.AssetAssignments
            .Where(a => a.AssignmentDate >= startDate && a.AssignmentDate <= endDate)
            .ToList();
    }

    public void AddAssetAssignment(AssetAssignment assetAssignment)
    {
        assetAssignment.AssetAssignmentID = _db.AssetAssignments.Max(a => a.AssetAssignmentID) + 1;
        _db.AssetAssignments.Add(assetAssignment);
    }

    public void UpdateAssetAssignment(AssetAssignment assetAssignment)
    {
        var assetAssignmentToUpdate = _db.AssetAssignments
            .FirstOrDefault(a => a.AssetAssignmentID == assetAssignment.AssetAssignmentID);
        
        assetAssignmentToUpdate.DepartmentID = assetAssignment.DepartmentID;
        assetAssignmentToUpdate.EmployeeID = assetAssignment.EmployeeID;
        assetAssignmentToUpdate.AssetID = assetAssignment.AssetID;
        assetAssignmentToUpdate.ReturnDate = assetAssignment.ReturnDate;
    }

    public void DeleteAssetAssignment(AssetAssignment assetAssignment)
    {
        _db.AssetAssignments.Remove(assetAssignment);
    }
}
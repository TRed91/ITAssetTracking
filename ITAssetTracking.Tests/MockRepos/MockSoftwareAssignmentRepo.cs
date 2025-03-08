using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockSoftwareAssignmentRepo : ISoftwareAssetAssignmentRepository
{
    private readonly MockDB _db;

    public MockSoftwareAssignmentRepo()
    {
        _db = new MockDB();
    }
    public SoftwareAssetAssignment? GetSoftwareAssetAssignmentById(int softwareAssetAssignmentId)
    {
        return _db.SoftwareAssetAssignments
            .FirstOrDefault(a => a.AssetAssignmentID == softwareAssetAssignmentId);
    }

    public List<SoftwareAssetAssignment> GetSoftwareAssetAssignments(bool includeReturned)
    {
        return includeReturned ? _db.SoftwareAssetAssignments : _db.SoftwareAssetAssignments
            .Where(a => a.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsBySoftwareAssetId(int softwareAssetId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _db.SoftwareAssetAssignments
                .Where(a => a.SoftwareAssetID == softwareAssetId)
                .ToList();
        }
        return _db.SoftwareAssetAssignments
            .Where(a => a.SoftwareAssetID == softwareAssetId && a.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsByEmployeeId(int employeeId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _db.SoftwareAssetAssignments
                .Where(a => a.EmployeeID == employeeId)
                .ToList();
        }
        return _db.SoftwareAssetAssignments
            .Where(a => a.EmployeeID == employeeId && a.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentByAssetId(long assetId, bool includeReturned)
    {
        if (includeReturned)
        {
            return _db.SoftwareAssetAssignments
                .Where(a => a.AssetID == assetId)
                .ToList();
        }
        return _db.SoftwareAssetAssignments
            .Where(a => a.AssetID == assetId && a.ReturnDate == null)
            .ToList();
    }

    public List<SoftwareAssetAssignment> GetAssignmentsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _db.SoftwareAssetAssignments
            .Where(a => a.AssignmentDate >= startDate && a.AssignmentDate <= endDate)
            .ToList();
    }

    public void AddSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        softwareAssetAssignment.AssetAssignmentID = _db.SoftwareAssetAssignments.Max(a => a.AssetAssignmentID) + 1;
        _db.SoftwareAssetAssignments.Add(softwareAssetAssignment);
    }

    public void UpdateSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        var assignment = _db.SoftwareAssetAssignments
            .FirstOrDefault(a => a.AssetAssignmentID == softwareAssetAssignment.AssetAssignmentID);
        assignment.EmployeeID = softwareAssetAssignment.EmployeeID;
        assignment.SoftwareAssetID = softwareAssetAssignment.SoftwareAssetID;
        assignment.AssetID = softwareAssetAssignment.AssetID;
        assignment.ReturnDate = softwareAssetAssignment.ReturnDate;
    }

    public void DeleteSoftwareAssetAssignment(int softwareAssetAssignmentId)
    {
        var softwareAssetAssignment = _db.SoftwareAssetAssignments.FirstOrDefault(a => a.AssetAssignmentID == softwareAssetAssignmentId);
        _db.SoftwareAssetAssignments.Remove(softwareAssetAssignment);
    }
}
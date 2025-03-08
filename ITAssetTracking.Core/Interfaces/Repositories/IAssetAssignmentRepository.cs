using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IAssetAssignmentRepository
{
    AssetAssignment? GetAssetAssignmentById(int assetAssignmentId);
    
    List<AssetAssignment> GetAllAssetAssignments(bool includeReturned);
    List<AssetAssignment> GetAssetAssignmentsByAssetId(long assetId, bool includeReturned);
    List<AssetAssignment> GetAssetAssignmentsByDepartmentId(int departmentId, bool includeReturned);
    List<AssetAssignment> GetAssetAssignmentsByEmployeeId(int employeeId, bool includeReturned);
    List<AssetAssignment> GetAssetAssignmentsInDateRange(DateTime startDate, DateTime endDate);
    
    void AddAssetAssignment(AssetAssignment assetAssignment);
    void UpdateAssetAssignment(AssetAssignment assetAssignment);
    void DeleteAssetAssignment(int assetAssignmentId);
}
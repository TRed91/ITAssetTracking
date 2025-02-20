using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IAssetAssignmentRepository
{
    AssetAssignment? GetAssetAssignmentById(int assetAssignmentId);
    
    List<AssetAssignment> GetAllAssetAssignments();
    List<AssetAssignment> GetAssetAssignmentsByAssetId(int assetId);
    List<AssetAssignment> GetAssetAssignmentsByDepartmentId(int departmentId);
    List<AssetAssignment> GetAssetAssignmentsByEmployeeId(int employeeId);
    List<AssetAssignment> GetAssetAssignmentsInDateRange(DateTime startDate, DateTime endDate);
    
    void AddAssetAssignment(AssetAssignment assetAssignment);
    void UpdateAssetAssignment(AssetAssignment assetAssignment);
    void DeleteAssetAssignment(AssetAssignment assetAssignment);
}
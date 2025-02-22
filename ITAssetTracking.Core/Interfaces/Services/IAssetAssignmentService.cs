using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface IAssetAssignmentService
{
    Result<AssetAssignment> GetAssetAssignmentById(int assetAssignmentId);
    
    Result<List<AssetAssignment>> GetAllAssetAssignments(bool includeReturned = true);
    Result<List<AssetAssignment>> GetAssetAssignmentsByAsset(long assetId, bool includeReturned = true);
    Result<List<AssetAssignment>> GetAssetAssignmentsByDepartment(int departmentId, bool includeReturned = true);
    Result<List<AssetAssignment>> GetAssetAssignmentsByEmployee(int employeeId, bool includeReturned = true);
    Result<List<AssetAssignment>> GetAssetAssignmentsInDateRange(DateTime startDate, DateTime endDate);
    
    Result AddAssetAssignment(AssetAssignment assetAssignment);
    Result UpdateAssetAssignment(AssetAssignment assetAssignment);
    Result DeleteAssetAssignment(int assetAssignmentId);
}
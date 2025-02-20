using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface ISoftwareAssetAssignmentService
{
    Result<SoftwareAssetAssignment> GetSoftwareAssetAssignment(int softwareAssetAssignmentId);
    
    Result<List<SoftwareAssetAssignment>> GetSoftwareAssetAssignments(bool includeReturned = true);
    Result<List<SoftwareAssetAssignment>> GetAssignmentsBySoftwareAssetId(
        int softwareAssetId, bool includeReturned = true);
    Result<List<SoftwareAssetAssignment>> GetAssignmentsByEmployee(int employeeId, bool includeReturned = true);
    Result<List<SoftwareAssetAssignment>> GetAssignmentByAsset(int assetId, bool includeReturned = true);
    
    Result AddSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
    Result UpdateSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
    Result DeleteSoftwareAssetAssignment(int softwareAssetAssignmentId);
}
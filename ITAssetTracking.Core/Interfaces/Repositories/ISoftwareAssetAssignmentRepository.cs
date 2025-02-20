using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ISoftwareAssetAssignmentRepository
{
    SoftwareAssetAssignment? GetSoftwareAssetAssignmentById(int softwareAssetAssignmentId);
    
    List<SoftwareAssetAssignment> GetSoftwareAssetAssignments(bool includeReturned);
    List<SoftwareAssetAssignment> GetAssignmentsBySoftwareAssetId(int softwareAssetId, bool includeReturned);
    List<SoftwareAssetAssignment> GetAssignmentsByEmployeeId(int employeeId, bool includeReturned);
    List<SoftwareAssetAssignment> GetAssignmentByAssetId(int assetId, bool includeReturned);
    
    void AddSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
    void UpdateSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
    void DeleteSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
}
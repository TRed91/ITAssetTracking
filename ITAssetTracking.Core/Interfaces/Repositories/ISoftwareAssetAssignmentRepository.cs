using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ISoftwareAssetAssignmentRepository
{
    SoftwareAssetAssignment? GetSoftwareAssetAssignmentById(int softwareAssetAssignmentId);
    
    List<SoftwareAssetAssignment> GetSoftwareAssetAssignments();
    List<SoftwareAssetAssignment> GetAssignmentsBySoftwareAssetId(int softwareAssetId);
    List<SoftwareAssetAssignment> GetAssignmentsByEmployeeId(int employeeId);
    List<SoftwareAssetAssignment> GetAssignmentByAssetId(int assetId);
    
    void AddSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
    void UpdateSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
    void DeleteSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment);
}
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Core.Interfaces;

public interface IAppConfig
{
    string[] AllowedOrigins();
    IAssetAssignmentRepository GetAssetAssignmentRepository();
    IAssetRepository GetAssetRepository();
    IAssetRequestRepository GetAssetRequestRepository();
    IDepartmentRepository GetDepartmentRepository();
    IEmployeeRepository GetEmployeeRepository();
    ISoftwareAssetAssignmentRepository GetSoftwareAssetAssignmentRepository();
    ISoftwareAssetRepository GetSoftwareAssetRepository();
    ISoftwareAssetRequestRepository GetSoftwareAssetRequestRepository();
    ITicketRepository GetTicketRepository();
    IReportsRepository GetReportsRepository();
}
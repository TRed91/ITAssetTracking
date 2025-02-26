using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.Data.Repositories;

namespace ITAssetTracking.App.Utility;

public class ServiceFactory
{
    private readonly IAssetAssignmentRepository _assetAssignmentRepo;
    private readonly IAssetRepository _assetRepo;
    private readonly IAssetRequestRepository _assetRequestRepo;
    private readonly IDepartmentRepository _departmentRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly ISoftwareAssetAssignmentRepository _softwareAssetAssignmentRepo;
    private readonly ISoftwareAssetRepository _softwareAssetRepo;
    private readonly ISoftwareAssetRequestRepository _softwareAssetRequestRepo;
    private readonly ITicketRepository _ticketRepo;

    public ServiceFactory(ITAssetTrackingContext context)
    {
        _assetAssignmentRepo = new AssetAssignmentRepository(context);
        _assetRepo = new AssetRepository(context);
        _assetRequestRepo = new AssetRequestRepository(context);
        _departmentRepo = new DepartmentRepository(context);
        _employeeRepo = new EmployeeRepository(context);
        _softwareAssetAssignmentRepo = new SoftwareAssetAssignmentRepository(context);
        _softwareAssetRepo = new SoftwareAssetRepository(context);
        _softwareAssetRequestRepo = new SoftwareAssetRequestRepository(context);
        _ticketRepo = new TicketRepository(context);
    }
    
    public IAssetAssignmentService GetAssetAssignmentService()
    {
        return new AssetAssignmentService(_assetAssignmentRepo, _employeeRepo, _departmentRepo, _assetRepo);
    }

    public IAssetService GetAssetRepository()
    {
        return new AssetService(_assetRepo);
    }

    public IAssetRequestService GetAssetRequestService()
    {
        return new AssetRequestService(_assetRequestRepo, _assetRepo, _employeeRepo, _departmentRepo);
    }

    public IDepartmentService GetDepartmentService()
    {
        return new DepartmentService(_departmentRepo);
    }

    public IEmployeeService GetEmployeeService()
    {
        return new EmployeeService(_employeeRepo, _departmentRepo);
    }

    public ISoftwareAssetAssignmentService GetSoftwareAssetAssignmentService()
    {
        return new SoftwareAssetAssignmentService(
            _softwareAssetAssignmentRepo, 
            _softwareAssetRepo, 
            _assetRepo,
            _employeeRepo, 
            _assetAssignmentRepo);
    }

    public ISoftwareAssetService GetSoftwareAssetService()
    {
        return new SoftwareAssetService(_softwareAssetRepo);
    }

    public ISoftwareRequestService GetSoftwareRequestService()
    {
        return new SoftwareRequestService(
            _softwareAssetRequestRepo,
            _softwareAssetRepo,
            _softwareAssetAssignmentRepo,
            _assetRepo,
            _employeeRepo);
    }

    public ITicketService GetTicketService()
    {
        return new TicketService(_ticketRepo, _employeeRepo, _assetRepo);
    }
}
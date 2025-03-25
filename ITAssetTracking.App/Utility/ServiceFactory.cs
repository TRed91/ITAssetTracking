using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Interfaces;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.Data.Repositories;

namespace ITAssetTracking.App.Utility;

public class ServiceFactory
{
    private readonly IAppConfig _appConfig;

    public ServiceFactory(IAppConfig appConfig)
    {
        _appConfig = appConfig;
    }
    
    public IAssetAssignmentService GetAssetAssignmentService()
    {
        return new AssetAssignmentService(
            _appConfig.GetAssetAssignmentRepository(),
            _appConfig.GetEmployeeRepository(),
            _appConfig.GetDepartmentRepository(),
            _appConfig.GetAssetRepository());
    }

    public IAssetService GetAssetService()
    {
        return new AssetService(
            _appConfig.GetAssetRepository(),
            _appConfig.GetAssetAssignmentRepository());
    }

    public IAssetRequestService GetAssetRequestService()
    {
        return new AssetRequestService(
            _appConfig.GetAssetRequestRepository(), 
            _appConfig.GetAssetAssignmentRepository(),
            _appConfig.GetAssetRepository(), 
            _appConfig.GetEmployeeRepository(), 
            _appConfig.GetDepartmentRepository());
    }

    public IDepartmentService GetDepartmentService()
    {
        return new DepartmentService(_appConfig.GetDepartmentRepository());
    }

    public IEmployeeService GetEmployeeService()
    {
        return new EmployeeService(
            _appConfig.GetEmployeeRepository(), 
            _appConfig.GetDepartmentRepository());
    }

    public ISoftwareAssetAssignmentService GetSoftwareAssetAssignmentService()
    {
        return new SoftwareAssetAssignmentService(
            _appConfig.GetSoftwareAssetAssignmentRepository(), 
            _appConfig.GetSoftwareAssetRepository(), 
            _appConfig.GetAssetRepository(),
            _appConfig.GetEmployeeRepository());
    }

    public ISoftwareAssetService GetSoftwareAssetService()
    {
        return new SoftwareAssetService(
            _appConfig.GetSoftwareAssetRepository(),
            _appConfig.GetAssetRepository());
    }

    public ISoftwareRequestService GetSoftwareRequestService()
    {
        return new SoftwareRequestService(
            _appConfig.GetSoftwareAssetRequestRepository(),
            _appConfig.GetSoftwareAssetRepository(),
            _appConfig.GetSoftwareAssetAssignmentRepository(),
            _appConfig.GetAssetRepository(),
            _appConfig.GetAssetRequestRepository(),
            _appConfig.GetEmployeeRepository());
    }

    public ITicketService GetTicketService()
    {
        return new TicketService(
            _appConfig.GetTicketRepository(), 
            _appConfig.GetEmployeeRepository(), 
            _appConfig.GetAssetRepository());
    }
}
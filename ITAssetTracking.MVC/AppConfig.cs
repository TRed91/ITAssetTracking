using ITAssetTracking.Core.Interfaces;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Data;
using ITAssetTracking.Data.Repositories;

namespace ITAssetTracking.MVC;

public class AppConfig : IAppConfig
{
    private readonly IConfiguration _config;

    public AppConfig(IConfiguration configuration)
    {
        _config = configuration;
    }

    private ITAssetTrackingContext GetContext()
    {
        if (string.IsNullOrEmpty(_config["ConnectionString"]))
        {
            throw new ApplicationException("Missing connection string");
        }
        return new ITAssetTrackingContext(_config["ConnectionString"]);
    }

    public IAssetAssignmentRepository GetAssetAssignmentRepository()
    {
        return new AssetAssignmentRepository(GetContext());
    }

    public IAssetRepository GetAssetRepository()
    {
        return new AssetRepository(GetContext());
    }

    public IAssetRequestRepository GetAssetRequestRepository()
    {
        return new AssetRequestRepository(GetContext());
    }

    public IDepartmentRepository GetDepartmentRepository()
    {
        return new DepartmentRepository(GetContext());
    }

    public IEmployeeRepository GetEmployeeRepository()
    {
        return new EmployeeRepository(GetContext());
    }

    public ISoftwareAssetAssignmentRepository GetSoftwareAssetAssignmentRepository()
    {
        return new SoftwareAssetAssignmentRepository(GetContext());
    }

    public ISoftwareAssetRepository GetSoftwareAssetRepository()
    {
        return new SoftwareAssetRepository(GetContext());
    }

    public ISoftwareAssetRequestRepository GetSoftwareAssetRequestRepository()
    {
        return new SoftwareAssetRequestRepository(GetContext());
    }

    public ITicketRepository GetTicketRepository()
    {
        return new TicketRepository(GetContext());
    }
    
}
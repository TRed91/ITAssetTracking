using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepo;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepo = departmentRepository;
    }
    
    public Result<Department> GetDepartmentById(int departmentId)
    {
        try
        {
            var department = _departmentRepo.GetDepartmentById(departmentId);
            if (department == null)
            {
                return ResultFactory.Fail<Department>("Department not found");
            }

            return ResultFactory.Success(department);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Department>(ex.Message, ex);
        }
    }

    public Result<List<Department>> GetDepartments()
    {
        try
        {
            var departments = _departmentRepo.GetDepartments();
            return ResultFactory.Success(departments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Department>>(ex.Message, ex);
        }
    }
}
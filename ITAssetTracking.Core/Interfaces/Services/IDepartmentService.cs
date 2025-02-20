using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface IDepartmentService
{
    Result<Department> GetDepartmentById(int departmentId);
    Result<List<Department>> GetDepartments();
}
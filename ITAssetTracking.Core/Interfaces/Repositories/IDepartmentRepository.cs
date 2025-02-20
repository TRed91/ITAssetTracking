using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IDepartmentRepository
{
    Department? GetDepartmentById(int departmentId);
    
    List<Department> GetDepartments();
}
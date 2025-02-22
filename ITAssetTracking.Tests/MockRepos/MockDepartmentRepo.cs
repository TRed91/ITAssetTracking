using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockDepartmentRepo : IDepartmentRepository
{
    private readonly MockDB _db;

    public MockDepartmentRepo()
    {
        _db = new MockDB();
    }
    
    public Department? GetDepartmentById(int departmentId)
    {
        return _db.Departments.FirstOrDefault(d => d.DepartmentID == departmentId);
    }

    public List<Department> GetDepartments()
    {
        return _db.Departments;
    }
}
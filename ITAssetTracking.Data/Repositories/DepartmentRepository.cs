using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITAssetTracking.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private ITAssetTrackingContext _context;

    public DepartmentRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public Department? GetDepartmentById(int departmentId)
    {
        return _context.Department
            .Include(d => d.Employees)
            .FirstOrDefault(d => d.DepartmentID == departmentId);
    }

    public List<Department> GetDepartments()
    {
        return _context.Department.ToList();
    }
}
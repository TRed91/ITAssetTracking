using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private ITAssetTrackingContext _context;

    public EmployeeRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public Employee? GetEmployee(int employeeId)
    {
        return _context.Employee.FirstOrDefault(e => e.EmployeeID == employeeId);
    }

    public List<Employee> GetEmployees()
    {
        return _context.Employee.ToList();
    }

    public List<Employee> GetEmployeesByDepartmentId(int departmentId)
    {
        return _context.Employee
            .Where(e => e.DepartmentID == departmentId)
            .ToList();
    }

    public void AddEmployee(Employee employee)
    {
        _context.Employee.Add(employee);
        _context.SaveChanges();
    }

    public void UpdateEmployee(Employee employee)
    {
        _context.Employee.Update(employee);
        _context.SaveChanges();
    }

    public void DeleteEmployee(Employee employee)
    {
        _context.Employee.Remove(employee);
        _context.SaveChanges();
    }
}
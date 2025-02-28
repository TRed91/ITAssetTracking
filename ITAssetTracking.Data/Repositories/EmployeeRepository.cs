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
        var password = _context.EmployeePasswords
            .FirstOrDefault(p => p.EmployeeID == employee.EmployeeID);
        if (password != null)
        {
            _context.EmployeePasswords.Remove(password);
            _context.Employee.Remove(employee);
            _context.SaveChanges();   
        }
    }

    public void AddEmployeePassword(EmployeePasswords employeePassword)
    {
        _context.EmployeePasswords.Add(employeePassword);
        _context.SaveChanges();
    }

    public void DeleteEmployeePassword(int employeeId)
    {
        var password = _context.EmployeePasswords
            .FirstOrDefault(p => p.EmployeeID == employeeId);
        if (password == null)
        {
            throw new Exception($"Password with EmployeeID {employeeId} not found");
        }
        _context.EmployeePasswords.Remove(password);
        _context.SaveChanges();
    }
}
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
        return _context.Employee
            .Include(e => e.Department)
            .Include(e => e.AssetAssignments)
            .Include(e => e.SoftwareAssetAssignments)
            .FirstOrDefault(e => e.EmployeeID == employeeId);
    }

    public List<Employee> GetEmployees()
    {
        return _context.Employee
            .Include(e => e.Department)
            .Include(e => e.AssetAssignments)
            .Include(e => e.SoftwareAssetAssignments)
            .ToList();
    }

    public List<Employee> GetEmployeesByDepartmentId(int departmentId)
    {
        return _context.Employee
            .Include(e => e.Department)
            .Include(e => e.AssetAssignments)
            .Include(e => e.SoftwareAssetAssignments)
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
        var employeeToUpdate = _context.Employee.FirstOrDefault(e => e.EmployeeID == employee.EmployeeID);
        if (employeeToUpdate != null)
        {
            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.DepartmentID = employee.DepartmentID;
            _context.SaveChanges();
        }
    }

    public void DeleteEmployee(int employeeId)
    {
        var employee = _context.Employee.FirstOrDefault(e => e.EmployeeID == employeeId);
        var password = _context.EmployeePasswords
            .FirstOrDefault(p => p.EmployeeID == employeeId);
        if (employee != null)
        {
            if (password != null)
            {
                _context.EmployeePasswords.Remove(password);
            }
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
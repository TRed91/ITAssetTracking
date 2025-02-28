using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockEmployeeRepo : IEmployeeRepository
{
    private readonly MockDB _db;

    public MockEmployeeRepo()
    {
        _db = new MockDB();
    }
    
    public Employee? GetEmployee(int employeeId)
    {
        return _db.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);
    }

    public List<Employee> GetEmployees()
    {
        return _db.Employees;
    }

    public List<Employee> GetEmployeesByDepartmentId(int departmentId)
    {
        return _db.Employees.Where(e => e.DepartmentID == departmentId).ToList();
    }

    public void AddEmployee(Employee employee)
    {
        employee.EmployeeID = _db.Employees.Max(e => e.EmployeeID) + 1;
        _db.Employees.Add(employee);
    }

    public void UpdateEmployee(Employee employee)
    {
        var employeeToUpdate = _db.Employees.FirstOrDefault(e => e.EmployeeID == employee.EmployeeID);
        employeeToUpdate.FirstName = employee.FirstName;
        employeeToUpdate.LastName = employee.LastName;
        employeeToUpdate.DepartmentID = employee.DepartmentID;
    }

    public void DeleteEmployee(Employee employee)
    {
        _db.Employees.Remove(employee);
    }

    public void AddEmployeePassword(EmployeePasswords employeePassword)
    {
        throw new NotImplementedException();
    }
}
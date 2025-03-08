using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Employee? GetEmployee(int employeeId);
    
    List<Employee> GetEmployees();
    List<Employee> GetEmployeesByDepartmentId(int departmentId);
    
    void AddEmployee(Employee employee);
    void UpdateEmployee(Employee employee);
    void DeleteEmployee(int employeeId);
    
    void AddEmployeePassword(EmployeePasswords employeePassword);
}
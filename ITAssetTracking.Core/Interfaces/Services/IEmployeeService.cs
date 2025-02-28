using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface IEmployeeService
{
    Result<Employee> GetEmployeeById(int employeeId);
    
    Result<List<Employee>> GetEmployees();
    Result<List<Employee>> GetEmployeesByDepartment(int departmentId);
    
    Result AddEmployee(Employee employee);
    Result UpdateEmployee(Employee employee);
    Result DeleteEmployee(int employeeId);
    Result AddEmployeePassword(EmployeePasswords employeePassword);
}
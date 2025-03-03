using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IDepartmentRepository _departmentRepo;

    public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepo = employeeRepository;
        _departmentRepo = departmentRepository;
    }
    
    public Result<Employee> GetEmployeeById(int employeeId)
    {
        try
        {
            var employee = _employeeRepo.GetEmployee(employeeId);
            if (employee == null)
            {
                return ResultFactory.Fail<Employee>("Employee not found");
            }

            return ResultFactory.Success(employee);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Employee>(ex.Message, ex);
        }
    }

    public Result<List<Employee>> GetEmployees()
    {
        try
        {
            var employees = _employeeRepo.GetEmployees();
            return ResultFactory.Success(employees);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Employee>>(ex.Message, ex);
        }
    }

    public Result<List<Employee>> GetEmployeesByDepartment(int departmentId)
    {
        try
        {
            var department = _departmentRepo.GetDepartmentById(departmentId);
            if (department == null)
            {
                return ResultFactory.Fail<List<Employee>>("Invalid department id");
            }
            var employees = _employeeRepo.GetEmployeesByDepartmentId(departmentId);
            return ResultFactory.Success(employees);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Employee>>(ex.Message, ex);
        }
    }

    public Result AddEmployee(Employee employee)
    {
        try
        {
            _employeeRepo.AddEmployee(employee);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateEmployee(Employee employee)
    {
        try
        {
            var oldEmployee = _employeeRepo.GetEmployee(employee.EmployeeID);
            if (oldEmployee == null)
            {
                return ResultFactory.Fail("Employee not found");
            }

            oldEmployee.FirstName = employee.FirstName;
            oldEmployee.LastName = employee.LastName;
            oldEmployee.DepartmentID = employee.DepartmentID;

            _employeeRepo.UpdateEmployee(oldEmployee);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteEmployee(int employeeId)
    {
        try
        {
            var employee = _employeeRepo.GetEmployee(employeeId);
            if (employee == null)
            {
                return ResultFactory.Fail("Employee not found");
            }

            _employeeRepo.DeleteEmployee(employee);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result AddEmployeePassword(EmployeePasswords employeePassword)
    {
        try
        {
            var employee = _employeeRepo.GetEmployee(employeePassword.EmployeeID);
            if (employee == null)
            {
                return ResultFactory.Fail("Employee not found");
            }

            _employeeRepo.AddEmployeePassword(employeePassword);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result<string> GeneratePassword(string lastName)
    {
        if (lastName.Length < 2)
        {
            return ResultFactory.Fail<string>("Last name must be at least 2 characters");
        }
        
        Random rng = new Random();
        string randomNumbers = "";
        if (lastName.Length == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                randomNumbers += rng.Next(1, 10).ToString();
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                randomNumbers += rng.Next(1, 10).ToString();
            }
        }
        var password = "!" + lastName + randomNumbers;
        return ResultFactory.Success(password);
    }
}
using ITAssetTracking.API.Models;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly IEmployeeService _employeeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EmployeesController(
        Serilog.ILogger logger, 
        IEmployeeService employeeService, 
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _employeeService = employeeService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Gets a list of all employees or optionally filtered by search param
    /// </summary>
    /// <param name="search">search by last name and first name</param>
    /// <returns>List of Employees</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<EmployeeModel>> GetEmployees(string? search)
    {
        var result = _employeeService.GetEmployees();
        if (!result.Ok)
        {
            _logger.Error(result.Exception, "Failed to get employees: " + result.Message);
            return StatusCode(500, result.Message);
        }

        var employees = result.Data;
        if (!string.IsNullOrEmpty(search))
        {
            employees = employees
                .Where(e => e.LastName.ToLower().Contains(search.ToLower()) || 
                            e.FirstName.ToLower().Contains(search.ToLower()))
                .ToList();
        }

        var models = employees.Select(e => new EmployeeModel
        {
            EmployeeId = e.EmployeeID,
            FirstName = e.FirstName,
            LastName = e.LastName,
            DepartmentId = e.DepartmentID,
        }).ToList();
        
        return Ok(models);
    }

    /// <summary>
    /// Gets data for a single employee
    /// </summary>
    /// <param name="id">Employee Id</param>
    /// <returns>Employee Data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<EmployeeModel> GetEmployee(int id)
    {
        var result = _employeeService.GetEmployeeById(id);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, "Failed to get employee " + result.Message);
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }

        var model = new EmployeeModel
        {
            EmployeeId = result.Data.EmployeeID,
            FirstName = result.Data.FirstName,
            LastName = result.Data.LastName,
            DepartmentId = result.Data.DepartmentID,
        };
        return Ok(model);
    }

    /// <summary>
    /// Create a new Employee, Password and Identity record
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> CreateEmployee(EmployeeForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var role = await _roleManager.FindByNameAsync(form.Role ?? "Employee");
        if (role == null)
        {
            return BadRequest("Invalid role name");
        }
        var employee = form.ToEntity();
        
        // generate Password
        var genPasswordResult = _employeeService.GeneratePassword(employee.LastName);
        if (!genPasswordResult.Ok)
        {
            _logger.Error(genPasswordResult.Exception, "Failed to generate password " + genPasswordResult.Message);
            return StatusCode(500, genPasswordResult.Message);
        }
        
        // add employee to database
        var addEmployeeResult = _employeeService.AddEmployee(employee);
        if (!addEmployeeResult.Ok)
        {
            _logger.Error(addEmployeeResult.Exception, "Failed to add employee " + addEmployeeResult.Message);
            return StatusCode(500, addEmployeeResult.Message);
        }

        // add employee password to database
        var password = new EmployeePasswords
        {
            EmployeeID = employee.EmployeeID,
            Password = genPasswordResult.Data,
        };
        var addPasswordResult = _employeeService.AddEmployeePassword(password);
        if (!addPasswordResult.Ok)
        {
            _logger.Error(addPasswordResult.Exception, "Failed to add employee password " + addPasswordResult.Message);
            _employeeService.DeleteEmployee(employee.EmployeeID);
            return StatusCode(500, addPasswordResult.Message);
        }

        // add identity user record
        var user = new ApplicationUser
        {
            EmployeeID = employee.EmployeeID,
            UserName = employee.LastName.ToLower() + employee.FirstName.ToLower(),
        };
        var addUserResult = await _userManager.CreateAsync(user, password.Password);
        if (!addUserResult.Succeeded)
        {
            var errMsg = string.Join(",", addUserResult.Errors.Select(e => e.Description));
            _logger.Error("Failed to add identity user: " + errMsg);
            foreach (var e in addUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, e.Description);
            }
            _employeeService.DeleteEmployee(employee.EmployeeID);
            return StatusCode(500, ModelState);
        }

        // add user role
        var addRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
        if (!addRoleResult.Succeeded)
        {
            var errMsg = string.Join(",", addRoleResult.Errors.Select(e => e.Description));
            _logger.Error("Failed to add role: " + errMsg);
            foreach (var e in addUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, e.Description);
            }

            await _userManager.DeleteAsync(user);
            _employeeService.DeleteEmployee(employee.EmployeeID);
            return StatusCode(500, ModelState);
        }

        return Created();
    }

    /// <summary>
    /// Updates an Employee Record
    /// </summary>
    /// <param name="id">Employee Id</param>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateEmployee(int id, EmployeeForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var employee = form.ToEntity();
        employee.EmployeeID = id;
        
        var updateEmployeeResult = _employeeService.UpdateEmployee(employee);
        if (!updateEmployeeResult.Ok)
        {
            if (updateEmployeeResult.Exception != null)
            {
                _logger.Error(updateEmployeeResult.Exception, "Failed to update employee " + employee.LastName);
                return StatusCode(500, updateEmployeeResult.Message);
            }
            return NotFound(updateEmployeeResult.Message);
        }
        return NoContent();
    }

    [HttpPost("{id}/roles/{roleName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddEmployeeRole(int id, string roleName)
    {
        var user = await _userManager.FindByEmployeeIdAsync(id);
        if (user == null)
        {
            return NotFound("User does not exist");
        }
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return BadRequest("Role does not exist");
        }
        var result = await _userManager.AddToRoleAsync(user, role.Name);
        if (!result.Succeeded)
        {
            var errMsg = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.Error("Failed to add role: " + errMsg);
            foreach (var e in result.Errors)
            {
                ModelState.AddModelError(string.Empty, e.Description);
            }
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }

    [HttpDelete("{id}/roles/{roleName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoveEmployeeRole(int id, string roleName)
    {
        var user = await _userManager.FindByEmployeeIdAsync(id);
        if (user == null)
        {
            return NotFound("User does not exist");
        }
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return BadRequest("Role does not exist");
        }
        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
        if (!result.Succeeded)
        {
            var errMsg = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.Error("Failed to add role: " + errMsg);
            foreach (var e in result.Errors)
            {
                ModelState.AddModelError(string.Empty, e.Description);
            }
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
}
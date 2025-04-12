using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;
using ITAssetTracking.Data;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Result = ITAssetTracking.Core.Utility.Result;

namespace ITAssetTracking.MVC.Controllers;

public class EmployeeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmployeeService  _employeeService;
    private readonly IDepartmentService  _departmentService;
    private readonly Serilog.ILogger _logger;

    public EmployeeController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        Serilog.ILogger logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Register()
    {
        var departmentsResult = _departmentService.GetDepartments();
        if (!departmentsResult.Ok)
        {
            _logger.Error(departmentsResult.Exception, departmentsResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , departmentsResult.Message));
            return RedirectToAction("Index", "Home");
        }
        var model = new NewEmployeeModel();
        model.DepartmentList = new SelectList(departmentsResult.Data,  "DepartmentID", "DepartmentName");
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(NewEmployeeModel model)
    {
        var departments = _departmentService.GetDepartments().Data;
        model.DepartmentList = new SelectList(departments, "DepartmentID", "DepartmentName");
        if (!ModelState.IsValid)
        {
            _logger.Warning("Invalid Model State while registering user");
            return View(model);
        }
        var employee = model.ToEntity();
        // generate password 
        var passwordResult = _employeeService.GeneratePassword(employee.LastName);
        if (!passwordResult.Ok)
        {
            _logger.Error(passwordResult.Exception, passwordResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, passwordResult.Message));
            return View(model);
        }
        string password = passwordResult.Data;
        // add employee to db
        var addEmployeeResult = _employeeService.AddEmployee(employee);
        if (!addEmployeeResult.Ok)
        {
            _logger.Error(addEmployeeResult.Exception, "Error adding user: " + addEmployeeResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , addEmployeeResult.Message));
            return View(model);
        }
        // add password to db
        var employeePassword = new EmployeePasswords
        {
            EmployeeID = employee.EmployeeID,
            Password = password
        };
        var addPasswordResult = _employeeService.AddEmployeePassword(employeePassword);
        if (!addPasswordResult.Ok)
        {
            _logger.Error(addPasswordResult.Exception, "Error adding password: " + addPasswordResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , addPasswordResult.Message));
            _employeeService.DeleteEmployee(employee.EmployeeID);
            return View(model);
        }
        // create application user
        var user = new ApplicationUser
        {
            EmployeeID = employee.EmployeeID,
            UserName = employee.LastName.ToLower() + employee.FirstName.ToLower(),
        };
        var addUserResult =  await _userManager.CreateAsync(user, password);
        if (!addUserResult.Succeeded)
        {
            foreach (var error in addUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            _employeeService.DeleteEmployee(employee.EmployeeID);
            return View(model);
        }
        
        _logger.Information($"New User created with EmployeeID: {employee.EmployeeID}.");
        TempData["msg"] = TempDataExtension
            .Serialize(new TempDataMsg(true, $"User created with id {employee.EmployeeID}"));
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin, SoftwareLicenseManager, AssetManager, DepartmentManager, HelpDescTechnician, Auditor")]
    public IActionResult Index(SelectEmployeeModel model)
    {
        Result<List<Employee>> employeesResult;
        List<Employee> employees;
        if (model.DepartmentId != null)
        {
            employeesResult = _employeeService.GetEmployeesByDepartment((int)model.DepartmentId);
        }
        else
        {
            employeesResult = _employeeService.GetEmployees();
        }
        if (!employeesResult.Ok)
        {
            _logger.Error(employeesResult.Exception, "Error retrieving employees list: " + employeesResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , employeesResult.Message));
            return RedirectToAction("Index", "Home");
        }
        employees = employeesResult.Data;
        var departments = _departmentService.GetDepartments().Data;
        
        if (model.StartsWith != null)
        {
            employees = employees
                .Where(e => e.LastName[0] == model.StartsWith)
                .ToList();
        }
        if (model.Search != null)
        {
            employees = employees
                .Where(e => e.LastName.ToLower().Contains(model.Search.ToLower()) ||
                            e.FirstName.ToLower().Contains(model.Search.ToLower()))
                .ToList();
        }
        model.Employees = employees;
        model.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
        
        return View(model);
    }
}
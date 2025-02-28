using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class EmployeeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmployeeService  _employeeService;
    private readonly IDepartmentService  _departmentService;
    private readonly ILogger _logger;

    public EmployeeController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,  
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        ILogger<EmployeeController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        var departmentsResult = _departmentService.GetDepartments();
        if (!departmentsResult.Ok)
        {
            _logger.LogError(departmentsResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , departmentsResult.Message));
            return RedirectToAction("Index", "Home");
        }
        var model = new NewEmployeeModel();
        model.DepartmentList = new SelectList(departmentsResult.Data,  "DepartmentID", "DepartmentName");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(NewEmployeeModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid Model State while registering user");
            return View(model);
        }
        var employee = model.ToEntity();
        
        // add employee to db
        var addEmployeeResult = _employeeService.AddEmployee(employee);
        if (!addEmployeeResult.Ok)
        {
            _logger.LogError(addEmployeeResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , addEmployeeResult.Message));
            return View(model);
        }
        // generate password
        Random rng = new Random();
        string randomNumbers = "";
        if (employee.LastName.Length == 2)
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
        var password = "!" + employee.LastName + randomNumbers;
        
        // add password to db
        var employeePassword = new EmployeePasswords
        {
            EmployeeID = employee.EmployeeID,
            Password = password
        };
        var addPasswordResult = _employeeService.AddEmployeePassword(employeePassword);
        if (!addPasswordResult.Ok)
        {
            _logger.LogError(addPasswordResult.Message);
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
        await _signInManager.SignInAsync(user, false);
        
        _logger.LogInformation($"New User created with EmployeeID: {employee.EmployeeID}.");
        TempData["msg"] = TempDataExtension
            .Serialize(new TempDataMsg(true, $"User created with id {employee.EmployeeID}"));
        return RedirectToAction("Index", "Home");
    }
}
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

[Authorize(Roles = "Admin, AssetManager")]
public class AssetAssignmentController : Controller
{
    private readonly IAssetAssignmentService _assignmentService;
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly IAssetService _assetService;
    private readonly Serilog.ILogger _logger;

    public AssetAssignmentController(IAssetAssignmentService assetAssignmentService, 
        IEmployeeService employeeService, 
        IDepartmentService departmentService,
        IAssetService assetService,
        Serilog.ILogger logger)
    {
        _assignmentService = assetAssignmentService;
        _employeeService = employeeService;
        _departmentService = departmentService;
        _assetService = assetService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult AssignEmployee(long assetId, SelectEmployeeAssignmentModel model)
    {
        Result<List<Employee>> employeesResult;
        List<Employee> employees;
        if (model.DepartmentId.HasValue && model.DepartmentId > 0)
        {
            employeesResult = _employeeService.GetEmployeesByDepartment((int)model.DepartmentId);
        }
        else
        {
            employeesResult = _employeeService.GetEmployees();
        }
        if (!employeesResult.Ok)
        {
            _logger.Error("Error retrieving employees list: " + employeesResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , employeesResult.Message));
            return RedirectToAction("Index", "Home");
        }
        employees = employeesResult.Data;
        var departments = _departmentService.GetDepartments().Data;
        
        if (model.StartsWith.HasValue && char.IsLetter((char)model.StartsWith))
        {
            employees = employees
                .Where(e => e.LastName[0] == model.StartsWith)
                .ToList();
        }
        if (!string.IsNullOrEmpty(model.Search))
        {
            employees = employees
                .Where(e => e.LastName.ToLower().Contains(model.Search.ToLower()) ||
                            e.FirstName.ToLower().Contains(model.Search.ToLower()))
                .ToList();
        }
        var assetResult = _assetService.GetAssetById(assetId);
        if (!assetResult.Ok)
        {
            _logger.Error("Error retrieving asset: " + assetResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false , assetResult.Message));
            return RedirectToAction("Index", "Home");
        }
        model.AssetID = assetId;
        model.SerialNumber = assetResult.Data.SerialNumber;
        model.Employees = employees;
        model.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AssignEmployee(SelectEmployeeAssignmentModel model)
    {
        var employeeResult = _employeeService.GetEmployeeById(model.EmployeeID);
        if (!employeeResult.Ok)
        {
            _logger.Error("Error retrieving employee record: " + employeeResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false , employeeResult.Message));
            return RedirectToAction("Index", "Home");
        }
        var assetAssignment = new AssetAssignment
        {
            AssetID = model.AssetID,
            EmployeeID = model.EmployeeID,
            DepartmentID = employeeResult.Data.DepartmentID,
        };
        var assignmentResult = _assignmentService.AddAssetAssignment(assetAssignment);
        if (!assignmentResult.Ok)
        {
            _logger.Error($"Error assigning asset: {assignmentResult.Message} => {assignmentResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false , assignmentResult.Message));
            return RedirectToAction("AssignEmployee",  new { assetId = model.AssetID });
        }
        var msg = $"Assigned Asset with ID {model.AssetID} to Employee with ID {model.EmployeeID}";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true , msg));
        return RedirectToAction("Details", "Asset", new { assetId = model.AssetID });
    }

    [HttpGet]
    public IActionResult AssignDepartment(long assetId)
    {
        var departmentsResult = _departmentService.GetDepartments();
        var assetResult = _assetService.GetAssetById(assetId);
        if (!departmentsResult.Ok || !assetResult.Ok)
        {
            _logger.Error("Error retrieving departments: " + (departmentsResult.Exception ?? assetResult.Exception));
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, (departmentsResult.Message ?? departmentsResult.Message)));
            return RedirectToAction("Details", "Asset", new { assetId });
        }

        var model = new AssignDepartmentModel
        {
            AssetID = assetId,
            SerialNumber = assetResult.Data.SerialNumber,
            Departments = departmentsResult.Data
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AssignDepartment(AssignDepartmentModel model)
    {
        var assignment = new AssetAssignment
        {
            AssetID = model.AssetID,
            DepartmentID = (byte)model.DepartmentID,
        };
        var assignResult = _assignmentService.AddAssetAssignment(assignment);
        if (!assignResult.Ok)
        {
            _logger.Error("Error assigning asset: " + assignResult.Exception);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, assignResult.Message));
            return RedirectToAction("AssignDepartment", new { assetId = model.AssetID });
        }
        var msg = $"Asset with ID {model.AssetID} assigned to Department with ID {model.DepartmentID}";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true , msg));
        return RedirectToAction("Details", "Asset", new { assetId = model.AssetID });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Return(long assetId)
    {
        var returnResult = _assignmentService.Return(assetId);
        if (!returnResult.Ok)
        {
            _logger.Error($"Error returning asset: {returnResult.Message} => {returnResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, returnResult.Message));
            return RedirectToAction("Details", "Asset", new { assetId = assetId });
        }
        var msg = $"Returned Asset with ID {assetId}";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true , msg));
        return RedirectToAction("Details", "Asset", new { assetId = assetId });
    }
}
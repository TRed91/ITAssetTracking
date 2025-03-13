using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.MVC.Controllers;

public class AssetRequestController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IEmployeeService _employeeService;
    private readonly IAssetRequestService _assetRequestService;
    private readonly Serilog.ILogger _logger;

    public AssetRequestController(
        IDepartmentService departmentService,
        IEmployeeService employeeService,
        IAssetRequestService assetRequestService,
        Serilog.ILogger logger)
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
        _assetRequestService = assetRequestService;
        _logger = logger;
    }

    public IActionResult AvailableAssets(int departmentId, AvailableAssetsModel model)
    {
        return View();
    }
}
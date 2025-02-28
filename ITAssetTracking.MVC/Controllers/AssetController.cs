using ITAssetTracking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.MVC.Controllers;

public class AssetController : Controller
{
    private readonly IAssetService _assetService;
    private readonly IAssetAssignmentService _assetAssignmentService;
    private readonly ILogger _logger;

    public AssetController(
        IAssetService assetService, 
        IAssetAssignmentService assetAssignmentService,
        ILogger<AssetController> logger)
    {
        _assetService = assetService;
        _assetAssignmentService = assetAssignmentService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
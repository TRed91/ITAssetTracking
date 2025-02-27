using ITAssetTracking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.MVC.Controllers;

public class AssetController : Controller
{
    private readonly IAssetService _assetService;
    private readonly ILogger _logger;

    public AssetController(IAssetService assetService, ILogger<AssetController> logger)
    {
        _assetService = assetService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
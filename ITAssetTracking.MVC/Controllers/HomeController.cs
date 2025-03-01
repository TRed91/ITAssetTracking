using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ITAssetTracking.MVC.Models;

namespace ITAssetTracking.MVC.Controllers;

public class HomeController : Controller
{
    private readonly Serilog.ILogger _logger;

    public HomeController(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
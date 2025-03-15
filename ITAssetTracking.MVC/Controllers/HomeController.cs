using System.Diagnostics;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Mvc;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ITAssetTracking.MVC.Controllers;

public class HomeController : Controller
{
    private readonly Serilog.ILogger _logger;
    
    public HomeController(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}
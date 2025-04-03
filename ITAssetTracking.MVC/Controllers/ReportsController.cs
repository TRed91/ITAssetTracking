using ITAssetTracking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.MVC.Controllers;

public class ReportsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
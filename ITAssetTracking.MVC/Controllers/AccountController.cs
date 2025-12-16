using ITAssetTracking.Data;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.MVC.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly Serilog.ILogger _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        Serilog.ILogger logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        var model = new LoginModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == model.UserName);
            if (user == null)
            {
                var errMsg = new TempDataMsg(false, "Username incorrect.");
                TempData["msg"] = TempDataExtension.Serialize(errMsg);
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                var errMsg = new TempDataMsg(false, "Password incorrect.");
                TempData["msg"] = TempDataExtension.Serialize(errMsg);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Login failed");
            ModelState.AddModelError(string.Empty, "Login failed.");
            var errMsg = new TempDataMsg(false, "Internal error.");
            TempData["msg"] = TempDataExtension.Serialize(errMsg);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
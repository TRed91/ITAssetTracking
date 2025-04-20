using ITAssetTracking.API.Models;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly Serilog.ILogger _logger;

    public AuthenticationController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        Serilog.ILogger logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpPost("/login")]
    public async Task<ActionResult> Login(LoginForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _signInManager.PasswordSignInAsync(form.Username, form.Password, false, false);
        if (!result.Succeeded)
        {
            _logger.Warning("Invalid login attempt");
            return BadRequest(ModelState);
        }
        return Ok();
    }
}
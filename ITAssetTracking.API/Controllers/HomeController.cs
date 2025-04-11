using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly Serilog.ILogger _logger;

    public HomeController(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    [HttpGet("Index")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> Index()
    {
        return Ok("Welcome to IT Asset Tracking API!");
    }
}
using ITAssetTracking.API.Models;
using ITAssetTracking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("requests")]
public class AssetRequestsController : ControllerBase
{
    private readonly IAssetRequestService _assetRequestService;
    private readonly ISoftwareRequestService _softwareRequestService;
    private readonly Serilog.ILogger _logger;

    public AssetRequestsController(IAssetRequestService assetRequestService, ISoftwareRequestService softwareRequestService, Serilog.ILogger logger)
    {
        _assetRequestService = assetRequestService;
        _softwareRequestService = softwareRequestService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves the history of all asset requests
    /// </summary>
    /// <returns>List of Asset Requests</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, AssetManager, Auditor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<RequestsModel> GetRequests()
    {
        var assetResults = _assetRequestService.GetAllAssetRequests();
        var softwareResults = _softwareRequestService.GetSoftwareRequests();
        if (!assetResults.Ok)
        {
            _logger.Error(assetResults.Exception, $"Failed to get requests data: {assetResults.Message}");
            return StatusCode(500, assetResults.Message);
        }
        if (!softwareResults.Ok)
        {
            _logger.Error(softwareResults.Exception, $"Failed to get requests data: {softwareResults.Message}");
            return StatusCode(500, softwareResults.Message);
        }
        
        var assetRequests = assetResults.Data
            .Select(r => new AssetRequestModel(r))
            .ToList();
        var softwareRequest = softwareResults.Data
            .Select(r => new SoftwareRequestModel(r))
            .ToList();

        var model = new RequestsModel
        {
            AssetRequests = assetRequests,
            SoftwareRequests = softwareRequest
        };
        
        return Ok(model);
    }

    /// <summary>
    /// Retrieve a list of open asset requests
    /// </summary>
    /// <returns>List of Asset Requests</returns>
    [HttpGet("open")]
    [Authorize(Roles = "Admin, AssetManager, Auditor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<RequestsModel> GetOpenRequests()
    {
        var assetResults = _assetRequestService.GetOpenAssetRequests();
        var softwareResults = _softwareRequestService.GetOpenSoftwareRequests();
        if (!assetResults.Ok)
        {
            _logger.Error(assetResults.Exception, $"Failed to get requests data: {assetResults.Message}");
            return StatusCode(500, assetResults.Message);
        }
        if (!softwareResults.Ok)
        {
            _logger.Error(softwareResults.Exception, $"Failed to get requests data: {softwareResults.Message}");
            return StatusCode(500, softwareResults.Message);
        }
        
        var assetRequests = assetResults.Data
            .Select(r => new AssetRequestModel(r))
            .ToList();
        var softwareRequest = softwareResults.Data
            .Select(r => new SoftwareRequestModel(r))
            .ToList();

        var model = new RequestsModel
        {
            AssetRequests = assetRequests,
            SoftwareRequests = softwareRequest
        };
        
        return Ok(model);
    }

    [HttpPost("assets")]
    [Authorize(Roles = "Admin, DepartmentManager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult CreateAssetRequest(AssetRequestForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = form.ToAssetRequest();
        var result = _assetRequestService.AddAssetRequest(request);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to add asset request: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }

        return Created();
    }
    
    [HttpPost("licences")]
    [Authorize(Roles = "Admin, DepartmentManager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult CreateSoftwareRequest(SoftwareRequestForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = form.ToSoftwareAssetRequest();
        var result = _softwareRequestService.AddSoftwareRequest(request);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to add asset request: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }

        return Created();
    }

    [HttpPut("assets/{assetRequestId}")]
    [Authorize(Roles = "Admin, AssetManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult UpdateAssetRequest(int assetRequestId, AssetRequestForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var request = form.ToAssetRequest();
        request.AssetRequestID = assetRequestId;
        
        var result = _assetRequestService.UpdateAssetRequest(request);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to update asset request: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }

        return NoContent();
    }
    
    [HttpPut("licences/{assetRequestId}")]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult UpdateSoftwareRequest(int assetRequestId, SoftwareRequestForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var request = form.ToSoftwareAssetRequest();
        request.SoftwareAssetRequestID = assetRequestId;
        
        var result = _softwareRequestService.UpdateSoftwareRequest(request);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to update asset request: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }

        return NoContent();
    }
}
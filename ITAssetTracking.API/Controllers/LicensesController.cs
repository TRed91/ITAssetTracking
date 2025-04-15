using ITAssetTracking.API.Models;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LicensesController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly ISoftwareAssetService _service;
    private readonly ISoftwareAssetAssignmentService _assignmentService;

    public LicensesController(Serilog.ILogger logger, ISoftwareAssetService service, ISoftwareAssetAssignmentService assignmentService)
    {
        _logger = logger;
        _service = service;
        _assignmentService = assignmentService;
    }

    /// <summary>
    /// Returns all software assets / licenses filtered by the given parameters
    /// </summary>
    /// <param name="licenseTypeId"></param>
    /// <param name="manufacturerId"></param>
    /// <param name="assetStatusId"></param>
    /// <param name="includeExpired"></param>
    /// <returns>List of software assets / licenses</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<SoftwareAssetModel>> GetLicenses(
        int? licenseTypeId,
        int? manufacturerId,
        int? assetStatusId,
        bool? includeExpired)
    {
        var licenseType = licenseTypeId ?? 0;
        var manufacturer = manufacturerId ?? 0;
        var assetStatus = assetStatusId ?? 0;
        var expired = includeExpired ?? false;
        
        var assetsResult = _service.GetSoftwareAssets(licenseType, assetStatus, manufacturer, expired);
        if (!assetsResult.Ok)
        {
            _logger.Error(assetsResult.Exception, $"Failed to get software assets: {assetsResult.Message}");
            return StatusCode(500, assetsResult.Message);
        }
        
        var assets = assetsResult.Data
            .Select(a => new SoftwareAssetModel(a))
            .ToList();
        
        return Ok(assets);
    }

    /// <summary>
    /// Returns all software assets / licenses currently assigned to a given Employee Id
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>List of software assets / licenses</returns>
    [HttpGet("employee/{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<SoftwareAssetModel>> GetLicensesByEmployee(int employeeId)
    {
        var assetsResult = _service.GetSoftwareAssetsByEmployeeId(employeeId);
        if (!assetsResult.Ok)
        {
            if (assetsResult.Exception != null)
            {
                _logger.Error(assetsResult.Exception, $"Failed to get software assets: {assetsResult.Message}");
                return StatusCode(500, assetsResult.Message);
            }
            return NotFound(assetsResult.Message);
        }
        
        var assets = assetsResult.Data
            .Select(a => new SoftwareAssetModel(a))
            .ToList();
        
        return Ok(assets);
    }

    /// <summary>
    /// Returns a single software asset with the given assetId
    /// </summary>
    /// <param name="assetId">Software Asset ID</param>
    /// <returns>Software Asset / License</returns>
    [HttpGet("{assetId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SoftwareAssetModel> GetLicense(int assetId)
    {
        var assetResult = _service.GetSoftwareAsset(assetId);
        if (!assetResult.Ok)
        {
            if (assetResult.Exception != null)
            {
                _logger.Error(assetResult.Exception, $"Failed to get software asset: {assetResult.Message}");
                return StatusCode(500, assetResult.Message);
            }
            return NotFound(assetResult.Message);
        }
        var asset = new SoftwareAssetModel(assetResult.Data);
        return Ok(asset);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult AddLicense(SoftwareAssetForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var asset = form.ToEntity();
        var result = _service.AddSoftwareAsset(asset);
        
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to add software asset: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return BadRequest(result.Message);
        }

        _logger.Information($"Added software asset with id {asset.SoftwareAssetID}");
        return Created();
    }

    [HttpPut("{assetId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateLicense(int assetId, SoftwareAssetForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var asset = form.ToEntity();
        asset.SoftwareAssetID = assetId;
        var result = _service.UpdateSoftwareAsset(asset);
        
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to update software asset: {result.Message}");
                return StatusCode(500, result.Message);
            }
            if (result.Message == "Software asset not found")
            {
                return NotFound(result.Message);
            }
            return BadRequest(result.Message);
        }
        
        _logger.Information($"Software asset with id {assetId} updated");
        return NoContent();
    }

    [HttpDelete("{assetId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteLicense(int assetId)
    {
        var result = _service.DeleteSoftwareAsset(assetId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to delete software asset: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        
        _logger.Warning($"Software asset with id {assetId} deleted");
        return NoContent();
    }

    /// <summary>
    /// Get a list of all assignment for a given software asset
    /// </summary>
    /// <param name="softwareAssetId"></param>
    /// <returns>List of Software Assignments</returns>
    [HttpGet("{softwareAssetId}/assignments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<SoftwareAssignmentModel>> GetAssignmentsForLicenses(int softwareAssetId)
    {
        var result = _assignmentService.GetAssignmentsBySoftwareAssetId(softwareAssetId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to get software asset: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        
        var assignments = result.Data
            .Select(a => new SoftwareAssignmentModel(a))
            .ToList();
        
        return Ok(assignments);
    }

    [HttpPost("assignments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult AddAssignment(SoftwareAssignmentForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var assignment = form.ToSoftwareAssetAssignment();
        assignment.ReturnDate = null;
        
        var result = _assignmentService.AddSoftwareAssetAssignment(assignment);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Failed to add software asset assignment: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }

        return Created();
    }
    
    [HttpPut("assignments/{assetAssignmentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateAssetAssignment(int assetAssignmentId, SoftwareAssignmentForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var assignment = form.ToSoftwareAssetAssignment();
        assignment.AssetAssignmentID = assetAssignmentId;
        
        var result = _assignmentService.UpdateSoftwareAssetAssignment(assignment);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error updating asset assignment: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }
        return NoContent();
    }
}
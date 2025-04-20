using ITAssetTracking.API.Models;
using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AssetsController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly IAssetService _assetService;
    private readonly IAssetAssignmentService _assetAssignmentService;
    private readonly IEmployeeService _employeeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AssetsController(
        Serilog.ILogger logger, 
        IAssetService assetService, 
        IAssetAssignmentService assetAssignmentService,
        UserManager<ApplicationUser> userManager,
        IEmployeeService employeeService)
    {
        _logger = logger;
        _assetService = assetService;
        _assetAssignmentService = assetAssignmentService;
        _employeeService = employeeService;
        _userManager = userManager;
    }

    /// <summary>
    /// Returns a list of all assets or filtered by query parameters
    /// </summary>
    /// <param name="assetTypeId"></param>
    /// <param name="locationId"></param>
    /// <param name="assetStatusId"></param>
    /// <param name="manufacturerId"></param>
    /// <returns>List of Assets</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, AssetManager, Auditor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<AssetModel>> GetAssets(
        int? assetTypeId, 
        int? locationId, 
        int? assetStatusId, 
        int? manufacturerId)
    {
        int atId = assetTypeId ?? 0;
        int locId = locationId ?? 0;
        int statusId = assetStatusId ?? 0;
        int mId = manufacturerId ?? 0;
        var assetsResult = _assetService.GetAllAssets(atId, locId, statusId, mId);

        if (!assetsResult.Ok)
        {
            _logger.Error(assetsResult.Exception, $"Error retrieving assets: {assetsResult.Message}");
            return StatusCode(500, $"There was an error retrieving the assets: {assetsResult.Message}");
        }
        
        var assets = assetsResult.Data
            .Select(a => new AssetModel(a))
            .ToList();
        
        return Ok(assets);
    }

    /// <summary>
    /// Get Assets assigned to a given department
    /// </summary>
    /// <param name="id">Department ID</param>
    /// <returns>List of Assets</returns>
    [HttpGet("department/{id}")]
    [Authorize(Roles = "Admin, AssetManager, Auditor, DepartmentManager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<AssetModel>>> GetDepartmentAssets(int id)
    {
        // ensure Department Managers can only view assets for their assigned Department
        var user  = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        
        if (!roles.Any(r => r == "Admin" || r == "AssetManager" || r == "Auditor"))
        {
            var employeeResult = _employeeService.GetEmployeeById(user.EmployeeID);
            if (!employeeResult.Ok || employeeResult.Data.DepartmentID != id)
            {
                return Unauthorized();
            }
        }
        
        var assetsResult = _assetService.GetDepartmentAssets(id);
        if (!assetsResult.Ok)
        {
            _logger.Error(assetsResult.Exception ,$"Error retrieving assets: {assetsResult.Message}");
            return StatusCode(500, $"There was an error retrieving assets: {assetsResult.Message}");
        }
        
        var assets = assetsResult.Data
            .Select(a => new AssetModel(a))
            .ToList();
        
        return Ok(assets);
    }

    /// <summary>
    /// Get assets assigned of a given Employee
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>List of Assets</returns>
    [HttpGet("employee/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<AssetModel>>> GetEmployeeAssets(int id)
    {
        // ensure everyone except for Admins, Asset Managers and Auditors can only view their own assets    
        var user  = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Any(r => r == "Admin" || r == "Auditor" || r == "AssetManager"))
        {
            var employeeResult = _employeeService.GetEmployeeById(user.EmployeeID);
            if (!employeeResult.Ok || employeeResult.Data.EmployeeID != id)
            {
                return Unauthorized();
            }
        }
        var assetsResult = _assetService.GetEmployeeAssets(id);

        if (!assetsResult.Ok)
        {
            _logger.Error(assetsResult.Exception, $"Error retrieving assets: {assetsResult.Message}");
            return StatusCode(500, assetsResult.Message);
        }
        
        var assets = assetsResult.Data
            .Select(a => new AssetModel(a))
            .ToList();
        
        return Ok(assets);
    }

    /// <summary>
    /// Retrieves a single asset for the provided asset ID
    /// </summary>
    /// <param name="assetId"></param>
    /// <returns>Asset</returns>
    [HttpGet("{assetId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetModel>> GetAsset(int assetId)
    {
        var assetResult = _assetService.GetAssetById(assetId);
        if (!assetResult.Ok)
        {
            if (assetResult.Exception != null)
            {
                _logger.Error(assetResult.Exception, $"Error retrieving asset: {assetResult.Message}");
                return StatusCode(500, assetResult.Message);
            }
            return StatusCode(404, assetResult.Message);
        }
        
        // ensure Department Managers can only get assets assigned to their department
        // and everyone else can only get assets assigned to them
        var user  = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Any(r => r == "Admin" || r == "AssetManager" || r == "Auditor"))
        {
            var employeeResult = _employeeService.GetEmployeeById(user.EmployeeID);
            var assignments = assetResult.Data.AssetAssignments;

            if (roles.Contains("DepartmentManager"))
            {
                if (!assignments.Any(a 
                        => a.DepartmentID == employeeResult.Data.DepartmentID && a.ReturnDate == null))
                {
                    return Unauthorized();
                }
            }
            else
            {
                if (!assignments.Any(a =>
                        a.EmployeeID == employeeResult.Data.EmployeeID && a.ReturnDate == null))
                {
                    return Unauthorized();
                }
            }
        }
        
        var asset = new AssetModel(assetResult.Data);
        return Ok(asset);
    }

    /// <summary>
    /// Returns a list of currently available assets
    /// </summary>
    /// <returns></returns>
    [HttpGet("available")]
    [Authorize(Roles = "Admin, AssetManager, Auditor, DepartmentManager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<AssetModel>> GetAvailableAssets()
    {
        var assetsResult = _assetService.GetAvailableAssets();
        if (!assetsResult.Ok)
        {
            _logger.Error(assetsResult.Exception, $"Error retrieving assets: {assetsResult.Message}");
            return StatusCode(500, assetsResult.Message);
        }
        var assets = assetsResult.Data
            .Select(a => new AssetModel(a))
            .ToList();
        
        return Ok(assets);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, AssetManager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult AddAsset(AssetForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var asset = form.ToEntity();
        var result = _assetService.AddAsset(asset);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error adding asset: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }

        _logger.Information($"Asset added with id {asset.AssetID}");
        return Created();
    }

    [HttpPut("{assetId}")]
    [Authorize(Roles = "Admin, AssetManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateAsset(int assetId, AssetForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var asset = form.ToEntity();
        asset.AssetID = assetId;
        
        var result = _assetService.UpdateAsset(asset);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error updating asset: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        
        _logger.Information($"Updated asset with id {assetId}");
        return NoContent();
    }

    [HttpDelete("{assetId}")]
    [Authorize(Roles = "Admin, AssetManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteAsset(int assetId)
    {
        var result = _assetService.DeleteAsset(assetId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error deleting asset: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        
        _logger.Warning($"Deleted asset with id {assetId}");
        return NoContent();
    }

    /// <summary>
    /// Gets all assignments for a given Asset
    /// </summary>
    /// <param name="assetId"></param>
    /// <returns>List of Asset Assignments</returns>
    [HttpGet("{assetId}/assignments")]
    [Authorize(Roles = "Admin, AssetManager, Auditor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<AssetAssignmentModel>> GetAssignmentsForAsset(long assetId)
    {
        var result = _assetAssignmentService.GetAssetAssignmentsByAsset(assetId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error retrieving asset assignments: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        
        var assignments = result.Data
            .Select(a => new AssetAssignmentModel(a))
            .ToList();
        
        return Ok(assignments);
    }
    
    [HttpPost("assignments")]
    [Authorize(Roles = "Admin, AssetManager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult AddAssetAssignment(AssetAssignmentForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var assignment = form.ToAssetAssignment();
        assignment.ReturnDate = null;
        
        var result = _assetAssignmentService.AddAssetAssignment(assignment);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error adding asset assignment: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }
        
        return Created();
    }

    [HttpPut("assignments/{assetAssignmentId}")]
    [Authorize(Roles = "Admin, AssetManager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateAssetAssignment(int assetAssignmentId, AssetAssignmentForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var assignment = form.ToAssetAssignment();
        assignment.AssetAssignmentID = assetAssignmentId;
        
        var result = _assetAssignmentService.UpdateAssetAssignment(assignment);
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
using ITAssetTracking.API.Models;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Authorize]
public class ListsController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly IDepartmentService _departmentService;
    private readonly IAssetService _assetService;
    private readonly ISoftwareAssetService _softwareAssetService;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ListsController(
        Serilog.ILogger logger, 
        IDepartmentService departmentService, 
        IAssetService assetService, 
        ISoftwareAssetService softwareAssetService, 
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _departmentService = departmentService;
        _assetService = assetService;
        _softwareAssetService = softwareAssetService;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Get a list of all departments with their id
    /// </summary>
    /// <returns>List of Departments</returns>
    [HttpGet("/departments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetDepartments()
    {
        var departments = _departmentService.GetDepartments();
        if (!departments.Ok)
        {
            _logger.Error(departments.Exception, "Failed to get departments: " + departments.Message);
            return StatusCode(500, departments.Message);
        }

        var list = departments.Data
            .Select(d => new DepartmentModel
            {
                DepartmentId = d.DepartmentID,
                DepartmentName = d.DepartmentName,
            })
            .ToList();
        
        return Ok(list);
    }
    
    /// <summary>
    /// Gets a list of all locations with their id
    /// </summary>
    /// <returns>List of Locations</returns>
    [HttpGet("/locations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetLocations()
    {
        var result = _assetService.GetLocations();
        if (!result.Ok)
        {
            _logger.Error(result.Exception, "Failed to get locations: " + result.Message);
            return StatusCode(500, result.Message);
        }

        var locations = result.Data
            .Select(l => new LocationsModel
            {
                LocationId = l.LocationID,
                LocationName = l.LocationName,
            })
            .ToList();
        
        return Ok(locations);
    }
    
    /// <summary>
    /// Gets a list of all asset types with their id
    /// </summary>
    /// <returns>List of Asset Types</returns>
    [HttpGet("/assetTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetAssetTypes()
    {
        var result = _assetService.GetAssetTypes();
        if (!result.Ok)
        {
            _logger.Error(result.Exception, "Failed to get asset types: " + result.Message);
            return StatusCode(500, result.Message);
        }
        
        var types = result.Data
            .Select(t => new AssetTypesModel
            {
                AssetTypeId = t.AssetTypeID,
                AssetTypeName = t.AssetTypeName,
            })
            .ToList();
        
        return Ok(types);
    }
    
    /// <summary>
    /// Gets a list of license types with their id
    /// </summary>
    /// <returns>List of Software Asset / License Types</returns>
    [HttpGet("/licenseTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetLicenseTypes()
    {
        var result = _softwareAssetService.GetLicenseTypes();
        if (!result.Ok)
        {
            _logger.Error(result.Exception, "Failed to get asset types: " + result.Message);
            return StatusCode(500, result.Message);
        }
        
        var types = result.Data
            .Select(t => new AssetTypesModel
            {
                AssetTypeId = t.LicenseTypeID,
                AssetTypeName = t.LicenseTypeName,
            })
            .ToList();
        
        return Ok(types);
    }
    
    /// <summary>
    /// Gets a list of user roles
    /// </summary>
    /// <returns>List of roles</returns>
    [HttpGet("/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<string>> GetRoles()
    {
        var roles = _roleManager.Roles.ToList();

        var rolesData = roles.Select(r => r.Name).ToList();
        
        return Ok(rolesData);
    }
}
using ITAssetTracking.API.Models;
using ITAssetTracking.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AssetsController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly IAssetService _assetService;

    public AssetsController(Serilog.ILogger logger, IAssetService assetService)
    {
        _logger = logger;
        _assetService = assetService;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<AssetModel>> GetDepartmentAssets(int id)
    {
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
    /// Get assets assigned to a given Employee
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>List of Assets</returns>
    [HttpGet("employee/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<AssetModel>> GetEmployeeAssets(int id)
    {
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<AssetModel> GetAsset(int assetId)
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
        var asset = new AssetModel(assetResult.Data);
        return Ok(asset);
    }

    /// <summary>
    /// Returns a list of currently available assets
    /// </summary>
    /// <returns></returns>
    [HttpGet("available")]
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
    
    
}
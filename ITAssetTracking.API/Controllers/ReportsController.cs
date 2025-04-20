using ITAssetTracking.API.Models;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly IReportsService _service;

    public ReportsController(Serilog.ILogger logger, IReportsService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Generates Distribution, Status and Value Reports for Assets
    /// </summary>
    /// <param name="fromDate">default: today 3 months ago</param>
    /// <param name="toDate">default: today</param>
    /// <param name="assetTypeId">default: 0(all)</param>
    /// <param name="departmentId">default: 0(all)</param>
    /// <returns>A list Asset Distribution and Status Reports, Value Report</returns>
    [HttpGet("assets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<AssetReportsModel> GenerateReport(
        DateTime? fromDate, DateTime? toDate,
        byte assetTypeId = 0,
        byte departmentId = 0)
    {
        var from  = fromDate ?? DateTime.Today.AddMonths(-3);
        var to = toDate ?? DateTime.Today;
        
        var distributionReport = _service.GetAssetDistributionReport(from, to, departmentId, assetTypeId);
        var statusReport = _service.GetAssetStatusReport(from, to, departmentId, assetTypeId);
        var valueReport = _service.GetAssetValuesReport(from, to);
        if (!distributionReport.Ok || !statusReport.Ok || !valueReport.Ok)
        {
            var ex = distributionReport.Exception ?? valueReport.Exception ?? statusReport.Exception;
            var msg = distributionReport.Message ?? valueReport.Message ?? statusReport.Message;
            
            if (ex != null)
            {
                _logger.Error(ex, "Error generating asset reports: " + msg);
                return StatusCode(500, msg);
            }
            return BadRequest(msg);
        }

        var model = new AssetReportsModel
        {
            AssetDistributionReports = distributionReport.Data,
            AssetStatusReports = statusReport.Data,
            AssetValuesReport = valueReport.Data,
        };
        
        return Ok(model);
    }

    /// <summary>
    /// Generates Distribution and Status Reports for Software Assets
    /// </summary>
    /// <param name="fromDate">Default: Today 3 months ago</param>
    /// <param name="toDate">Default: Today</param>
    /// <param name="licenseTypeId">Default: 0(all)</param>
    /// <param name="departmentId">Default: 0(all)</param>
    /// <returns>List of Software Asset / License Distribution and Status reports</returns>
    [HttpGet("licenses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LicenseReportsModel> GenerateSoftwareAssetReports(
        DateTime? fromDate, DateTime? toDate,
        byte licenseTypeId = 0, byte departmentId = 0)
    {
         var from = fromDate ?? DateTime.Today.AddMonths(-3);
         var to = toDate ?? DateTime.Today;

         var licenseDistribution = _service.GetSoftwareAssetDistributionReport(from, to, departmentId, licenseTypeId);
         var statusReport = _service.GetSoftwareAssetStatusReport(from, to, departmentId, licenseTypeId);

         if (!licenseDistribution.Ok || !statusReport.Ok)
         {
             var ex = licenseDistribution.Exception ?? statusReport.Exception;
             var msg = licenseDistribution.Message ?? statusReport.Message;
             if (ex != null)
             {
                 _logger.Error(ex, "Error generating license reports: " + msg);
                 return StatusCode(500, msg);
             }
             return BadRequest(msg);
         }

         var model = new LicenseReportsModel
         {
             SoftwareAssetDistributionReports = licenseDistribution.Data,
             SoftwareAssetStatusReports = statusReport.Data,
         };
         
         return Ok(model);
    }

    /// <summary>
    /// Generates Support Ticket Report
    /// </summary>
    /// <param name="fromDate">Default: Today 3 months ago</param>
    /// <param name="toDate">Default: Today</param>
    /// <param name="ticketTypeId">Default 0(all)</param>
    /// <returns></returns>
    [HttpGet("tickets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<TicketsReport> GenerateTicketsReport(DateTime? fromDate, DateTime? toDate, byte ticketTypeId = 0)
    {
        var from = fromDate ?? DateTime.Today.AddMonths(-3);
        var to = toDate ?? DateTime.Today;
        
        var ticketsReport = _service.GetTicketsReport(from, to, ticketTypeId);
        if (!ticketsReport.Ok)
        {
            if (ticketsReport.Exception != null)
            {
                _logger.Error(ticketsReport.Exception, "Error generating tickets report: " + ticketsReport.Message);
                return StatusCode(500, ticketsReport.Message);
            }
            return BadRequest(ticketsReport.Message);
        }
        
        return Ok(ticketsReport.Data);
    }
}
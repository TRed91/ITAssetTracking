using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;
using ITAssetTracking.Data;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

public class TicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IAssetService _assetService;
    private readonly IEmployeeService _employeeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Serilog.ILogger _logger;

    public TicketController(
        ITicketService ticketService,
        IAssetService assetService,
        IEmployeeService employeeService,
        UserManager<ApplicationUser> userManager,
        Serilog.ILogger logger)
    {
        _ticketService = ticketService;
        _assetService = assetService;
        _employeeService = employeeService;
        _userManager = userManager;
        _logger = logger;
    }
    
    public IActionResult Index(TicketsIndexModel model, int page = 1)
    {
        Result<List<Ticket>> ticketsResult;
        if (model.OnlyUnassigned)
        {
            ticketsResult = _ticketService.GetUnassignedTickets();
        }
        else
        {
            ticketsResult = _ticketService.GetTickets(page);
        }
        if (!ticketsResult.Ok)
        {
            _logger.Error($"There was an error retrieving tickets data: {ticketsResult.Message} => {ticketsResult.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, $"There was an error retrieving tickets data: {ticketsResult.Message}"));
            return RedirectToAction("Index", "Home");
        }
        var tickets = ticketsResult.Data;
        
        // populate Select Lists
        var ticketPriorities = _ticketService.GetTicketPriorities();
        var ticketTypes = _ticketService.GetTicketTypes();
        var ticketStatuses = _ticketService.GetTicketStatuses();
        if (!ticketPriorities.Ok || !ticketTypes.Ok || !ticketStatuses.Ok)
        {
            var ex = ticketPriorities.Exception ?? ticketTypes.Exception ?? ticketStatuses.Exception;
            var errMsg = $"There was an error retrieving data: " +
                         $"{ ticketPriorities.Message ?? ticketTypes.Message ?? ticketStatuses.Message }";
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            _logger.Error(errMsg + " => " + ex);
            return RedirectToAction("Index", "Home");
        }

        if (model.TicketStatusId != null && model.TicketStatusId > 0)
        {
            tickets = tickets
                .Where(t => t.TicketStatusID == model.TicketStatusId)
                .ToList();
        }
        if (model.TicketPriorityId != null && model.TicketPriorityId > 0)
        {
            tickets = tickets
                .Where(t => t.TicketPriorityID == model.TicketPriorityId)
                .ToList();
        }
        if (model.TicketTypeId != null && model.TicketTypeId > 0)
        {
            tickets = tickets
                .Where(t => t.TicketTypeID == model.TicketTypeId)
                .ToList();
        }
        if (!string.IsNullOrEmpty(model.Search))
        {
            tickets = tickets
                .Where(t => t.Asset.Model.ModelNumber.Contains(model.Search))
                .ToList();
        }

        switch (model.Order)
        {
            case TicketsOrder.Asset:
                tickets = tickets.OrderBy(t => t.Asset.Model.ModelNumber).ToList();
                break;
            case TicketsOrder.Priority:
                tickets = tickets.OrderBy(t => t.TicketPriorityID).ToList();
                break;
            case TicketsOrder.Status:
                tickets = tickets.OrderBy(t => t.TicketStatusID).ToList();
                break;
            case TicketsOrder.Type:
                tickets = tickets.OrderBy(t => t.TicketTypeID).ToList();
                break;
            default:
                tickets = tickets.OrderByDescending(t => t.DateReported).ToList();
                break;
        }
        
        model.Tickets = tickets;
        model.TicketTypeList = new SelectList(ticketTypes.Data, "TicketTypeID", "TicketTypeName");
        model.TicketPriorityList = new SelectList(ticketPriorities.Data, "TicketPriorityID", "TicketPriorityName");
        model.TicketStatusList = new SelectList(ticketStatuses.Data, "TicketStatusID", "TicketStatusName");
        
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Add(long assetId)
    {
        var asset = _assetService.GetAssetById(assetId);
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var employee = _employeeService.GetEmployeeById(user.EmployeeID);
        var types = _ticketService.GetTicketTypes();
        var priorities = _ticketService.GetTicketPriorities();
        if (!asset.Ok || !employee.Ok || !types.Ok || !priorities.Ok)
        {
            var ex = asset.Exception ?? employee.Exception ?? types.Exception ?? priorities.Exception;
            var errMsg = asset.Message ?? employee.Message ?? 
                types.Message ?? priorities.Message ?? "Unknown Error";
            _logger.Error(errMsg + " => " + ex);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            return RedirectToAction("Index", "Home");
        }

        var model = new TicketFormModel();
        
        model.AssetId = assetId;
        model.SerialNumber = asset.Data.SerialNumber;
        model.ReportedByEmployeeId = user.EmployeeID;
        model.ReportedEmployeeName = employee.Data.LastName + ", " + employee.Data.FirstName;
        model.TypeSelectList = new SelectList(types.Data, "TicketTypeID", "TicketTypeName");
        model.PrioritySelectList = new SelectList(priorities.Data, "TicketPriorityID", "TicketPriorityName");
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(TicketFormModel model)
    {
        if (!ModelState.IsValid)
        {
            var types = _ticketService.GetTicketTypes();
            var priorities = _ticketService.GetTicketPriorities();
            model.TypeSelectList = new SelectList(types.Data, "TicketTypeID", "TicketTypeName");
            model.PrioritySelectList = new SelectList(priorities.Data, "TicketPriorityID", "TicketPriorityName");
            return View(model);
        }
        var ticket = model.ToEntity();
        var result = _ticketService.AddTicket(ticket);
        if (!result.Ok)
        {
            _logger.Error($"There was an error adding ticket: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Add", new {assetId = model.AssetId});
        }
        var successMsg = "Ticket added successfully";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Details", new {ticketId = ticket.TicketID});
    }

    [HttpGet]
    public IActionResult Edit(int ticketId)
    {
        var ticket = _ticketService.GetTicket(ticketId);
        if (!ticket.Ok)
        {
            _logger.Error($"There was an error retrieving ticket details: {ticket.Message} => {ticket.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, ticket.Message));
            return RedirectToAction("Details", new { ticketId });
        }
        var model = new TicketFormModel(ticket.Data);
        var types = _ticketService.GetTicketTypes().Data;
        var priorities = _ticketService.GetTicketPriorities().Data;
        var statuses = _ticketService.GetTicketStatuses().Data;
        var resolutions = _ticketService.GetTicketResolutions().Data;
        model.TypeSelectList = new SelectList(types, "TicketTypeID", "TicketTypeName");
        model.PrioritySelectList = new SelectList(priorities, "TicketPriorityID", "TicketPriorityName");
        model.StatusSelectList = new SelectList(statuses, "TicketStatusID", "TicketStatusName");
        model.ResolutionSelectList = new SelectList(resolutions, "TicketResolutionID", "TicketResolutionName");
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int ticketId, TicketFormModel model)
    {
        if (!ModelState.IsValid)
        {
            var types = _ticketService.GetTicketTypes().Data;
            var priorities = _ticketService.GetTicketPriorities().Data;
            var statuses = _ticketService.GetTicketStatuses().Data;
            var resolutions = _ticketService.GetTicketResolutions().Data;
            model.TypeSelectList = new SelectList(types, "TicketTypeID", "TicketTypeName");
            model.PrioritySelectList = new SelectList(priorities, "TicketPriorityID", "TicketPriorityName");
            model.StatusSelectList = new SelectList(statuses, "TicketStatusID", "TicketStatusName");
            model.ResolutionSelectList = new SelectList(resolutions, "TicketResolutionID", "TicketResolutionName");
            return View(model);
        }
        var ticket = model.ToEntity();
        Console.WriteLine("===model====> " + model.TicketTypeId);
        Console.WriteLine("===entity===> " + ticket.TicketTypeID);
        
        var result = _ticketService.UpdateTicket(ticket);
        if (!result.Ok)
        {
            _logger.Error($"There was an error editing ticket: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Edit", new { ticketId });
        }
        var successMsg = "Ticket updated successfully";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Details", new { ticketId });
    }

    public IActionResult Details(int ticketId)
    {
        var ticket = _ticketService.GetTicket(ticketId);
        if (!ticket.Ok)
        {
            _logger.Error($"There was an error getting ticket details: {ticket.Message} => {ticket.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, ticket.Message));
            return RedirectToAction("Index", "Home");
        }
        
        return View(ticket.Data);
    }
}
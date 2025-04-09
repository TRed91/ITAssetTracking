using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.MVC.Controllers;

[Authorize(Roles = "Admin, HelpDescTechnician")]
public class TicketNotesController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Serilog.ILogger _logger;

    public TicketNotesController(ITicketService ticketService, Serilog.ILogger logger,  UserManager<ApplicationUser> userManager)
    {
        _ticketService = ticketService;
        _userManager = userManager;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Add(int ticketId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var model = new TicketNoteModel();
        model.TicketId = ticketId;
        model.EmployeeId = user.EmployeeID;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(TicketNoteModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var note = model.ToEntity();
        var result = _ticketService.AddTicketNotes(note);
        if (!result.Ok)
        {
            _logger.Error($"Error adding ticket notes: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Details", "Ticket", new { ticketId = model.TicketId });
        }
        var successMsg = $"Added ticket notes to Ticket with ID {model.TicketId}";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Details", "Ticket", new { ticketId = model.TicketId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int ticketNoteId)
    {
        var ticket = _ticketService.GetTicketNotes(ticketNoteId);
        if (!ticket.Ok)
        {
            _logger.Error($"Error editing ticket notes: {ticket.Message} => {ticket.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, ticket.Message));
            return RedirectToAction("Index", "Home");
        }
        var user = await _userManager.GetUserAsync(HttpContext.User);
        
        var model = new TicketNoteModel(ticket.Data);
        model.EmployeeId = user.EmployeeID;
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(TicketNoteModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var note = model.ToEntity();
        var result = _ticketService.UpdateTicketNotes(note);
        if (!result.Ok)
        {
            _logger.Error($"Error updating ticket notes: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Details", "Ticket", new { ticketId = model.TicketId });
        }
        var successMsg = $"Note updated for Ticket with ID {model.TicketId}";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Details", "Ticket", new { ticketId = model.TicketId });
    }

    [HttpGet]
    public IActionResult Delete(int ticketNoteId)
    {
        var ticket = _ticketService.GetTicketNotes(ticketNoteId);
        if (!ticket.Ok)
        {
            _logger.Error($"Error deleting ticket notes: {ticket.Message} => {ticket.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, ticket.Message));
            return RedirectToAction("Index", "Home");
        }
        var model = new TicketNoteModel(ticket.Data);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(TicketNoteModel model)
    {
        var result = _ticketService.DeleteTicketNotes((int)model.TicketNoteId);
        if (!result.Ok)
        {
            _logger.Error($"Error deleting ticket notes: {result.Message} => {result.Exception}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, result.Message));
            return RedirectToAction("Details", "Ticket", new { ticketId = model.TicketId });
        }
        var successMsg = $"Deleted ticket note for ticket with ID {model.TicketId}";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Details", "Ticket", new { ticketId = model.TicketId });
    }
}
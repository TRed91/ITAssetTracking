using ITAssetTracking.API.Models;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetTracking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _service;
    private readonly Serilog.ILogger _logger;

    public TicketsController(ITicketService service, Serilog.ILogger logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Gets the last 20 items of ticket history the starting from the provided page parameter.
    /// Gets the most recent 20 entries by default.
    /// </summary>
    /// <param name="page">Page number. 1 = most recent 20 entries. 2 = next 20 entries and so on</param>
    /// <returns>List of Tickets</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Auditor, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<TicketModel>> GetTicketHistory(int page = 1)
    {
        var result = _service.GetTickets(page);
        if (!result.Ok)
        {
            _logger.Error(result.Exception, $"Error retrieving ticket history: {result.Message}");
            return StatusCode(500, result.Message);
        }

        var tickets = result.Data
            .Select(t => new TicketModel(t))
            .ToList();
        
        return Ok(tickets);
    }

    /// <summary>
    /// Gets all tickets that have not yet a resolution assigned
    /// </summary>
    /// <returns>List of Tickets</returns>
    [HttpGet("open")]
    [Authorize(Roles = "Admin, Auditor, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<TicketModel>> GetOpenTickets()
    {
        var result = _service.GetOpenTickets();
        if (!result.Ok)
        {
            _logger.Error(result.Exception, $"Error retrieving ticket history: {result.Message}");
            return StatusCode(500, result.Message);
        }

        var tickets = result.Data
            .Select(t => new TicketModel(t))
            .ToList();
        
        return Ok(tickets);
    }

    /// <summary>
    /// Gets all tickets that are not yet assigned to an employee
    /// </summary>
    /// <returns>List of Tickets</returns>
    [HttpGet("unassigned")]
    [Authorize(Roles = "Admin, Auditor, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<TicketModel>> GetUnassignedTickets()
    {
        var result = _service.GetUnassignedTickets();
        if (!result.Ok)
        {
            _logger.Error(result.Exception, $"Error retrieving ticket history: {result.Message}");
            return StatusCode(500, result.Message);
        }

        var tickets = result.Data
            .Select(t => new TicketModel(t))
            .ToList();
        
        return Ok(tickets);
    }

    /// <summary>
    /// Retrieves data about a single tickets
    /// </summary>
    /// <param name="ticketId">Ticket Id</param>
    /// <returns>Ticket</returns>
    [HttpGet("{ticketId}")]
    [Authorize(Roles = "Admin, Auditor, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TicketDetailsModel> GetTicket(int ticketId)
    {
        var result = _service.GetTicket(ticketId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error retrieving ticket: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound();
        }
        var ticket = new TicketDetailsModel(result.Data);
        return Ok(ticket);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult CreateTicket(TicketForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var ticket = form.ToEntity();
        var result = _service.AddTicket(ticket);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error adding ticket: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }

        return Created();
    }

    [HttpPut("{ticketId}")]
    [Authorize(Roles = "Admin, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult UpdateTicket(int ticketId, TicketUpdateForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var ticket = form.ToEntity();
        ticket.TicketID = ticketId;
        
        var result = _service.UpdateTicket(ticket);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error updating ticket: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return Conflict(result.Message);
        }
        return NoContent();
    }

    [HttpDelete("{ticketId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteTicket(int ticketId)
    {
        var result = _service.DeleteTicket(ticketId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error deleting ticket: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        return NoContent();
    }

    /// <summary>
    /// Gets the noted for a given ticket
    /// </summary>
    /// <param name="ticketId"></param>
    /// <returns>List of Ticket Notes</returns>
    [HttpGet("{ticketId}/notes")]
    [Authorize(Roles = "Admin, Auditor, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<TicketNotesModel>> GetTicketNotes(int ticketId)
    {
        var result = _service.GetTicketNotesByTicket(ticketId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error retrieving ticket notes: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        var notes = result.Data
            .Select(t => new TicketNotesModel(t))
            .ToList();
        
        return Ok(notes);
    }

    [HttpPost("{ticketId}/notes")]
    [Authorize(Roles = "Admin, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult CreateTicketNote(int ticketId, TicketNoteForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var ticketNote = form.ToEntity();
        ticketNote.TicketID = ticketId;
        
        var result = _service.AddTicketNotes(ticketNote);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error adding ticket note: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        return Created();
    }

    [HttpPut("notes/{ticketNoteId}")]
    [Authorize(Roles = "Admin, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult UpdateTicketNote(int ticketNoteId, TicketNoteForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var ticketNote = form.ToEntity();
        ticketNote.TicketNoteID = ticketNoteId;
        
        var result = _service.UpdateTicketNotes(ticketNote);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error updating ticket note: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        return NoContent();
    }

    [HttpDelete("notes/{ticketNoteId}")]
    [Authorize(Roles = "Admin, HelpDescTechnician")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteTicketNote(int ticketNoteId)
    {
        var result = _service.DeleteTicketNotes(ticketNoteId);
        if (!result.Ok)
        {
            if (result.Exception != null)
            {
                _logger.Error(result.Exception, $"Error deleting ticket note: {result.Message}");
                return StatusCode(500, result.Message);
            }
            return NotFound(result.Message);
        }
        return NoContent();
    }
}
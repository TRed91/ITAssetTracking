using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;
using ITAssetTracking.Data.Repositories;

namespace ITAssetTracking.App.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IAssetRepository _assetRepo;

    public TicketService(
        ITicketRepository ticketRepository, 
        IEmployeeRepository employeeRepository, 
        IAssetRepository assetRepository)
    {
        _ticketRepo = ticketRepository;
        _employeeRepo = employeeRepository;
        _assetRepo = assetRepository;
    }
    
    public Result<Ticket> GetTicket(int ticketId)
    {
        try
        {
            var ticket = _ticketRepo.GetTicketById(ticketId);
            if (ticket == null)
            {
                return ResultFactory.Fail<Ticket>("Ticket not found");
            }

            return ResultFactory.Success(ticket);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Ticket>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetTickets()
    {
        try
        {
            var tickets = _ticketRepo.GetTickets();
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetUnassignedTickets()
    {
        try
        {
            var tickets = _ticketRepo.GetUnassignedTickets();
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetOpenTickets()
    {
        try
        {
            var tickets = _ticketRepo.GetOpenTickets();
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetTicketsInDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            var tickets = _ticketRepo.GetTicketsInDateRange(startDate, endDate);
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetTicketsByStatus(int ticketStatusId, bool includeClosed = true)
    {
        try
        {
            var tickets = _ticketRepo.GetTicketsByStatus(ticketStatusId, includeClosed = true);
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetTicketsByType(int ticketTypeId, bool includeClosed = true)
    {
        try
        {
            var tickets = _ticketRepo.GetTicketsByType(ticketTypeId, includeClosed);
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetTicketsByPriority(int ticketPriorityId, bool includeClosed = true)
    {
        try
        {
            var tickets = _ticketRepo.GetTicketsByPriority(ticketPriorityId, includeClosed);
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result<List<Ticket>> GetTicketsByAsset(int assetId, bool includeClosed = true)
    {
        try
        {
            var tickets = _ticketRepo.GetTicketsByAsset(assetId, includeClosed);
            return ResultFactory.Success(tickets);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Ticket>>(ex.Message, ex);
        }
    }

    public Result AddTicket(Ticket ticket)
    {
        ticket.DateReported = DateTime.Now;
        try
        {
            // check of asset exists
            var asset = _assetRepo.GetAssetById(ticket.AssetID);
            if (asset == null)
            {
                return ResultFactory.Fail("Asset not found");
            }
            // check of there are open tickets for the asset
            var assetTickets = _ticketRepo.GetTicketsByAsset(ticket.AssetID, false);
            if (assetTickets.Count > 0)
            {
                return ResultFactory.Fail("There are unresolved tickets for that asset");
            }

            // check if reporting employee exists
            var reportingEmployee = _employeeRepo.GetEmployee(ticket.ReportedByEmployeeID);
            if (reportingEmployee == null)
            {
                return ResultFactory.Fail("Reporting employee not found");
            }

            _ticketRepo.AddTicket(ticket);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateTicket(Ticket ticket)
    {
        try
        {
            var ticketToUpdate = _ticketRepo.GetTicketById(ticket.TicketID);
            if (ticketToUpdate == null)
            {
                return ResultFactory.Fail("Ticket not found");
            }

            // check of asset exists
            var asset = _assetRepo.GetAssetById(ticket.AssetID);
            if (asset == null)
            {
                return ResultFactory.Fail("Asset not found");
            }

            // check of there are open tickets for the asset
            var assetTickets = _ticketRepo.GetTicketsByAsset(ticket.AssetID, false);
            if (assetTickets.Count > 0 && assetTickets.Any(t => t.TicketID != ticket.TicketID))
            {
                return ResultFactory.Fail("There are unresolved tickets for that asset");
            }

            // check if employees exist
            var reportingEmployee = _employeeRepo.GetEmployee(ticket.ReportedByEmployeeID);
            if (reportingEmployee == null)
            {
                return ResultFactory.Fail("Reporting employee not found");
            }

            if (ticket.AssignedToEmployeeID != null)
            {
                var assignedEmployee = _employeeRepo.GetEmployee((int)ticket.AssignedToEmployeeID);
                if (assignedEmployee == null)
                {
                    return ResultFactory.Fail("Assigned employee not found");
                }
            }

            ticketToUpdate.TicketPriorityID = ticket.TicketPriorityID;
            ticketToUpdate.TicketTypeID = ticket.TicketTypeID;
            ticketToUpdate.TicketStatusID = ticket.TicketStatusID;
            ticketToUpdate.TicketResolutionID = ticket.TicketResolutionID;
            ticketToUpdate.AssignedToEmployeeID = ticket.AssignedToEmployeeID;
            ticketToUpdate.ReportedByEmployeeID = ticket.ReportedByEmployeeID;
            ticketToUpdate.AssetID = ticket.AssetID;
            ticketToUpdate.IssueDescription = ticket.IssueDescription;
            ticketToUpdate.DateClosed = ticket.DateClosed;

            _ticketRepo.UpdateTicket(ticketToUpdate);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteTicket(int ticketId)
    {
        try
        {
            var ticket = _ticketRepo.GetTicketById(ticketId);
            if (ticket == null)
            {
                return ResultFactory.Fail("Ticket not found");
            }

            _ticketRepo.DeleteTicket(ticketId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result<TicketNotes> GetTicketNotes(int ticketNoteId)
    {
        try
        {
            var ticketNote = _ticketRepo.GetTicketNoteById(ticketNoteId);
            if (ticketNote == null)
            {
                return ResultFactory.Fail<TicketNotes>("Ticket note not found");
            }

            return ResultFactory.Success(ticketNote);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<TicketNotes>(ex.Message, ex);
        }
    }

    public Result<List<TicketNotes>> GetTicketNotesByTicket(int ticketId)
    {
        try
        {
            var ticketNotes = _ticketRepo.GetTicketNotes(ticketId);
            return ResultFactory.Success(ticketNotes);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<TicketNotes>>(ex.Message, ex);
        }
    }

    public Result AddTicketNotes(TicketNotes ticketNotes)
    {
        ticketNotes.CreatedDate = DateTime.Now;
        try
        {
            //check if employee exists
            var employee = _employeeRepo.GetEmployee(ticketNotes.EmployeeID);
            if (employee == null)
            {
                return ResultFactory.Fail("Employee not found");
            }
            // check if ticket exists
            var ticket = _ticketRepo.GetTicketById(ticketNotes.TicketID);
            if (ticket == null)
            {
                return ResultFactory.Fail("Ticket not found");
            }

            _ticketRepo.AddTicketNote(ticketNotes);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateTicketNotes(TicketNotes ticketNotes)
    {
        try
        {
            var notesToUpdate = _ticketRepo.GetTicketNoteById(ticketNotes.TicketNoteID);
            if (notesToUpdate == null)
            {
                return ResultFactory.Fail("Ticket note not found");
            }

            //check if employee exists
            var employee = _employeeRepo.GetEmployee(ticketNotes.EmployeeID);
            if (employee == null)
            {
                return ResultFactory.Fail("Employee not found");
            }

            // check if ticket exists
            var ticket = _ticketRepo.GetTicketById(ticketNotes.TicketID);
            if (ticket == null)
            {
                return ResultFactory.Fail("Ticket not found");
            }

            notesToUpdate.EmployeeID = ticketNotes.EmployeeID;
            notesToUpdate.TicketID = ticketNotes.TicketID;
            notesToUpdate.Note = ticketNotes.Note;

            _ticketRepo.UpdateTicketNote(notesToUpdate);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteTicketNotes(int ticketNoteId)
    {
        try
        {
            var ticketNotes = _ticketRepo.GetTicketNoteById(ticketNoteId);
            if (ticketNotes == null)
            {
                return ResultFactory.Fail("Ticket note not found");
            }

            _ticketRepo.DeleteTicketNote(ticketNoteId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result<List<TicketStatus>> GetTicketStatuses()
    {
        try
        {
            var ticketStatuses = _ticketRepo.GetTicketStatuses();
            return ResultFactory.Success(ticketStatuses);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<TicketStatus>>(ex.Message, ex);
        }
    }

    public Result<List<TicketPriority>> GetTicketPriorities()
    {
        try
        {
            var ticketPriorities = _ticketRepo.GetTicketPriorities();
            return ResultFactory.Success(ticketPriorities);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<TicketPriority>>(ex.Message, ex);
        }
    }

    public Result<List<TicketResolution>> GetTicketResolutions()
    {
        try
        {
            var ticketResolutions = _ticketRepo.GetTicketResolutions();
            return ResultFactory.Success(ticketResolutions);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<TicketResolution>>(ex.Message, ex);
        }
    }

    public Result<List<TicketType>> GetTicketTypes()
    {
        try
        {
            var ticketTypes = _ticketRepo.GetTicketTypes();
            return ResultFactory.Success(ticketTypes);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<TicketType>>(ex.Message, ex);
        }
    }
}
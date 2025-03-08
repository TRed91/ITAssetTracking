using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Data.Repositories;

public class TicketRepository : ITicketRepository
{
    private ITAssetTrackingContext _context;

    public TicketRepository(ITAssetTrackingContext context)
    {
        _context = context;
    }
    
    public Ticket? GetTicketById(int ticketId)
    {
        return _context.Ticket.FirstOrDefault(t => t.TicketID == ticketId);
    }

    public List<Ticket> GetTickets()
    {
        return _context.Ticket.ToList();
    }

    public List<Ticket> GetUnassignedTickets()
    {
        return _context.Ticket
            .Where(t => t.AssignedToEmployeeID == null)
            .ToList();
    }

    public List<Ticket> GetOpenTickets()
    {
        return _context.Ticket
            .Where(t => t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.Ticket
            .Where(t => t.DateReported >= startDate && t.DateReported <= endDate)
            .ToList();
    }

    public List<Ticket> GetTicketsByStatus(int ticketStatusId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.Ticket
                .Where(t => t.TicketStatusID == ticketStatusId)
                .ToList();
        }
        return _context.Ticket
            .Where(t => t.TicketStatusID == ticketStatusId && t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsByType(int ticketTypeId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.Ticket
                .Where(t => t.TicketTypeID == ticketTypeId)
                .ToList();
        }
        return _context.Ticket
            .Where(t => t.TicketTypeID == ticketTypeId && t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsByPriority(int ticketPriorityId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.Ticket
                .Where(t => t.TicketPriorityID == ticketPriorityId)
                .ToList();
        }
        return _context.Ticket
            .Where(t => t.TicketPriorityID == ticketPriorityId && t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsByAsset(long assetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _context.Ticket
                .Where(t => t.AssetID == assetId)
                .ToList();
        }

        return _context.Ticket
            .Where(t => t.AssetID == assetId && t.DateClosed == null)
            .ToList();
    }

    public void AddTicket(Ticket ticket)
    {
        _context.Ticket.Add(ticket);
        _context.SaveChanges();
    }

    public void UpdateTicket(Ticket ticket)
    {
        var ticketToUpdate = _context.Ticket.FirstOrDefault(t => t.TicketID == ticket.TicketID);
        if (ticketToUpdate != null)
        {
            ticketToUpdate.TicketPriorityID = ticket.TicketPriorityID;
            ticketToUpdate.TicketStatusID = ticket.TicketStatusID;
            ticketToUpdate.TicketResolutionID = ticket.TicketResolutionID;
            ticketToUpdate.DateClosed = ticket.DateClosed;
            ticketToUpdate.DateReported = ticket.DateReported;
            ticketToUpdate.AssignedToEmployeeID = ticket.AssignedToEmployeeID;
            ticketToUpdate.AssetID = ticket.AssetID;
            ticketToUpdate.IssueDescription = ticket.IssueDescription;
            ticketToUpdate.ReportedByEmployeeID = ticket.ReportedByEmployeeID;
            _context.SaveChanges();
        }
    }

    public void DeleteTicket(int ticketId)
    {
        var ticket = _context.Ticket.FirstOrDefault(t => t.TicketID == ticketId);
        if (ticket != null)
        {
            _context.Ticket.Remove(ticket);
            _context.SaveChanges();
        }
    }

    public TicketNotes? GetTicketNoteById(int ticketNoteId)
    {
        return _context.TicketNotes.FirstOrDefault(t => t.TicketNoteID == ticketNoteId);
    }

    public List<TicketNotes> GetTicketNotes(int ticketId)
    {
        return _context.TicketNotes
            .Where(t => t.TicketID == ticketId)
            .ToList();
    }

    public void AddTicketNote(TicketNotes ticketNote)
    {
        _context.TicketNotes.Add(ticketNote);
        _context.SaveChanges();
    }

    public void UpdateTicketNote(TicketNotes ticketNote)
    {
        var note = _context.TicketNotes.FirstOrDefault(t => t.TicketNoteID == ticketNote.TicketNoteID);
        if (note != null)
        {
            note.EmployeeID = ticketNote.EmployeeID;
            note.Note = ticketNote.Note;
            _context.SaveChanges();
        }
    }

    public void DeleteTicketNote(int ticketNoteId)
    {
        var note = _context.TicketNotes.FirstOrDefault(t => t.TicketNoteID == ticketNoteId);
        if (note != null)
        {
            _context.TicketNotes.Remove(note);
            _context.SaveChanges();
        }
    }

    public List<TicketStatus> GetTicketStatuses()
    {
        return _context.TicketStatus.ToList();
    }

    public List<TicketPriority> GetTicketPriorities()
    {
        return _context.TicketPriority.ToList();
    }

    public List<TicketResolution> GetTicketResolutions()
    {
        return _context.TicketResolution.ToList();
    }

    public List<TicketType> GetTicketTypes()
    {
        return _context.TicketType.ToList();
    }
}
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;

namespace ITAssetTracking.Tests.MockRepos;

public class MockTicketRepo : ITicketRepository
{
    private readonly MockDB _db;

    public MockTicketRepo()
    {
        _db = new MockDB();
    }
    
    public Ticket? GetTicketById(int ticketId)
    {
        return _db.Tickets.FirstOrDefault(t => t.TicketID == ticketId);
    }

    public List<Ticket> GetTickets()
    {
        return _db.Tickets;
    }

    public List<Ticket> GetUnassignedTickets()
    {
        return _db.Tickets
            .Where(t => t.AssignedToEmployeeID == null)
            .ToList();
    }

    public List<Ticket> GetOpenTickets()
    {
        return _db.Tickets
            .Where(t => t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsInDateRange(DateTime startDate, DateTime endDate)
    {
        return _db.Tickets
            .Where(t => t.DateReported >= startDate && t.DateReported <= endDate)
            .ToList();
    }

    public List<Ticket> GetTicketsByStatus(int ticketStatusId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.Tickets
                .Where(t => t.TicketStatusID == ticketStatusId)
                .ToList();
        }
        return _db.Tickets
            .Where(t => t.TicketStatusID == ticketStatusId && t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsByType(int ticketTypeId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.Tickets
                .Where(t => t.TicketTypeID == ticketTypeId)
                .ToList();
        }
        return _db.Tickets
            .Where(t => t.TicketTypeID == ticketTypeId && t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsByPriority(int ticketPriorityId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.Tickets
                .Where(t => t.TicketPriorityID == ticketPriorityId)
                .ToList();
        }
        return _db.Tickets
            .Where(t => t.TicketPriorityID == ticketPriorityId && t.DateClosed == null)
            .ToList();
    }

    public List<Ticket> GetTicketsByAsset(long assetId, bool includeClosed)
    {
        if (includeClosed)
        {
            return _db.Tickets
                .Where(t => t.AssetID == assetId)
                .ToList();
        }
        return _db.Tickets
            .Where(t => t.AssetID == assetId && t.DateClosed == null)
            .ToList();
    }

    public void AddTicket(Ticket ticket)
    {
        ticket.TicketID = _db.Tickets.Max(t => t.TicketID) + 1;
        _db.Tickets.Add(ticket);
    }

    public void UpdateTicket(Ticket ticket)
    {
        var ticketToUpdate = _db.Tickets.FirstOrDefault(t => t.TicketID == ticket.TicketID);
        ticketToUpdate.TicketStatusID = ticket.TicketStatusID;
        ticketToUpdate.TicketTypeID = ticket.TicketTypeID;
        ticketToUpdate.TicketPriorityID = ticket.TicketPriorityID;
        ticketToUpdate.TicketResolutionID = ticket.TicketResolutionID;
        
        ticketToUpdate.DateClosed = ticket.DateClosed;
        
        ticketToUpdate.AssignedToEmployeeID = ticket.AssignedToEmployeeID;
        ticketToUpdate.ReportedByEmployeeID = ticket.ReportedByEmployeeID;
        ticketToUpdate.AssetID = ticket.AssetID;
        ticketToUpdate.IssueDescription = ticket.IssueDescription;
        
    }

    public void DeleteTicket(int ticketId)
    {
        var ticket = _db.Tickets.FirstOrDefault(t => t.TicketID == ticketId);
        _db.Tickets.Remove(ticket);
    }

    public TicketNotes? GetTicketNoteById(int ticketNoteId)
    {
        return _db.TicketNotes.FirstOrDefault(t => t.TicketNoteID == ticketNoteId);
    }

    public List<TicketNotes> GetTicketNotes(int ticketId)
    {
        return _db.TicketNotes
            .Where(t => t.TicketID == ticketId)
            .ToList();
    }

    public void AddTicketNote(TicketNotes ticketNote)
    {
        ticketNote.TicketNoteID = _db.TicketNotes.Max(t => t.TicketNoteID) + 1;
        ticketNote.CreatedDate = DateTime.Now;
        _db.TicketNotes.Add(ticketNote);
    }

    public void UpdateTicketNote(TicketNotes ticketNote)
    {
        var note = _db.TicketNotes.FirstOrDefault(t => t.TicketNoteID == ticketNote.TicketNoteID);
        note.EmployeeID = ticketNote.EmployeeID;
        note.TicketID = ticketNote.TicketID;
        note.Note = ticketNote.Note;
    }

    public void DeleteTicketNote(int ticketNoteId)
    {
        var ticketNote = _db.TicketNotes.FirstOrDefault(t => t.TicketNoteID == ticketNoteId);
        _db.TicketNotes.Remove(ticketNote);
    }

    public List<TicketStatus> GetTicketStatuses()
    {
        throw new NotImplementedException();
    }

    public List<TicketPriority> GetTicketPriorities()
    {
        throw new NotImplementedException();
    }

    public List<TicketResolution> GetTicketResolutions()
    {
        throw new NotImplementedException();
    }

    public List<TicketType> GetTicketTypes()
    {
        throw new NotImplementedException();
    }
}
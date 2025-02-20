using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ITicketRepository
{
    Ticket? GetTicketById(int ticketId);
    
    List<Ticket> GetTickets();
    List<Ticket> GetUnassignedTickets();
    List<Ticket> GetOpenTickets();
    List<Ticket> GetTicketsInDateRange(DateTime startDate, DateTime endDate);
    List<Ticket> GetTicketsByStatus(int ticketStatusId);
    List<Ticket> GetTicketsByType(int ticketTypeId);
    List<Ticket> GetTicketsByPriority(int ticketPriorityId);
    
    void AddTicket(Ticket ticket);
    void UpdateTicket(Ticket ticket);
    void DeleteTicket(Ticket ticket);
    
    TicketNotes? GetTicketNoteById(int ticketNoteId);
    List<TicketNotes> GetTicketNotes(int ticketId);
    void AddTicketNote(TicketNotes ticketNote);
    void UpdateTicketNote(TicketNotes ticketNote);
    void DeleteTicketNote(TicketNotes ticketNote);
    
    List<TicketStatus> GetTicketStatuses();
    List<TicketPriority> GetTicketPriorities();
    List<TicketResolution> GetTicketResolutions();
    List<TicketType> GetTicketTypes();
}
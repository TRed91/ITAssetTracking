using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface ITicketService
{
    Result<Ticket> GetTicket(int ticketId);
    
    Result<List<Ticket>> GetTickets();
    Result<List<Ticket>> GetUnassignedTickets();
    Result<List<Ticket>> GetOpenTickets();
    Result<List<Ticket>> GetTicketsInDateRange(DateTime startDate, DateTime endDate);
    Result<List<Ticket>> GetTicketsByStatus(int ticketStatusId);
    Result<List<Ticket>> GetTicketsByType(int ticketTypeId);
    Result<List<Ticket>> GetTicketsByPriority(int ticketPriorityId);
    
    Result AddTicket(Ticket ticket);
    Result UpdateTicket(Ticket ticket);
    Result DeleteTicket(int ticketId);
    
    Result<TicketNotes> GetTicketNotes(int ticketNoteId);
    Result<List<TicketNotes>> GetTicketNotesByTicket(int ticketId);
    Result AddTicketNotes(TicketNotes ticketNotes);
    Result UpdateTicketNotes(TicketNotes ticketNotes);
    Result DeleteTicketNotes(int ticketNoteId);
    
    Result<List<TicketStatus>> GetTicketStatuses();
    Result<List<TicketPriority>> GetTicketPriorities();
    Result<List<TicketResolution>> GetTicketResolutions();
    Result<List<TicketType>> GetTicketTypes();
}
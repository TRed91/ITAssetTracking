﻿using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Core.Interfaces.Repositories;

public interface ITicketRepository
{
    Ticket? GetTicketById(int ticketId);
    
    List<Ticket> GetTickets(int page);
    List<Ticket> GetUnassignedTickets();
    List<Ticket> GetOpenTickets();
    List<Ticket> GetTicketsInDateRange(DateTime startDate, DateTime endDate);
    List<Ticket> GetTicketsByStatus(int ticketStatusId, bool includeClosed);
    List<Ticket> GetTicketsByType(int ticketTypeId, bool includeClosed);
    List<Ticket> GetTicketsByPriority(int ticketPriorityId, bool includeClosed);
    List<Ticket> GetTicketsByAsset(long assetId, bool includeClosed);
    
    void AddTicket(Ticket ticket);
    void UpdateTicket(Ticket ticket);
    void DeleteTicket(int ticketId);
    
    TicketNotes? GetTicketNoteById(int ticketNoteId);
    List<TicketNotes> GetTicketNotes(int ticketId);
    void AddTicketNote(TicketNotes ticketNote);
    void UpdateTicketNote(TicketNotes ticketNote);
    void DeleteTicketNote(int ticketNoteId);
    
    List<TicketStatus> GetTicketStatuses();
    List<TicketPriority> GetTicketPriorities();
    List<TicketResolution> GetTicketResolutions();
    List<TicketType> GetTicketTypes();
}
﻿namespace ITAssetTracking.Core.Entities;

public class TicketType
{
    public byte TicketTypeID { get; set; }
    public string TicketTypeName { get; set; }
    
    List<Ticket> Tickets { get; set; } = new List<Ticket>();
}

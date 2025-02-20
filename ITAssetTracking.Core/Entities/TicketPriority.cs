namespace ITAssetTracking.Core.Entities;

public class TicketPriority
{
    public byte TicketPriorityID { get; set; }
    public string TicketPriorityName { get; set; }
    
    List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
namespace ITAssetTracking.Core.Entities;

public class TicketStatus
{
    public byte TicketStatusID { get; set; }
    public string TicketStatusName { get; set; }
    
    List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
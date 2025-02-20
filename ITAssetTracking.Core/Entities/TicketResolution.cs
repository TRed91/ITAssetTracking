namespace ITAssetTracking.Core.Entities;

public class TicketResolution
{
    public byte TicketResolutionID { get; set; }
    public string TicketResolutionName { get; set; }
    
    List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class TicketResolution
{
    [Key]
    public byte TicketResolutionID { get; set; }
    public string TicketResolutionName { get; set; }
    
    List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
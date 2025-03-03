using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class TicketStatus
{
    [Key]
    public byte TicketStatusID { get; set; }
    public string TicketStatusName { get; set; }
    
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
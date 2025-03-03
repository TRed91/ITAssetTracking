using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class TicketPriority
{
    [Key]
    public byte TicketPriorityID { get; set; }
    public string TicketPriorityName { get; set; }
    
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
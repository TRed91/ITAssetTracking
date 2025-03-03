using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class TicketType
{
    [Key]
    public byte TicketTypeID { get; set; }
    public string TicketTypeName { get; set; }
    
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}

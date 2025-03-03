using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class TicketNotes
{
    [Key]
    public int TicketNoteID { get; set; }
    
    public int TicketID { get; set; }
    public int EmployeeID { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public string Note { get; set; }
    
    public Ticket Ticket { get; set; }
    public Employee Employee { get; set; }
}
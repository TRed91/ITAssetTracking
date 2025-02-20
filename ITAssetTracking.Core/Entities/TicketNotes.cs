namespace ITAssetTracking.Core.Entities;

public class TicketNotes
{
    public int TicketNoteID { get; set; }
    
    public int TicketID { get; set; }
    public int EmployeeID { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public string Note { get; set; }
    
    Ticket Ticket { get; set; }
    Employee Employee { get; set; }
}
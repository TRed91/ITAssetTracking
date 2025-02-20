namespace ITAssetTracking.Core.Entities;

public class Ticket
{
    public int TicketID { get; set; }
    
    public byte TicketStatusID { get; set; }
    public byte TicketTypeID { get; set; }
    public byte TicketPriorityID { get; set; }
    public byte TicketResolutionID { get; set; }
    public int ReportedByEmployeeID { get; set; }
    public int AssignedToEmployeeID { get; set; }
    public long AssetID { get; set; }
    
    public DateTime DateReported { get; set; }
    public DateTime DateClosed { get; set; }
    public string IssueDescription { get; set; }
    
    TicketStatus TicketStatus { get; set; }
    TicketType TicketType { get; set; }
    TicketPriority TicketPriority { get; set; }
    TicketResolution TicketResolution { get; set; }
    Employee AssignedToEmployee { get; set; }
    Employee ReportedByEmployee { get; set; }
    
    List<TicketNotes> TicketNotes { get; set; } = new List<TicketNotes>();
}
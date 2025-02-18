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
}
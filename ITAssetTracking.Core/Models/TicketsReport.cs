namespace ITAssetTracking.Core.Models;

public class TicketsReport
{
    public int TotalTickets { get; set; }
    public List<TicketTypeReport> ReportsList{ get; set; }
}

public class TicketTypeReport
{
    public string TicketTypeName { get; set; }
    public int NumberOfTickets { get; set; }
    public int AvgResolutionTimeInDays { get; set; }
    
    public int CompletedTickets { get; set; }
    public int CancelledTickets { get; set; }
    public int UserErrorTickets { get; set; }
    public int OtherTickets { get; set; }
}
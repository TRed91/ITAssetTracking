using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetTracking.Core.Entities;

public class Ticket
{
    [Key]
    public int TicketID { get; set; }
    
    public byte TicketStatusID { get; set; }
    public byte TicketTypeID { get; set; }
    public byte TicketPriorityID { get; set; }
    public byte? TicketResolutionID { get; set; }
    [ForeignKey("Employee")]
    public int ReportedByEmployeeID { get; set; }
    [ForeignKey("Employee")]
    public int? AssignedToEmployeeID { get; set; }
    public long AssetID { get; set; }
    
    public DateTime DateReported { get; set; }
    public DateTime? DateClosed { get; set; }
    public string IssueDescription { get; set; }
    
    public Asset Asset { get; set; }
    public TicketStatus TicketStatus { get; set; }
    public TicketType TicketType { get; set; }
    public TicketPriority TicketPriority { get; set; }
    public TicketResolution TicketResolution { get; set; }
    public Employee AssignedToEmployee { get; set; }
    public Employee ReportedByEmployee { get; set; }
    
    public List<TicketNotes> TicketNotes { get; set; } = new List<TicketNotes>();
}
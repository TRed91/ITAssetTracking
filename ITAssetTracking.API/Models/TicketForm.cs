using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class TicketForm
{
    [Required]
    [Range(1, byte.MaxValue)]
    public byte TicketTypeId { get; set; }
    
    [Required]
    [Range(1, byte.MaxValue)]
    public byte TicketPriorityId { get; set; }
    
    [Required]
    [Range(1, long.MaxValue)]
    public long AssetId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int ReportedByEmployeeId { get; set; }
    
    [Required]
    public string IssueDescription { get; set; }

    public Ticket ToEntity()
    {
        return new Ticket
        {
            TicketTypeID = TicketTypeId,
            TicketPriorityID = TicketPriorityId,
            AssetID = AssetId,
            ReportedByEmployeeID = ReportedByEmployeeId,
            IssueDescription = IssueDescription,
            DateReported = DateTime.Now,
        };
    }
}
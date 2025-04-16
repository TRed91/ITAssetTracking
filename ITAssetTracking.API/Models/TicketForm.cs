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

    public virtual Ticket ToEntity()
    {
        return new Ticket
        {
            TicketTypeID = TicketTypeId,
            TicketPriorityID = TicketPriorityId,
            AssetID = AssetId,
            ReportedByEmployeeID = ReportedByEmployeeId,
            IssueDescription = IssueDescription,
        };
    }
}

public class TicketUpdateForm : TicketForm, IValidatableObject
{
    public byte? TicketStatusId { get; set; }
    public byte? TicketResolutionId { get; set; }
    public int? AssignedToEmployeeId { get; set; }

    public override Ticket ToEntity()
    {
        var ticket = base.ToEntity();
        if (AssignedToEmployeeId.HasValue)
        {
            ticket.AssignedToEmployeeID = AssignedToEmployeeId;
        }

        if (TicketStatusId.HasValue)
        {
            ticket.TicketStatusID = TicketStatusId.Value;
        }

        if (TicketResolutionId.HasValue)
        {
            ticket.TicketResolutionID = TicketResolutionId.Value;
            ticket.DateClosed = DateTime.Now;
        }
        return ticket;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (AssignedToEmployeeId.HasValue && AssignedToEmployeeId < 1)
        {
            errors.Add(new ValidationResult("Employee Id must be a positive integer or empty", 
                ["AssignedToEmployeeId"]));
        }

        if (TicketStatusId.HasValue && TicketStatusId < 1)
        {
            errors.Add(new ValidationResult("Ticket Status Id must be a positive integer or empty", 
                ["TicketStatusId"]));
        }

        if (TicketResolutionId.HasValue && TicketResolutionId < 1)
        {
            errors.Add(new ValidationResult("Ticket Resolution Id must be a positive integer or empty", 
                ["TicketResolutionId"]));
        }
        return errors;
    }
}
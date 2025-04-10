using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class TicketFormModel
{
    public int? TicketId { get; set; }
    public long AssetId { get; set; }
    [Display(Name = "Status")]
    public byte? TicketStatusId { get; set; }
    [Required]
    [Display(Name = "Type")]
    public byte TicketTypeId { get; set; }
    [Required]
    [Display(Name = "Priority")]
    public byte TicketPriorityId { get; set; }
    [Display(Name = "Resolution")]
    public byte? TicketResolutionId { get; set; }
    public int ReportedByEmployeeId { get; set; }
    public int? AssignedToEmployeeId { get; set; }
    public DateTime DateReported { get; set; }
    public DateTime? DateClosed { get; set; }
    [Required]
    [Display(Name = "Issue Description")]
    public string IssueDescription { get; set; }
    
    public string? SerialNumber { get; set; }
    public string? ReportedEmployeeName { get; set; }
    public string? AssignedEmployeeName { get; set; }
    
    public SelectList? TypeSelectList { get; set; }
    public SelectList? PrioritySelectList { get; set; }
    public SelectList? ResolutionSelectList { get; set; }
    public SelectList? StatusSelectList { get; set; }

    public TicketFormModel() { }

    public TicketFormModel(Ticket entity)
    {
        TicketId = entity.TicketID;
        AssetId = entity.AssetID;
        TicketStatusId = entity.TicketStatusID;
        TicketTypeId = entity.TicketTypeID;
        TicketPriorityId = entity.TicketPriorityID;
        TicketResolutionId = entity.TicketResolutionID;
        ReportedByEmployeeId = entity.ReportedByEmployeeID;
        AssignedToEmployeeId = entity.AssignedToEmployeeID;
        DateReported = entity.DateReported;
        DateClosed = entity.DateClosed;
        IssueDescription = entity.IssueDescription;
        SerialNumber = entity.Asset.SerialNumber;
        ReportedEmployeeName = entity.ReportedByEmployee.LastName + 
                               ", " + entity.ReportedByEmployee.FirstName;
        AssignedEmployeeName = entity.AssignedToEmployee != null ? entity.AssignedToEmployee.LastName + 
                               ", " + entity.AssignedToEmployee.FirstName : "";
    }

    public Ticket ToEntity()
    {
        var ticket = new Ticket
        {
            AssetID = AssetId,
            TicketTypeID = TicketTypeId,
            TicketPriorityID = TicketPriorityId,
            ReportedByEmployeeID = ReportedByEmployeeId,
            AssignedToEmployeeID = AssignedToEmployeeId,
            IssueDescription = IssueDescription,
            DateReported = DateReported,
        };
        if (TicketId.HasValue)
        {
            ticket.TicketID = TicketId.Value;
        }
        if (TicketResolutionId.HasValue && TicketResolutionId.Value > 0)
        {
            ticket.TicketResolutionID = TicketResolutionId.Value;
        }
        if (TicketStatusId.HasValue)
        {
            ticket.TicketStatusID = TicketStatusId.Value;
        }
        if (DateClosed.HasValue)
        {
            ticket.DateClosed = DateClosed.Value;
        }
        return ticket;
    }
}
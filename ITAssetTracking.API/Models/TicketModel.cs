using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class TicketModel
{
    public int TicketId { get; set; }
    public string TicketStatus { get; set; }
    public string TicketType { get; set; }
    public string TicketPriority { get; set; }
    public string? TicketResolution { get; set; }
    public string ReportedBy { get; set; }
    public string? AssignedTo { get; set; }
    public long AssetId { get; set; }
    public string AssetSerialNumber { get; set; }
    public DateTime DateReported { get; set; }
    public DateTime? DateClosed { get; set; }
    public string IssueDescription { get; set; }

    public TicketModel() { }

    public TicketModel(Ticket entity)
    {
        TicketId = entity.TicketID;
        TicketStatus = entity.TicketStatus.TicketStatusName;
        TicketType = entity.TicketType.TicketTypeName;
        TicketPriority = entity.TicketPriority.TicketPriorityName;
        TicketResolution = entity.TicketResolutionID != null ? 
            entity.TicketResolution.TicketResolutionName :
            null;
        ReportedBy = entity.ReportedByEmployee.LastName + ", " + entity.ReportedByEmployee.FirstName;
        AssignedTo = entity.AssignedToEmployeeID != null ?
                entity.AssignedToEmployee.LastName + ", " + entity.AssignedToEmployee.FirstName : 
                null;
        AssetId = entity.AssetID;
        AssetSerialNumber = entity.Asset.SerialNumber;
        DateReported = entity.DateReported;
        DateClosed = entity.DateClosed;
        IssueDescription = entity.IssueDescription;
    }
}

public class TicketNotesModel
{
    public int TicketNoteId { get; set; }
    public string Employee { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Note { get; set; }

    public TicketNotesModel() { }

    public TicketNotesModel(TicketNotes entity)
    {
        TicketNoteId = entity.TicketNoteID;
        CreatedDate = entity.CreatedDate;
        Note = entity.Note;
        Employee = entity.Employee.LastName + ", " + entity.Employee.FirstName;
    }
}

public class TicketDetailsModel : TicketModel
{ 
    public int ReportedByEmployeeId { get; set; }
    public int? AssignedToEmployeeId { get; set; }
    public List<TicketNotesModel> TicketNotes { get; set; }

    public TicketDetailsModel() { }

    public TicketDetailsModel(Ticket entity) : base(entity)
    {
        ReportedByEmployeeId = entity.ReportedByEmployeeID;
        AssignedToEmployeeId = entity.AssignedToEmployeeID;
        
        TicketNotes = entity.TicketNotes
            .Select(t => new TicketNotesModel(t))
            .ToList();
    }
}
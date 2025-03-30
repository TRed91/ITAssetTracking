using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class TicketNoteModel
{
    public int? TicketNoteId { get; set; }
    public int TicketId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime CreatedDate { get; set; }
    [Required]
    [MinLength(1)]
    public string Note { get; set; }

    public TicketNoteModel() { }

    public TicketNoteModel(TicketNotes entity)
    {
        TicketNoteId = entity.TicketNoteID;
        TicketId = entity.TicketID;
        EmployeeId = entity.EmployeeID;
        CreatedDate = entity.CreatedDate;
        Note = entity.Note;
    }

    public TicketNotes ToEntity()
    {
        var notes = new TicketNotes
        {
            TicketID = TicketId,
            EmployeeID = EmployeeId,
            CreatedDate = CreatedDate,
            Note = Note
        };
        if (TicketNoteId.HasValue)
        {
            notes.TicketNoteID = TicketNoteId.Value;
        }
        return notes;
    }
}
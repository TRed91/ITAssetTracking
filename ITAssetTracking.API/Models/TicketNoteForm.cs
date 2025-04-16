using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class TicketNoteForm : IValidatableObject
{
    [Required]
    [Range(1, int.MaxValue)]
    public int EmployeeId { get; set; }
    [Required]
    public string Note { get; set; }
    
    public int? TicketId { get; set; }

    public TicketNotes ToEntity()
    {
        var note = new TicketNotes
        {
            EmployeeID = EmployeeId,
            Note = Note,
            CreatedDate = DateTime.Now
        };
        if (TicketId.HasValue)
        {
            note.TicketID = TicketId.Value;
        }
        return note;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (TicketId.HasValue && TicketId.Value < 1)
        {
            errors.Add(new ValidationResult("TicketId must be greater than 0 or empty", 
                ["TicketId"]));
        }
        return errors;
    }
}
using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class AssetAssignmentForm : IValidatableObject
{
    [Required]
    [Range(1, long.MaxValue)]
    public long AssetId { get; set; }
    
    [Required]
    [Range(1, byte.MaxValue)]
    public byte DepartmentId { get; set; }
    
    public int? EmployeeId { get; set; }
    
    public DateTime? ReturnDate { get; set; }

    public AssetAssignment ToAssetAssignment()
    {
        return new AssetAssignment
        {
            AssetID = AssetId,
            DepartmentID = DepartmentId,
            EmployeeID = EmployeeId,
            AssignmentDate = DateTime.Today,
            ReturnDate = ReturnDate,
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (EmployeeId.HasValue && EmployeeId.Value < 1)
        {
            errors.Add(new ValidationResult("Employee id must be greater than 0 or empty", ["EmployeeId"]));
        }
        
        return errors;
    }
}

public class AssetRequestForm : AssetAssignmentForm
{
    public byte? RequestResultId { get; set; }
    public string? RequestNote { get; set; }
    
    public AssetRequest ToAssetRequest()
    {
        return new AssetRequest
        {
            AssetID = AssetId,
            DepartmentID = DepartmentId,
            EmployeeID = EmployeeId,
            RequestDate = DateTime.Today,
            RequestResultID = RequestResultId,
            RequestNote = RequestNote
        };
    }
}
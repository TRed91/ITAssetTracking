using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class SoftwareAssignmentForm : IValidatableObject
{
    [Required]
    [Range(1, int.MaxValue)]
    public int SoftwareAssetId { get; set; }
    
    public long? AssetId { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? ReturnDate { get; set; }

    public SoftwareAssetAssignment ToEntity()
    {
        return new SoftwareAssetAssignment
        {
            SoftwareAssetID = SoftwareAssetId,
            AssetID = AssetId,
            EmployeeID = EmployeeId,
            AssignmentDate = DateTime.Today,
            ReturnDate = ReturnDate,
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (AssetId.HasValue && AssetId.Value < 1)
        {
            errors.Add(new ValidationResult("Asset Id must be greater than 0 or empty.", ["AssetId"]));
        }

        if (EmployeeId.HasValue && EmployeeId.Value < 1)
        {
            errors.Add(new ValidationResult("Employee Id must greater than 0 or empty.", ["EmployeeId"]));
        }

        if (!AssetId.HasValue && !EmployeeId.HasValue)
        {
            errors.Add(new ValidationResult("Either Asset Id or Employee Id must have a value.", ["AssetId", "EmployeeId"]));
        }

        if (AssetId.HasValue && EmployeeId.HasValue)
        {
            errors.Add(new ValidationResult("Software can be assigned to either Asset or Employee only", ["AssetId", "EmployeeId"]));
        }
        
        return errors;
    }
}
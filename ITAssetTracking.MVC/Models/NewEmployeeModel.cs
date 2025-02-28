using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class NewEmployeeModel : IValidatableObject
{
    [Required]
    [Display(Name = "Department")]
    public byte DepartmentID { get; set; }
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Required]
    [Display(Name = "Last Name")]
    [MinLength(2,  ErrorMessage = "Last Name must be at least 2 characters")]
    public string LastName { get; set; }

    public SelectList? DepartmentList { get; set; }

    public NewEmployeeModel() { }

    public NewEmployeeModel(Employee entity)
    {
        DepartmentID = entity.DepartmentID;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
    }

    public Employee ToEntity()
    {
        return new Employee
        {
            DepartmentID = DepartmentID,
            FirstName = FirstName,
            LastName = LastName
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();
        if (DepartmentID == 0)
        {
            errors.Add(new ValidationResult("Selecting a department is required", ["DepartmentID"]));
        }
        if (FirstName.Any(c => !char.IsLetter(c)))
        {
            errors.Add(new ValidationResult("First name contains invalid characters", ["FirstName"]));
        }
        if (LastName.Any(c => !char.IsLetter(c)))
        {
            errors.Add(new ValidationResult("Last name contains invalid characters", ["LastName"]));
        }
        return errors;
    }
}
using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.API.Models;

public class EmployeeForm
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [Range(1, byte.MaxValue)]
    public byte DepartmentId { get; set; }
    public string? Role { get; set; }

    public Employee ToEntity()
    {
        return new Employee
        {
            FirstName = FirstName,
            LastName = LastName,
            DepartmentID = DepartmentId,
        };
    }
}
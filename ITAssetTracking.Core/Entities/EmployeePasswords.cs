using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class EmployeePasswords
{
    [Key]
    public int PasswordID { get; set; }
    public int EmployeeID { get; set; }
    public string Password { get; set; }
    
    public Employee Employee { get; set; }
}
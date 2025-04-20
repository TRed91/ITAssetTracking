using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.API.Models;

public class LoginForm
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
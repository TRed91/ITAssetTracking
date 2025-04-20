using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITAssetTracking.Data;

// ASP Identity Context
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){}
}

public class ApplicationUser : IdentityUser
{
    public int EmployeeID { get; set; }
}

public static class UserManagerExtension
{
    public static Task<ApplicationUser?> FindByEmployeeIdAsync(this UserManager<ApplicationUser> userManager, int emplyoeeId)
    {
        return userManager.Users
            .FirstOrDefaultAsync(u => u.EmployeeID == emplyoeeId);
    }
}
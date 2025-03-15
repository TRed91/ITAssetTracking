using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Identity;

namespace ITAssetTracking.MVC.Middleware;

/// <summary>
/// Custom middleware that retrieves open requests count if the user is authorized as
/// 'admin', 'asset manager' or 'software license manager'
/// </summary>
public class FetchRequestsMiddleware
{
    private readonly RequestDelegate _next;

    public FetchRequestsMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var assetRequestService = context.RequestServices.GetRequiredService<IAssetRequestService>();
        var logger = context.RequestServices.GetRequiredService<Serilog.ILogger>();
        var user = await userManager.GetUserAsync(context.User);
        IList<string> roles = new List<string>();
        if (user != null)
        {
            roles = await userManager.GetRolesAsync(user);
        }

        if (roles.Contains("Admin") ||
            roles.Contains("AssetManager") ||
            roles.Contains("SoftwareLicenseManager"))
        {
            var requests = assetRequestService.GetOpenAssetRequests();
            if (!requests.Ok)
            {
                logger.Error("Error retrieving asset requests: " + requests.Message + requests.Exception);
            }
            context.Items.Add("Requests", requests.Data.Count);
        }
    
        await _next(context);
    }
}

/// <summary>
/// Extension method to expose middleware
/// </summary>
public static class FetchRequestsMiddlewareExtensions
{
    public static IApplicationBuilder UseFetchRequests(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FetchRequestsMiddleware>();
    }
}
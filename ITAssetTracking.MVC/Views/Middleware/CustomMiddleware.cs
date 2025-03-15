using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Identity;

namespace ITAssetTracking.MVC.Views.Middleware;

public static class CustomMiddleware
{
    public static async Task FetchRequests(HttpContext context, RequestDelegate next)
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
    
        await next(context);
    }
}
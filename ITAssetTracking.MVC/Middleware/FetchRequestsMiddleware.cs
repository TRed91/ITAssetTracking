using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
        ITempDataDictionaryFactory tempDataFactory = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
        ITempDataDictionary tempData = tempDataFactory.GetTempData(context);
        var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var assetRequestService = context.RequestServices.GetRequiredService<IAssetRequestService>();
        var softwareRequestService = context.RequestServices.GetRequiredService<ISoftwareRequestService>();
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
            var assetRequests = assetRequestService.GetOpenAssetRequests();
            if (!assetRequests.Ok)
            {
                logger.Error($"Error retrieving asset requests: {assetRequests.Message} => {assetRequests.Exception}");
            }
            var softwareRequests = softwareRequestService.GetOpenSoftwareRequests();
            if (!softwareRequests.Ok)
            {
                logger.Error($"Error retrieving asset requests: {softwareRequests.Message} => {softwareRequests.Exception}");
            }
            context.Items.Add("Requests", assetRequests.Data.Count + softwareRequests.Data.Count);
        }

        if (roles.Contains("Admin"))
        {
            tempData["IsAdmin"] = "true";
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
using ITAssetTracking.Core.Models;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface IReportsService
{
    Result<List<AssetDistributionReport>> GetAssetDistributionReport(DateTime startDate, DateTime endDate,
        byte? departmentId, byte? assetTypeId);
    Result<List<AssetStatusReport>> GetAssetStatusReport(DateTime startDate, DateTime endDate, 
        byte? departmentId, byte? assetTypeId);

    Result<AssetValuesReport> GetAssetValuesReport(DateTime startDate, DateTime endDate);

    Result<List<SoftwareAssetDistributionReport>> GetSoftwareAssetDistributionReport(DateTime startDate,
        DateTime endDate, byte? departmentId, byte? licenseTypeId);
    
    Result<List<AssetStatusReport>> GetSoftwareAssetStatusReport(DateTime startDate, DateTime endDate, 
        byte? departmentId, byte? licenseTypeId);
    
    Result<TicketsReport> GetTicketsReport(DateTime startDate, DateTime endDate, byte? ticketTypeId);
}
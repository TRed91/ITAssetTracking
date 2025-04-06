using ITAssetTracking.Core.Models;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.Core.Interfaces.Services;

public interface IReportsService
{
    Result<List<AssetDistributionReport>> GetAssetDistributionReport(DateTime startDate, DateTime endDate,
        byte departmentId = 0, byte assetTypeId = 0);
    Result<List<AssetStatusReport>> GetAssetStatusReport(DateTime startDate, DateTime endDate, 
        byte departmentId = 0, byte assetTypeId = 0);

    Result<AssetValuesReport> GetAssetValuesReport(DateTime startDate, DateTime endDate);

    Result<List<SoftwareAssetDistributionReport>> GetSoftwareAssetDistributionReport(DateTime startDate,
        DateTime endDate, byte departmentId = 0, byte licenseTypeId = 0);
    
    Result<List<AssetStatusReport>> GetSoftwareAssetStatusReport(DateTime startDate, DateTime endDate, 
        byte departmentId = 0, byte licenseTypeId = 0);
    
    Result<TicketsReport> GetTicketsReport(DateTime startDate, DateTime endDate, byte ticketTypeId = 0);
}
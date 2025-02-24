using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class SoftwareRequestService : ISoftwareRequestService
{
    private readonly ISoftwareAssetRequestRepository _sarRepo;
    

    public SoftwareRequestService(ISoftwareAssetRequestRepository softwareAssetRequestRepository)
    {
        _sarRepo = softwareAssetRequestRepository;
    }
    public Result<SoftwareAssetRequest> GetSoftwareRequestById(int softwareRequestId)
    {
        try
        {
            var request = _sarRepo.GetSoftwareAssetRequestById(softwareRequestId);
            if (request == null)
            {
                return ResultFactory.Fail<SoftwareAssetRequest>("Software request not found");
            }

            return ResultFactory.Success(request);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<SoftwareAssetRequest>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetRequest>> GetSoftwareRequests()
    {
        try
        {
            var requests = _sarRepo.GetSoftwareAssetRequests();
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetRequest>> GetOpenSoftwareRequests()
    {
        try
        {
            var requests = _sarRepo.GetOpenSoftwareAssetRequests();
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetRequest>> GetRequestsByEmployee(int employeeId, bool includeClosed = false)
    {
        try
        {
            var requests = _sarRepo.GetRequestsByEmployeeId(employeeId, includeClosed);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetRequest>> GetRequestsBySoftware(int softwareAssetId, bool includeClosed = false)
    {
        try
        {
            var requests = _sarRepo.GetRequestsBySoftwareId(softwareAssetId, includeClosed);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetRequest>> GetRequestsByAsset(long assetId, bool includeClosed = false)
    {
        try
        {
            var requests = _sarRepo.GetRequestsByAssetId(assetId, includeClosed);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetRequest>> GetRequestsByResult(int resultId)
    {
        try
        {
            var requests = _sarRepo.GetRequestsByResultId(resultId);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetRequest>> GetRequestsInDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            var requests = _sarRepo.GetRequestsInDateRange(startDate, endDate);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetRequest>>(ex.Message, ex);
        }
    }

    public Result AddSoftwareRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        softwareAssetRequest.RequestDate = DateTime.Now;
        try
        {
            //check if software asset exists
            var swAsset = 
        }
    }

    public Result UpdateSoftwareRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        throw new NotImplementedException();
    }

    public Result DeleteSoftwareRequest(int softwareRequestId)
    {
        throw new NotImplementedException();
    }
}
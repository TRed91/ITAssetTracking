using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class SoftwareRequestService : ISoftwareRequestService
{
    private readonly ISoftwareAssetRequestRepository _sarRepo;
    private readonly ISoftwareAssetRepository _softwareRepo;
    private readonly ISoftwareAssetAssignmentRepository _softwareAssignmentRepo;
    private readonly IAssetRepository _assetRepo;
    private readonly IEmployeeRepository _employeeRepo;

    public SoftwareRequestService(
        ISoftwareAssetRequestRepository softwareAssetRequestRepository,
        ISoftwareAssetRepository softwareAssetRepository,
        ISoftwareAssetAssignmentRepository softwareAssetAssignmentRepository,
        IAssetRepository assetRepository,
        IEmployeeRepository employeeRepository)
    {
        _sarRepo = softwareAssetRequestRepository;
        _softwareRepo = softwareAssetRepository;
        _softwareAssignmentRepo = softwareAssetAssignmentRepository;
        _assetRepo = assetRepository;
        _employeeRepo = employeeRepository;
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
        if (!softwareAssetRequest.AssetID.HasValue && !softwareAssetRequest.EmployeeID.HasValue)
        {
            return ResultFactory.Fail("Asset Id or Employee Id is required");
        }
        softwareAssetRequest.RequestDate = DateTime.Now;
        try
        {
            //check if software asset exists
            var swAsset = _softwareRepo.GetSoftwareAsset(softwareAssetRequest.SoftwareAssetID);
            if (swAsset == null)
            {
                return ResultFactory.Fail("Software asset not found");
            }

            //check if software is already assigned
            var assignments = _softwareAssignmentRepo
                .GetAssignmentsBySoftwareAssetId(softwareAssetRequest.SoftwareAssetID, false);
            if (assignments.Count > 0)
            {
                return ResultFactory.Fail("Software asset already assigned");
            }

            //check if employee exists
            if (softwareAssetRequest.EmployeeID != null)
            {
                var employee = _employeeRepo.GetEmployee((int)softwareAssetRequest.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }
            }

            //check if asset exists
            if (softwareAssetRequest.AssetID != null)
            {
                var asset = _assetRepo.GetAssetById((int)softwareAssetRequest.AssetID);
                if (asset == null)
                {
                    return ResultFactory.Fail("Asset not found");
                }
            }

            _sarRepo.AddRequest(softwareAssetRequest);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateSoftwareRequest(SoftwareAssetRequest softwareAssetRequest)
    {
        if (!softwareAssetRequest.AssetID.HasValue && !softwareAssetRequest.EmployeeID.HasValue)
        {
            return ResultFactory.Fail("Asset Id or Employee Id is required");
        }
        try
        {
            var request = _sarRepo.GetSoftwareAssetRequestById(softwareAssetRequest.SoftwareAssetRequestID);
            if (request == null)
            {
                return ResultFactory.Fail("Software asset request not found");
            }
            //check if software asset exists
            var softwareAsset = _softwareRepo.GetSoftwareAsset(softwareAssetRequest.SoftwareAssetID);
            if (softwareAsset == null)
            {
                return ResultFactory.Fail("Software asset not found");
            }
            //check if asset exists 
            if (softwareAssetRequest.AssetID != null)
            {
                var asset = _assetRepo.GetAssetById((int)softwareAssetRequest.AssetID);
                if (asset == null)
                {
                    return ResultFactory.Fail("Asset not found");
                }
            }
            // check if employee exists
            if (softwareAssetRequest.EmployeeID != null)
            {
                var employee = _employeeRepo.GetEmployee((int)softwareAssetRequest.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }
            }

            _sarRepo.UpdateRequest(softwareAssetRequest);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteSoftwareRequest(int softwareRequestId)
    {
        try
        {
            var request = _sarRepo.GetSoftwareAssetRequestById(softwareRequestId);
            if (request == null)
            {
                return ResultFactory.Fail("Software asset request not found");
            }

            _sarRepo.DeleteRequest(request);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }
}
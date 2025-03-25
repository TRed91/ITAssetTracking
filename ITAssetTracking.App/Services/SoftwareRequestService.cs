using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Enums;
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
    private readonly IAssetRequestRepository _assetRequestRepo;
    private readonly IEmployeeRepository _employeeRepo;

    public SoftwareRequestService(
        ISoftwareAssetRequestRepository softwareAssetRequestRepository,
        ISoftwareAssetRepository softwareAssetRepository,
        ISoftwareAssetAssignmentRepository softwareAssetAssignmentRepository,
        IAssetRepository assetRepository,
        IAssetRequestRepository assetRequestRepository,
        IEmployeeRepository employeeRepository)
    {
        _sarRepo = softwareAssetRequestRepository;
        _softwareRepo = softwareAssetRepository;
        _softwareAssignmentRepo = softwareAssetAssignmentRepository;
        _assetRepo = assetRepository;
        _assetRequestRepo = assetRequestRepository;
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

            _sarRepo.DeleteRequest(softwareRequestId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result<List<LicenseType>> GetAvailableAssets()
    {
        try
        {
            var assets = _softwareRepo.GetAvailableAssets();
            
            var types = assets
                .Select(a => a.LicenseType)
                .Distinct()
                .ToList();
            
            return ResultFactory.Success(types);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<LicenseType>>(ex.Message, ex);
        }
    }

    public Result ResolveRequest(int softwareAssetRequestId, RequestResultEnum requestResult, string? note)
    {
        try
        {
            var request = _sarRepo.GetSoftwareAssetRequestById(softwareAssetRequestId);
            if (request == null)
            {
                return ResultFactory.Fail<AssetRequest>("Asset Request Not Found");
            }

            var result = new RequestResult();
            switch (requestResult)
            {
                case RequestResultEnum.Confirmed: 
                    var assignment = new SoftwareAssetAssignment
                    {
                        SoftwareAssetID = request.SoftwareAssetID,
                        AssetID = request.AssetID,
                        EmployeeID = request.EmployeeID,
                        AssignmentDate = DateTime.Now,
                    };
                    
                    var currentAssignments = _softwareAssignmentRepo.GetAssignmentsBySoftwareAssetId(request.SoftwareAssetID, false);
                    if (currentAssignments.Count > 0)
                    {
                        currentAssignments[0].ReturnDate = DateTime.Now;
                        _softwareAssignmentRepo.UpdateSoftwareAssetAssignment(currentAssignments[0]);
                    }
            
                    _softwareAssignmentRepo.AddSoftwareAssetAssignment(assignment);
                    
                    var swAsset = _softwareRepo.GetSoftwareAsset(request.SoftwareAssetID);
                    var assetStatus = _assetRepo.GetAssetStatusByName("In Use");
                    
                    swAsset.AssetStatusID = assetStatus.AssetStatusID;
                    _softwareRepo.UpdateSoftwareAsset(swAsset);

                    result = _assetRequestRepo.GetAssetRequestResult("Confirmed");
                    break;
                
                case RequestResultEnum.Denied:
                    result = _assetRequestRepo.GetAssetRequestResult("Denied");
                    break;
                
                case RequestResultEnum.Incompatible:
                    result = _assetRequestRepo.GetAssetRequestResult("Incompatible");
                    break;
            }
            
            request.RequestResultID = result.RequestResultID;
            request.RequestNote = note;
                    
            _sarRepo.UpdateRequest(request);
            
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<AssetRequest>(ex.Message, ex);
        }
    }
}
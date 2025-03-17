using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Enums;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class AssetRequestService : IAssetRequestService
{
    private readonly IAssetRequestRepository _assetRequestRepo;
    private readonly IAssetAssignmentRepository _assetAssignmentRepo;
    private readonly IAssetRepository _assetRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IDepartmentRepository _departmentRepo;

    public AssetRequestService(
        IAssetRequestRepository assetRequestRepository, 
        IAssetAssignmentRepository assetAssignmentRepository,
        IAssetRepository assetRepository, 
        IEmployeeRepository employeeRepository, 
        IDepartmentRepository departmentRepository)
    {
        _assetRequestRepo = assetRequestRepository;
        _assetAssignmentRepo = assetAssignmentRepository;
        _assetRepo = assetRepository;
        _employeeRepo = employeeRepository;
        _departmentRepo = departmentRepository;
    }
    
    public Result<AssetRequest> GetAssetRequestById(int assetRequestId)
    {
        try
        {
            var request = _assetRequestRepo.GetAssetRequestById(assetRequestId);
            if (request == null)
            {
                return ResultFactory.Fail<AssetRequest>("Asset Request Not Found");
            }

            return ResultFactory.Success(request);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<AssetRequest>(ex.Message, ex);
        }
    }

    public Result<List<AssetType>> GetAvailableAssets()
    {
        try
        {
            var assets = _assetRepo.GetAvailableAssets();
            var types = assets
                .Select(a => a.AssetType)
                .Distinct()
                .ToList();
            
            foreach (var type in types)
            {
                var a = assets
                    .Where(a => a.AssetTypeID == type.AssetTypeID)
                    .ToList(); 
                type.Assets.AddRange(a);
            }
            
            return ResultFactory.Success(types);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetType>>(ex.Message, ex);
        }
    }

    public Result ResolveRequest(int assetRequestId, RequestResultEnum requestResult, string? note)
    {
        try
        {
            var request = _assetRequestRepo.GetAssetRequestById(assetRequestId);
            if (request == null)
            {
                return ResultFactory.Fail<AssetRequest>("Asset Request Not Found");
            }

            var result = new RequestResult();
            switch (requestResult)
            {
                case RequestResultEnum.Confirmed: 
                    var assignment = new AssetAssignment
                    {
                        AssetID = request.AssetID,
                        EmployeeID = request.EmployeeID,
                        DepartmentID = request.DepartmentID,
                        AssignmentDate = DateTime.Now,
                    };
            
                    _assetAssignmentRepo.AddAssetAssignment(assignment);
                    
                    var asset = _assetRepo.GetAssetById(request.AssetID);
                    var assetStatus = _assetRepo.GetAssetStatusByName("In Use");
                    
                    asset.AssetStatusID = assetStatus.AssetStatusID;
                    _assetRepo.UpdateAsset(asset);

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
                    
            _assetRequestRepo.UpdateAssetRequest(request);
            
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<AssetRequest>(ex.Message, ex);
        }
    }

    public Result<List<AssetRequest>> GetAllAssetRequests()
    {
        try
        {
            var requests = _assetRequestRepo.GetAssetRequests();
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<AssetRequest>> GetOpenAssetRequests()
    {
        try
        {
            var requests = _assetRequestRepo.GetOpenAssetRequests();
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<AssetRequest>> GetAssetRequestsByAsset(long assetId, bool includeClosed = false)
    {
        try
        {
            var requests = _assetRequestRepo.GetAssetRequestsByAssetId(assetId, includeClosed);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<AssetRequest>> GetAssetRequestsByEmployee(int employeeId, bool includeClosed = false)
    {
        try
        {
            var requests = _assetRequestRepo.GetAssetRequestsByEmployeeId(employeeId, includeClosed);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<AssetRequest>> GetAssetRequestsByDepartment(int departmentId, bool includeClosed = false)
    {
        try
        {
            var requests = _assetRequestRepo.GetAssetRequestsByDepartmentId(departmentId, includeClosed);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<AssetRequest>> GetAssetRequestsByResult(int resultId)
    {
        try
        {
            var requests = _assetRequestRepo.GetAssetRequestsByResultId(resultId);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetRequest>>(ex.Message, ex);
        }
    }

    public Result<List<AssetRequest>> GetAssetRequestsInDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            var requests = _assetRequestRepo.GetAssetRequestInDateRange(startDate, endDate);
            return ResultFactory.Success(requests);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetRequest>>(ex.Message, ex);
        }
    }

    public Result AddAssetRequest(AssetRequest assetRequest)
    {
        assetRequest.RequestDate = DateTime.Now;
        try
        {
            // check if asset exists
            var asset = _assetRepo.GetAssetById(assetRequest.AssetID);
            if (asset == null)
            {
                return ResultFactory.Fail("Asset not found");
            }
            // check if department exists
            var department = _departmentRepo.GetDepartmentById(assetRequest.DepartmentID);
            if (department == null)
            {
                return ResultFactory.Fail("Department not found");
            }
            // check if employee exists and is assigned to the same department
            if (assetRequest.EmployeeID != null && assetRequest.EmployeeID > 0)
            {
                var employee = _employeeRepo.GetEmployee((int)assetRequest.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }
                if (employee.DepartmentID != assetRequest.DepartmentID)
                {
                    return ResultFactory.Fail("Employee is not assigned to the requested department");
                }
            }
            _assetRequestRepo.AddAssetRequest(assetRequest);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
        
    }

    public Result UpdateAssetRequest(AssetRequest assetRequest)
    {
        try
        {
            var request = _assetRequestRepo.GetAssetRequestById(assetRequest.AssetRequestID);
            if (request == null)
            {
                return ResultFactory.Fail("Asset request not found");
            }
            // check if asset exists
            var asset = _assetRepo.GetAssetById(assetRequest.AssetID);
            if (asset == null)
            {
                return ResultFactory.Fail("Asset not found");
            }
            // check if department exists
            var department = _departmentRepo.GetDepartmentById(assetRequest.DepartmentID);
            if (department == null)
            {
                return ResultFactory.Fail("Department not found");
            }
            // check if employee exists and is assigned to the same department
            if (assetRequest.EmployeeID != null && assetRequest.EmployeeID > 0)
            {
                var employee = _employeeRepo.GetEmployee((int)assetRequest.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }
                if (employee.DepartmentID != assetRequest.DepartmentID)
                {
                    return ResultFactory.Fail("Employee is not assigned to the requested department");
                }
            }

            _assetRequestRepo.UpdateAssetRequest(assetRequest);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteAssetRequest(int assetRequestId)
    {
        try
        {
            var request = _assetRequestRepo.GetAssetRequestById(assetRequestId);
            if (request == null)
            {
                return ResultFactory.Fail("Asset request not found");
            }

            _assetRequestRepo.DeleteAssetRequest(assetRequestId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }
}
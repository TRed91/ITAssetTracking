using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class SoftwareAssetAssignmentService : ISoftwareAssetAssignmentService
{
    private readonly ISoftwareAssetAssignmentRepository _swaaRepository;
    private readonly ISoftwareAssetRepository _softwareRepo;
    private readonly IAssetRepository _assetRepo;
    private readonly IEmployeeRepository _employeeRepo;

    public SoftwareAssetAssignmentService(
        ISoftwareAssetAssignmentRepository softwareAssetAssignmentRepository, 
        ISoftwareAssetRepository softwareAssetRepository,
        IAssetRepository assetRepository,
        IEmployeeRepository employeeRepository)
    {
        _swaaRepository = softwareAssetAssignmentRepository;
        _softwareRepo = softwareAssetRepository;
        _assetRepo = assetRepository;
        _employeeRepo = employeeRepository;
    }
    
    public Result<SoftwareAssetAssignment> GetSoftwareAssetAssignment(int softwareAssetAssignmentId)
    {
        try
        {
            var assignment = _swaaRepository.GetSoftwareAssetAssignmentById(softwareAssetAssignmentId);
            if (assignment == null)
            {
                return ResultFactory.Fail<SoftwareAssetAssignment>("Software asset assignment not found");
            }

            return ResultFactory.Success(assignment);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<SoftwareAssetAssignment>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetAssignment>> GetSoftwareAssetAssignments(bool includeReturned = true)
    {
        try
        {
            var assignments = _swaaRepository.GetSoftwareAssetAssignments(includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetAssignment>> GetAssignmentsBySoftwareAssetId(int softwareAssetId, bool includeReturned = true)
    {
        try
        {
            var assignments = _swaaRepository.GetAssignmentsBySoftwareAssetId(softwareAssetId, includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetAssignment>> GetAssignmentsByEmployee(int employeeId, bool includeReturned = true)
    {
        try
        {
            var assignments = _swaaRepository.GetAssignmentsByEmployeeId(employeeId, includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetAssignment>> GetAssignmentByAsset(long assetId, bool includeReturned = true)
    {
        try
        {
            var assignments = _swaaRepository.GetAssignmentByAssetId(assetId, includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<SoftwareAssetAssignment>> GetAssignmentInDateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<List<SoftwareAssetAssignment>>("Start date cannot be earlier than end date");
        }

        try
        {
            var assignments = _swaaRepository.GetAssignmentsInDateRange(startDate, endDate);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<SoftwareAssetAssignment>>(ex.Message, ex);
        }
    }

    public Result AddSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        if (!softwareAssetAssignment.AssetID.HasValue && !softwareAssetAssignment.EmployeeID.HasValue)
        {
            return ResultFactory.Fail("Asset Id or Employee Id is required");
        }
        softwareAssetAssignment.AssignmentDate = DateTime.Now;
        try
        {
            // check if software asset exists
            var swAsset = _softwareRepo.GetSoftwareAsset(softwareAssetAssignment.SoftwareAssetID);
            if (swAsset == null)
            {
                return ResultFactory.Fail("Software asset not found");
            }
            // check if there are open assignment
            var assignments = _swaaRepository.GetAssignmentsBySoftwareAssetId(
                softwareAssetAssignment.SoftwareAssetID, false);
            if (assignments.Count > 0)
            {
                return ResultFactory.Fail("Software asset is already assigned");
            }
            // check if employee exists
            if (softwareAssetAssignment.EmployeeID != null)
            {
                var employee = _employeeRepo.GetEmployee((int)softwareAssetAssignment.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }
            }
            // check if asset exists
            if (softwareAssetAssignment.AssetID != null)
            {
                var asset = _assetRepo.GetAssetById((long)softwareAssetAssignment.AssetID);
                if (asset == null)
                {
                    return ResultFactory.Fail("Asset not found");
                }
            }

            _swaaRepository.AddSoftwareAssetAssignment(softwareAssetAssignment);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateSoftwareAssetAssignment(SoftwareAssetAssignment softwareAssetAssignment)
    {
        if (!softwareAssetAssignment.AssetID.HasValue && !softwareAssetAssignment.EmployeeID.HasValue)
        {
            return ResultFactory.Fail("Asset Id or Employee Id is required");
        }
        try
        {
            var assignmentToUpdate =
                _swaaRepository.GetSoftwareAssetAssignmentById(softwareAssetAssignment.AssetAssignmentID);
            if (assignmentToUpdate == null)
            {
                return ResultFactory.Fail("Software asset assignment not found");
            }

            // check if software asset exists
            var swAsset = _softwareRepo.GetSoftwareAsset(softwareAssetAssignment.SoftwareAssetID);
            if (swAsset == null)
            {
                return ResultFactory.Fail("Software asset not found");
            }

            // check if there are open assignment
            var assignments = _swaaRepository.GetAssignmentsBySoftwareAssetId(
                softwareAssetAssignment.SoftwareAssetID, false);
            if (assignments.Any(a => a.ReturnDate == null && 
                                     softwareAssetAssignment.AssetAssignmentID != a.AssetAssignmentID))
            {
                return ResultFactory.Fail("Software asset is already assigned");
            }

            // check if employee exists
            if (softwareAssetAssignment.EmployeeID != null)
            {
                var employee = _employeeRepo.GetEmployee((int)softwareAssetAssignment.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }
            }

            // check if asset exists
            if (softwareAssetAssignment.AssetID != null)
            {
                var asset = _assetRepo.GetAssetById((long)softwareAssetAssignment.AssetID);
                if (asset == null)
                {
                    return ResultFactory.Fail("Asset not found");
                }
            }

            assignmentToUpdate.EmployeeID = softwareAssetAssignment.EmployeeID;
            assignmentToUpdate.SoftwareAssetID = softwareAssetAssignment.SoftwareAssetID;
            assignmentToUpdate.AssetID = softwareAssetAssignment.AssetID;
            assignmentToUpdate.ReturnDate = softwareAssetAssignment.ReturnDate;

            _swaaRepository.UpdateSoftwareAssetAssignment(assignmentToUpdate);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteSoftwareAssetAssignment(int softwareAssetAssignmentId)
    {
        try
        {
            var assignmentToDelete = _swaaRepository.GetSoftwareAssetAssignmentById(softwareAssetAssignmentId);
            if (assignmentToDelete == null)
            {
                return ResultFactory.Fail("Software asset assignment not found");
            }

            _swaaRepository.DeleteSoftwareAssetAssignment(assignmentToDelete);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }
}
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Repositories;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Core.Utility;

namespace ITAssetTracking.App.Services;

public class AssetAssignmentService : IAssetAssignmentService
{
    private readonly IAssetAssignmentRepository _assetAssignmentRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IDepartmentRepository _departmentRepo;

    public AssetAssignmentService(
        IAssetAssignmentRepository assetAssignmentRepository, 
        IEmployeeRepository employeeRepository, 
        IDepartmentRepository departmentRepository)
    {
        _assetAssignmentRepo = assetAssignmentRepository;
        _employeeRepo = employeeRepository;
        _departmentRepo = departmentRepository;
    }
    
    public Result<AssetAssignment> GetAssetAssignmentById(int assetAssignmentId)
    {
        try
        {
            var assignment = _assetAssignmentRepo.GetAssetAssignmentById(assetAssignmentId);
            if (assignment == null)
            {
                return ResultFactory.Fail<AssetAssignment>("Asset assignment not found");
            }

            return ResultFactory.Success(assignment);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<AssetAssignment>(ex.Message, ex);
        }
    }

    public Result<List<AssetAssignment>> GetAllAssetAssignments(bool includeReturned = true)
    {
        try
        {
            var assignments = _assetAssignmentRepo.GetAllAssetAssignments(includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<AssetAssignment>> GetAssetAssignmentsByAsset(long assetId, bool includeReturned = true)
    {
        try
        {
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsByAssetId(assetId, includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<AssetAssignment>> GetAssetAssignmentsByDepartment(int departmentId, bool includeReturned = true)
    {
        try
        {
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsByDepartmentId(departmentId, includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<AssetAssignment>> GetAssetAssignmentsByEmployee(int employeeId, bool includeReturned = true)
    {
        try
        {
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsByEmployeeId(employeeId, includeReturned);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetAssignment>>(ex.Message, ex);
        }
    }

    public Result<List<AssetAssignment>> GetAssetAssignmentsInDateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return ResultFactory.Fail<List<AssetAssignment>>("'Start date' cannot be after 'end date'");
        }

        try
        {
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsInDateRange(startDate, endDate);
            return ResultFactory.Success(assignments);
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<AssetAssignment>>(ex.Message, ex);
        }
    }

    public Result AddAssetAssignment(AssetAssignment assetAssignment)
    {
        try
        {
            // Check if there are unreturned assignments for the asset
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsByAssetId(assetAssignment.AssetID, false);
            if (assignments.Any(a => a.ReturnDate == null))
            {
                return ResultFactory.Fail("Asset is currently in use");
            }
            // Check if Employee exists
            var employee = _employeeRepo.GetEmployee(assetAssignment.EmployeeID);
            if (employee == null)
            {
                return ResultFactory.Fail("Employee not found");
            }
            // Check of Department exists
            var department = _departmentRepo.GetDepartmentById(assetAssignment.DepartmentID);
            if (department == null)
            {
                return ResultFactory.Fail("Department not found");
            }
            // set assignment date to today
            assetAssignment.AssignmentDate = DateTime.Now;
            
            _assetAssignmentRepo.AddAssetAssignment(assetAssignment);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result UpdateAssetAssignment(AssetAssignment assetAssignment)
    {
        try
        {
            var originalAssignment = _assetAssignmentRepo.GetAssetAssignmentById(assetAssignment.AssetAssignmentID);
            if (originalAssignment == null)
            {
                return ResultFactory.Fail("Asset assignment not found");
            }
            
            // Check if there are unreturned assignments for the asset
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsByAssetId(assetAssignment.AssetID, false);
            if (assignments.Any(a => a.ReturnDate == null && a.AssetAssignmentID != assetAssignment.AssetAssignmentID))
            {
                return ResultFactory.Fail("Asset is currently in use");
            }

            originalAssignment.AssetID = assetAssignment.AssetID;
            originalAssignment.DepartmentID = assetAssignment.DepartmentID;
            originalAssignment.EmployeeID = assetAssignment.EmployeeID;
            originalAssignment.ReturnDate = assetAssignment.ReturnDate;

            _assetAssignmentRepo.UpdateAssetAssignment(originalAssignment);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }

    public Result DeleteAssetAssignment(int assetAssignmentId)
    {
        try
        {
            var assignment = _assetAssignmentRepo.GetAssetAssignmentById(assetAssignmentId);
            if (assignment == null)
            {
                return ResultFactory.Fail("Asset assignment not found");
            }

            _assetAssignmentRepo.DeleteAssetAssignment(assignment);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }
}
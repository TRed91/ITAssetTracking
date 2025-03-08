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
    private readonly IAssetRepository _assetRepo;

    public AssetAssignmentService(
        IAssetAssignmentRepository assetAssignmentRepository, 
        IEmployeeRepository employeeRepository, 
        IDepartmentRepository departmentRepository, 
        IAssetRepository assetRepository)
    {
        _assetAssignmentRepo = assetAssignmentRepository;
        _employeeRepo = employeeRepository;
        _departmentRepo = departmentRepository;
        _assetRepo = assetRepository;
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
            // check if asset exists
            var asset = _assetRepo.GetAssetById(assetAssignment.AssetID);
            if (asset == null)
            {
                return ResultFactory.Fail<AssetAssignment>("Asset not found");
            }
            // Check if there are unreturned assignments for the asset
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsByAssetId(assetAssignment.AssetID, false);
            if (assignments.Count > 0)
            {
                return ResultFactory.Fail("Asset is currently in use");
            }
            // Check if Department exists
            var department = _departmentRepo.GetDepartmentById(assetAssignment.DepartmentID);
            if (department == null)
            {
                return ResultFactory.Fail("Department not found");
            }
            // Check if Employee exists and is assigned to the correct department
            if (assetAssignment.EmployeeID != null && assetAssignment.EmployeeID != 0)
            {
                var employee = _employeeRepo.GetEmployee((int)assetAssignment.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }

                if (employee.DepartmentID != assetAssignment.DepartmentID)
                {
                    return ResultFactory.Fail("Employee is not assigned to the requested department");
                }
            }
            
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
            
            // check if asset exists
            var asset = _assetRepo.GetAssetById(assetAssignment.AssetID);
            if (asset == null)
            {
                return ResultFactory.Fail<AssetAssignment>("Asset not found");
            }
            // Check if there are unreturned assignments for the asset
            var assignments = _assetAssignmentRepo
                .GetAssetAssignmentsByAssetId(assetAssignment.AssetID, false);
            if (assignments.Any(a => a.ReturnDate == null && a.AssetAssignmentID != assetAssignment.AssetAssignmentID))
            {
                return ResultFactory.Fail("Asset is currently in use");
            }
            // Check if Employee exists
            if (assetAssignment.EmployeeID != null && assetAssignment.EmployeeID != 0)
            {
                var employee = _employeeRepo.GetEmployee((int)assetAssignment.EmployeeID);
                if (employee == null)
                {
                    return ResultFactory.Fail("Employee not found");
                }
            }
            // Check if Department exists
            var department = _departmentRepo.GetDepartmentById(assetAssignment.DepartmentID);
            if (department == null)
            {
                return ResultFactory.Fail("Department not found");
            }

            _assetAssignmentRepo.UpdateAssetAssignment(assetAssignment);
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

            _assetAssignmentRepo.DeleteAssetAssignment(assetAssignmentId);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message, ex);
        }
    }
}
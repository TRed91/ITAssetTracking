using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Data;
using ITAssetTracking.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Controllers;

[Authorize]
public class SoftwareAssetController : Controller
{
    private readonly ISoftwareAssetService _swaService;
    private readonly IAssetService _assetService;
    private readonly ISoftwareAssetAssignmentService _swaAssignmentService;
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Serilog.ILogger _logger;

    public SoftwareAssetController(
        ISoftwareAssetService softwareAssetService,
        IAssetService assetService,
        ISoftwareAssetAssignmentService softwareAssetAssignmentService,
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        UserManager<ApplicationUser> userManager,
        Serilog.ILogger logger)
    {
        _swaService = softwareAssetService;
        _assetService = assetService;
        _swaAssignmentService = softwareAssetAssignmentService;
        _employeeService = employeeService;
        _departmentService = departmentService;
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Filter()
    {
        var licenseTypes = _swaService.GetLicenseTypes();
        var statuses = _assetService.GetAssetStatuses();
        var manufacturers = _assetService.GetManufacturers();

        if (licenseTypes.Ok && statuses.Ok && statuses.Ok && manufacturers.Ok)
        {
            var model = new SoftwareFilterModel
            {
                LicenseTypes = new SelectList(licenseTypes.Data, "LicenseTypeID", "LicenseTypeName"),
                AssetStatuses = new SelectList(statuses.Data, "AssetStatusID", "AssetStatusName"),
                Manufacturers = new SelectList(manufacturers.Data, "ManufacturerID", "ManufacturerName"),
            };
            
            return View(model);
        }
        
        var errMsg = licenseTypes.Message ?? statuses.Message ?? manufacturers.Message ?? "Unknown Error";
        var ex = licenseTypes.Exception ?? statuses.Exception ?? manufacturers.Exception;
        _logger.Error(ex, $"Filter failed to fetch data: {errMsg}");
        ViewData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
        return RedirectToAction("Index");
    }
    
    [Authorize(Roles = "Admin, SoftwareLicenseManager, Auditor")]
    public IActionResult All(SoftwareFilterModel model)
    {
        // fetch data
        var assetsResult = _swaService.GetSoftwareAssets(model.LicenseTypeId, model.AssetStatusId, model.ManufacturerId, model.IncludeExpired);
        
        if (!assetsResult.Ok)
        {
            _logger.Error(assetsResult.Exception, "Error retrieving data: " + assetsResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, "There was an error retrieving data"));
            return RedirectToAction("Index");
        }

        var licenseTypes = _swaService.GetLicenseTypes();
        var manufacturers = _assetService.GetManufacturers();
        var statuses = _assetService.GetAssetStatuses();
        if (!licenseTypes.Ok || !manufacturers.Ok || !statuses.Ok)
        {
            string errMsg = licenseTypes.Message ?? manufacturers.Message ?? statuses.Message ?? "Unknown Error";
            var ex = licenseTypes.Exception ?? statuses.Exception ?? manufacturers.Exception;
            _logger.Error(ex, $"Error retrieving data: {errMsg}");
            TempData["msg"] = TempDataExtension.Serialize(
                new TempDataMsg(false, "There was an error retrieving data"));
            return RedirectToAction("Index", "Home");
        }

        var assets = assetsResult.Data;

        // filter by search string
        if (!string.IsNullOrEmpty(model.Search))
        {
            assets = assets
                .Where(a => a.LicenseType.LicenseTypeName.ToLower().Contains(model.Search.ToLower()))
                .ToList();
        }

        // order the result
        switch (model.Order)
        {
            case SoftwareAssetsOrder.Manufacturer:
                assets = assets.OrderBy(a => a.Manufacturer.ManufacturerName).ToList();
                break;
            case SoftwareAssetsOrder.AssetStatus:
                assets = assets.OrderBy(a => a.AssetStatus.AssetStatusName).ToList();
                break;
            case SoftwareAssetsOrder.ExpirationDate:
                assets = assets.OrderBy(a => a.ExpirationDate).ToList();
                break;
            default:
                assets = assets.OrderBy(a => a.LicenseType.LicenseTypeName).ToList();
                break;
        }
        
        model.Assets = assets;
        model.LicenseTypes = new SelectList(licenseTypes.Data, "LicenseTypeID", "LicenseTypeName");
        model.Manufacturers = new SelectList(manufacturers.Data, "ManufacturerID", "ManufacturerName");
        model.AssetStatuses = new SelectList(statuses.Data, "AssetStatusID", "AssetStatusName");
        
        return View(model);
    }
    
    [Authorize(Roles = "Admin, SoftwareLicenseManager, Auditor, DepartmentManager")]
    public async Task<IActionResult> DepartmentAssets(DepartmentSoftwareAssetsModel model)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var roles = await _userManager.GetRolesAsync(user);

        int departmentId;
        
        // Enable the Department select list if the user has any of the roles "admin, auditor or softwarelicensemanager"
        model.EnableDepSelectList = roles.Any(r => 
            r == "Admin" || r == "Auditor" || r == "SoftwareLicenseManager");
        
        if (!model.EnableDepSelectList)
        {
            // if access to one department, department id becomes the logged in employee's department id
            var employeeResult = _employeeService.GetEmployeeById(user.EmployeeID);
            if (!employeeResult.Ok)
            {
                _logger.Error(employeeResult.Exception, "Error retrieving data: " + employeeResult.Message);
                TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, employeeResult.Message));
                return RedirectToAction("Index", "Asset");
            }
            departmentId = employeeResult.Data.DepartmentID;
        }
        else
        {
            // if access to all departments default to department id 1
            departmentId = model.DepartmentId;
        }
        var assetsResult = _swaAssignmentService.GetAssignmentsByDepartment(departmentId, false);
        var licenseTypesRes = _swaService.GetLicenseTypes();
        var manufacturersRes = _assetService.GetManufacturers();
        var statusesRes = _assetService.GetAssetStatuses();
        var departmentsRes = _departmentService.GetDepartments();

        // handle data fetching errors
        if (!assetsResult.Ok || !licenseTypesRes.Ok || !manufacturersRes.Ok || !statusesRes.Ok || !departmentsRes.Ok)
        {
            var errMsg = assetsResult.Message ?? 
                         licenseTypesRes.Message ?? 
                         manufacturersRes.Message ?? 
                         statusesRes.Message ?? 
                         departmentsRes.Message ?? "Unknown Error";
            
            var ex = assetsResult.Exception ?? licenseTypesRes.Exception ?? 
                manufacturersRes.Exception ?? statusesRes.Exception ?? departmentsRes.Exception;
            
            _logger.Error(ex, $"Error retrieving data: {errMsg}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, errMsg));
            return RedirectToAction("Index", "SoftwareAsset");
        }
        
        // populate model
        model.DepartmentId = departmentId;
        model.AssignedAssets = assetsResult.Data;
        model.DepartmentSelectList = new SelectList(departmentsRes.Data, "DepartmentID", "DepartmentName");
        model.ManufacturerSelectList = new SelectList(manufacturersRes.Data, "ManufacturerID", "ManufacturerName");
        model.LicenseTypeSelectList = new SelectList(licenseTypesRes.Data, "LicenseTypeID", "LicenseTypeName");
        model.AssetStatusSelectList = new SelectList(statusesRes.Data, "AssetStatusID", "AssetStatusName");

        // filter assets
        if (model.ManufacturerId != 0)
        {
            model.AssignedAssets = model.AssignedAssets
                .Where(a => a.SoftwareAsset.ManufacturerID == model.ManufacturerId)
                .ToList();
        }
        if (model.LicenseTypeId != 0)
        {
            model.AssignedAssets = model.AssignedAssets
                .Where(a => a.SoftwareAssetID == model.LicenseTypeId)
                .ToList();
        }
        if (model.AssetStatusId != 0)
        {
            model.AssignedAssets = model.AssignedAssets
                .Where(a => a.SoftwareAsset.AssetStatusID == model.AssetStatusId)
                .ToList();
        }
        if (!string.IsNullOrEmpty(model.SearchString))
        {
            model.AssignedAssets = model.AssignedAssets
                .Where(a => 
                    a.SoftwareAsset.LicenseType.LicenseTypeName.ToLower().Contains(model.SearchString.ToLower()))
                .ToList();
        }
        
        // order the result
        switch (model.Order)
        {
            case SoftwareAssetsOrder.Manufacturer:
                model.AssignedAssets = model.AssignedAssets
                    .OrderBy(a => a.SoftwareAsset.Manufacturer.ManufacturerName)
                    .ToList();
                break;
            case SoftwareAssetsOrder.AssetStatus:
                model.AssignedAssets = model.AssignedAssets
                    .OrderBy(a => a.SoftwareAsset.AssetStatus.AssetStatusName)
                    .ToList();
                break;
            case SoftwareAssetsOrder.ExpirationDate:
                model.AssignedAssets = model.AssignedAssets
                    .OrderByDescending(a => a.SoftwareAsset.ExpirationDate)
                    .ToList();
                break;
            default:
                model.AssignedAssets = model.AssignedAssets
                    .OrderBy(a => a.SoftwareAsset.LicenseType.LicenseTypeName)
                    .ToList();
                break;
        }
        
        return View(model);
    }

    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult SoftwareSelect(SoftwareSelectModel model)
    {
        var software = _swaService.GetLicenseTypesByManufacturers();
        if (!software.Ok)
        {
            _logger.Error(software.Exception, $"Error retrieving data: {software.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, software.Message));
            return RedirectToAction("Index", "SoftwareAsset");
        }

        var manufacturers = software.Data;
        if (!string.IsNullOrEmpty(model.Search))
        {
            foreach (var m in manufacturers)
            {
                m.LicenseTypes = m.LicenseTypes
                    .Where(l => l.LicenseTypeName.ToLower().Contains(model.Search.ToLower()))
                    .ToList();
            }
            manufacturers = manufacturers
                .Where(l => l.LicenseTypes.Count > 0)
                .ToList();
        }
        
        model.Manufacturers = manufacturers;
        
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Add(int licenseTypeId)
    {
        var licenseType = _swaService.GetLicenseTypeById(licenseTypeId);
        if (!licenseType.Ok)
        {
            _logger.Error(licenseType.Exception, $"Error retrieving data: {licenseType.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, licenseType.Message));
            return RedirectToAction("Index", "SoftwareAsset");
        }

        var model = new SoftwareAssetFormModel();
        model.LicenseTypeId = licenseTypeId;
        model.LicenseName = licenseType.Data.LicenseTypeName;
        model.ManufacturerId = licenseType.Data.ManufacturerID;
        model.ManufacturerName = licenseType.Data.Manufacturer.ManufacturerName;
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Add(SoftwareAssetFormModel model)
    {
        if (!ModelState.IsValid)
        {
            var licenseType = _swaService.GetLicenseTypeById(model.LicenseTypeId);
            model.ManufacturerId = licenseType.Data.ManufacturerID;
            model.ManufacturerName = licenseType.Data.Manufacturer.ManufacturerName;
            model.LicenseTypeId = licenseType.Data.LicenseTypeID;
            model.LicenseName = licenseType.Data.LicenseTypeName;
            return View(model);
        }

        var softwareAsset = model.ToEntity();
        
        var addResult = _swaService.AddSoftwareAsset(softwareAsset);
        if (!addResult.Ok)
        {
            _logger.Error(addResult.Exception, $"Error adding software asset: {addResult.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, addResult.Message));
            return RedirectToAction("Add", new { licenseTypeId = model.LicenseTypeId });
        }

        var successMsg = $"Software asset added with id {softwareAsset.SoftwareAssetID}";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Details", new { assetId = softwareAsset.SoftwareAssetID });
    }

    [HttpGet]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Edit(int assetId)
    {
        var asset = _swaService.GetSoftwareAsset(assetId);
        if (!asset.Ok)
        {
            _logger.Error(asset.Exception, $"Error retrieving data: {asset.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, asset.Message));
            return RedirectToAction("Details", new  { assetId });
        }

        var assetStatuses = _assetService.GetAssetStatuses();
        
        var model = new SoftwareAssetFormModel(asset.Data);
        model.AssetStatuses = new SelectList(assetStatuses.Data, "AssetStatusID", "AssetStatusName");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Edit(int assetId, SoftwareAssetFormModel model)
    {
        if (!ModelState.IsValid)
        {
            var assetStatuses = _assetService.GetAssetStatuses();
            model.AssetStatuses = new SelectList(assetStatuses.Data, "AssetStatusID", "AssetStatusName");
            return View(model);
        }
        var softwareAsset = model.ToEntity();
        softwareAsset.SoftwareAssetID = assetId;
        
        var updateResult = _swaService.UpdateSoftwareAsset(softwareAsset);
        if (!updateResult.Ok)
        {
            _logger.Error(updateResult.Exception, $"Error updating software asset: {updateResult.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, updateResult.Message));
            return RedirectToAction("Details", new { assetId = softwareAsset.SoftwareAssetID });
        }

        var successMsg = $"Software asset with id {assetId} updated";
        _logger.Information(successMsg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, successMsg));
        return RedirectToAction("Details", new { assetId = softwareAsset.SoftwareAssetID });
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult Delete(int assetId)
    {
        var assetResult = _swaService.GetSoftwareAsset(assetId);
        if (!assetResult.Ok)
        {
            _logger.Error(assetResult.Exception, $"Error retrieving asset: {assetResult.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, assetResult.Message));
            return RedirectToAction("Details", new { assetId });
        }
        return View(assetResult.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, SoftwareLicenseManager")]
    public IActionResult DeleteAsset(int assetId)
    {
        var deleteResult = _swaService.DeleteSoftwareAsset(assetId);
        if (!deleteResult.Ok)
        {
            _logger.Error(deleteResult.Exception, $"Error deleting asset: {deleteResult.Message}");
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, deleteResult.Message));
            return RedirectToAction("Details", new { assetId });
        }
        
        var msg = $"Software Asset with id {assetId} deleted successfully";
        _logger.Information(msg);
        TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(true, msg));
        return RedirectToAction("Index");
    }
    
    public IActionResult Details(int assetId)
    {
        var assetResult = _swaService.GetSoftwareAsset(assetId);
        
        if (!assetResult.Ok)
        {
            _logger.Error(assetResult.Exception, "Error retrieving data: " + assetResult.Message);
            TempData["msg"] = TempDataExtension.Serialize(new TempDataMsg(false, assetResult.Message));
            return RedirectToAction("Index", "Asset");
        }
        
        var model = new SoftwareAssetDetailsModel(assetResult.Data);
        return View(model);
    }
}
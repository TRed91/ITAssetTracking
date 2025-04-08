using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class ReportsModel
{
    [Display(Name = "From")]
    [DataType(DataType.Date)]
    public DateTime FromDate { get; set; } = DateTime.Today.Subtract(TimeSpan.FromDays(90));
    [Display(Name = "To")]
    [DataType(DataType.Date)]
    public DateTime ToDate { get; set; } = DateTime.Today;
    [Display(Name = "Department")] 
    public byte DepartmentId { get; set; } = 0;
    
    [Display(Name = "Asset Type")] 
    public byte AssetTypeId { get; set; } = 0;
    [Display(Name = "License Type")] 
    public byte LicenseTypeId { get; set; } = 0;
    
    public SelectList? Departments { get; set; }
    public SelectList? AssetTypes { get; set; }
    public SelectList? LicenseTypes { get; set; }
    public SelectList? OrderSelectList { get; set; }
}

public class AssetDistributionReportModel : ReportsModel
{
    public AssetDistributionOrder Order { get; set; } = AssetDistributionOrder.AssetType;
    public List<AssetDistributionReport>? AssetDistributionReports { get; set; }

    public AssetDistributionReportModel()
    {
        OrderSelectList = GetOrderSelectList();
    }

    private SelectList GetOrderSelectList()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Asset Type" },
            new SelectListItem { Value = "2", Text = "Number Of Assets" }
        };
        return new SelectList(items, "Value", "Text");
    }
}

public class AssetStatusReportModel : ReportsModel
{
    public AssetStatusOrder Order { get; set; } = AssetStatusOrder.AssetType;
    
    public List<AssetStatusReport>? AssetStatusReports { get; set; }

    public AssetStatusReportModel()
    {
        OrderSelectList = GetOrderSelectList();
    }

    private SelectList GetOrderSelectList()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Asset Type" },
            new SelectListItem { Value = "2", Text = "Total Assets" },
            new SelectListItem { Value = "3", Text = "In Use" },
            new SelectListItem { Value = "4", Text = "Storage" },
            new SelectListItem { Value = "5", Text = "Repair" }
        };
        return new SelectList(items, "Value", "Text");
    }
}

public class AssetValueReportModel : ReportsModel
{
    public AssetValueOrder Order { get; set; } = AssetValueOrder.AssetType;
    
    public AssetValuesReport? AssetValuesReport { get; set; }
    
    public AssetValueReportModel()
    {
        OrderSelectList = GetOrderSelectList();
    }

    private SelectList GetOrderSelectList()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Asset Type" },
            new SelectListItem { Value = "2", Text = "Number Of Assets" },
            new SelectListItem { Value = "3", Text = "Value" }
        };
        return new SelectList(items, "Value", "Text");
    }
}

public class SoftwareDistributionReportModel : ReportsModel
{
    public SoftwareDistributionOrder Order { get; set; } = SoftwareDistributionOrder.LicenseType;
    public List<SoftwareAssetDistributionReport>? SoftwareDistributionReports { get; set; }

    public SoftwareDistributionReportModel()
    {
        OrderSelectList = GetOrderSelectList();
    }

    private SelectList GetOrderSelectList()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "License Type" },
            new SelectListItem { Value = "2", Text = "Number Of Licenses" }
        };
        return new SelectList(items, "Value", "Text");
    }
}

public class SoftwareStatusReportModel : ReportsModel
{
    public SoftwareStatusOrder Order { get; set; } = SoftwareStatusOrder.LicenseType;
    
    public List<AssetStatusReport>? SoftwareStatusReports { get; set; }

    public SoftwareStatusReportModel()
    {
        OrderSelectList = GetOrderSelectList();
    }

    private SelectList GetOrderSelectList()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "License Type" },
            new SelectListItem { Value = "2", Text = "Total Licenses" },
            new SelectListItem { Value = "3", Text = "In Use" },
            new SelectListItem { Value = "4", Text = "Storage" },
            new SelectListItem { Value = "5", Text = "Repair" }
        };
        return new SelectList(items, "Value", "Text");
    }
}

public class TicketReportModel : ReportsModel
{
    public byte TicketTypeId { get; set; } = 0;
    public SelectList? TicketTypes { get; set; }
    
    public TicketsReport? Report { get; set; }
}

public enum AssetDistributionOrder
{
    AssetType = 1,
    NumberOfAssets
}

public enum AssetStatusOrder
{
    AssetType = 1,
    TotalAssets,
    InUse,
    Storage,
    Repair
}

public enum SoftwareStatusOrder
{
    LicenseType = 1,
    TotalLicenses,
    InUse,
    Storage,
    Repair
}

public enum AssetValueOrder
{
    AssetType = 1,
    NumberOfAssets,
    Value
}

public enum SoftwareDistributionOrder
{
    LicenseType = 1,
    NumberOfLicenses
}
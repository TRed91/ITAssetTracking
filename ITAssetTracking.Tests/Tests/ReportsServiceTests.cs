using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Tests.MockRepos;
using NUnit.Framework;

namespace ITAssetTracking.Tests.Tests;

[TestFixture]
public class ReportsServiceTests
{
    private readonly ReportsService _service = new ReportsService(
        new MockReportsRepo(), new MockDepartmentRepo(), new MockAssetRepo(), new MockSoftwareAssetRepo(), new MockTicketRepo());
    
    [Test]
    public void Creates_AssetDistributionReport_Correctly()
    {
        var starDate = new DateTime(2025, 02, 01);
        var endDate = new DateTime(2025, 02, 03);
        var reportResult = _service.GetAssetDistributionReport(starDate, endDate);
        var report = reportResult.Data;
        
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.Count, Is.GreaterThan(0));
        Assert.That(report[0].DepartmentName, Is.EqualTo("Department 1"));
        Assert.That(report[0].Items.Count(), Is.EqualTo(3));
        Assert.That(report[0].Items[0].AssetTypeName, Is.EqualTo("Monitor"));
        Assert.That(report[0].Items[0].NumberOfAssets, Is.EqualTo(1));
        Assert.That(report[0].Items[1].NumberOfAssets, Is.EqualTo(1));
        Assert.That(report[0].Items[2].NumberOfAssets, Is.EqualTo(0));
        Assert.That(report[1].Items[0].NumberOfAssets, Is.EqualTo(1));
        Assert.That(report[1].Items[1].NumberOfAssets, Is.EqualTo(0));
        Assert.That(report[1].Items[2].NumberOfAssets, Is.EqualTo(0));
    }

    [Test]
    public void Fails_InvalidTimeframe()
    {
        var starDate = new DateTime(2025, 02, 04);
        var endDate = new DateTime(2025, 02, 03);
        
        var reportResult = _service.GetAssetDistributionReport(starDate, endDate);
        
        Assert.That(reportResult.Ok, Is.False);
        Assert.That(reportResult.Message, Is.EqualTo("Start date cannot be after end date"));
    }

    [Test]
    public void Creates_AssetDistributionReport_WithDepartmentId()
    {
        var starDate = new DateTime(2025, 02, 01);
        var endDate = new DateTime(2025, 02, 03);
        
        var reportResult = _service.GetAssetDistributionReport(starDate, endDate, 1);
        var report = reportResult.Data;
        
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.Count, Is.EqualTo(1));
        Assert.That(report[0].DepartmentName, Is.EqualTo("Department 1"));
    }
    
    [Test]
    public void Creates_AssetDistributionReport_WithDepartmentId_WithAssetTypeId()
    {
        var starDate = new DateTime(2025, 02, 01);
        var endDate = new DateTime(2025, 02, 03);
        
        var reportResult = _service.GetAssetDistributionReport(starDate, endDate, 1, 2);
        var report = reportResult.Data;
        
        Assert.That(reportResult.Exception, Is.Null);
        Assert.That(reportResult.Message, Is.EqualTo(""));
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.Count, Is.EqualTo(1));
        Assert.That(report[0].Items.Count, Is.EqualTo(1));
        Assert.That(report[0].Items[0].AssetTypeName, Is.EqualTo("Keyboard"));
        Assert.That(report[0].Items[0].NumberOfAssets, Is.EqualTo(1));
    }

    [Test]
    public void Creates_AssetStatusReport_Correctly()
    {
        var starDate = new DateTime(2025, 02, 01);
        var endDate = new DateTime(2025, 02, 03);
        
        var reportResult = _service.GetAssetStatusReport(starDate, endDate);
        
        var report = reportResult.Data;
        
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.Count, Is.EqualTo(3));
        Assert.That(report[0].AssetTypeName, Is.EqualTo("Monitor"));
        Assert.That(report[0].NumberOfAssetsTotal, Is.EqualTo(2));
        Assert.That(report[0].NumberOfAssetsInUse, Is.EqualTo(1));
        Assert.That(report[0].NumberOfAssetsStorage, Is.EqualTo(0));
        Assert.That(report[0].NumberOfAssetsRepair, Is.EqualTo(1));
        Assert.That(report[1].AssetTypeName, Is.EqualTo("Keyboard"));
        Assert.That(report[1].NumberOfAssetsTotal, Is.EqualTo(1));
        Assert.That(report[1].NumberOfAssetsInUse, Is.EqualTo(0));
        Assert.That(report[1].NumberOfAssetsStorage, Is.EqualTo(1));
        Assert.That(report[1].NumberOfAssetsRepair, Is.EqualTo(0));
        Assert.That(report[2].AssetTypeName, Is.EqualTo("Mouse"));
        Assert.That(report[2].NumberOfAssetsTotal, Is.EqualTo(0));
        Assert.That(report[2].NumberOfAssetsInUse, Is.EqualTo(0));
        Assert.That(report[2].NumberOfAssetsStorage, Is.EqualTo(0));
        Assert.That(report[2].NumberOfAssetsRepair, Is.EqualTo(0));
    }
    
    [Test]
    public void Creates_AssetStatusReport_WithDepartmentId_Correctly()
    {
        var starDate = new DateTime(2025, 02, 01);
        var endDate = new DateTime(2025, 02, 03);
        
        var reportResult = _service.GetAssetStatusReport(starDate, endDate, 1);
        
        var report = reportResult.Data;
        
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.Count, Is.EqualTo(3));
        Assert.That(report[0].AssetTypeName, Is.EqualTo("Monitor"));
        Assert.That(report[0].NumberOfAssetsTotal, Is.EqualTo(1));
        Assert.That(report[0].NumberOfAssetsInUse, Is.EqualTo(1));
        Assert.That(report[0].NumberOfAssetsStorage, Is.EqualTo(0));
        Assert.That(report[0].NumberOfAssetsRepair, Is.EqualTo(0));
        Assert.That(report[1].AssetTypeName, Is.EqualTo("Keyboard"));
        Assert.That(report[1].NumberOfAssetsTotal, Is.EqualTo(1));
        Assert.That(report[1].NumberOfAssetsInUse, Is.EqualTo(0));
        Assert.That(report[1].NumberOfAssetsStorage, Is.EqualTo(1));
        Assert.That(report[1].NumberOfAssetsRepair, Is.EqualTo(0));
        Assert.That(report[2].AssetTypeName, Is.EqualTo("Mouse"));
        Assert.That(report[2].NumberOfAssetsTotal, Is.EqualTo(0));
        Assert.That(report[2].NumberOfAssetsInUse, Is.EqualTo(0));
        Assert.That(report[2].NumberOfAssetsStorage, Is.EqualTo(0));
        Assert.That(report[2].NumberOfAssetsRepair, Is.EqualTo(0));
    }
    
    [Test]
    public void Creates_AssetStatusReport_WithDepartmentId_WithAssetTypeId_Correctly()
    {
        var starDate = new DateTime(2025, 02, 01);
        var endDate = new DateTime(2025, 02, 03);
        
        var reportResult = _service.GetAssetStatusReport(starDate, endDate, 1, 1);
        
        var report = reportResult.Data;
        
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.Count, Is.EqualTo(1));
        Assert.That(report[0].AssetTypeName, Is.EqualTo("Monitor"));
        Assert.That(report[0].NumberOfAssetsTotal, Is.EqualTo(1));
        Assert.That(report[0].NumberOfAssetsInUse, Is.EqualTo(1));
        Assert.That(report[0].NumberOfAssetsStorage, Is.EqualTo(0));
        Assert.That(report[0].NumberOfAssetsRepair, Is.EqualTo(0));
    }

    [Test]
    public void Creates_AssetValuesReport_Correctly()
    {
        var starDate = new DateTime(2025, 01, 01);
        var endDate = new DateTime(2025, 01, 03);
        var reportResult = _service.GetAssetValuesReport(starDate, endDate);
        var report = reportResult.Data;
        
        Assert.That(reportResult.Exception, Is.Null);
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.TotalValue, Is.EqualTo(600.00m));
        Assert.That(report.Items.Count, Is.EqualTo(3));
        Assert.That(report.Items[0].AssetTypeName, Is.EqualTo("Monitor"));
        Assert.That(report.Items[0].NumberOfAssets, Is.EqualTo(2));
        Assert.That(report.Items[0].AssetsValue, Is.EqualTo(400.00m));
    }
    
    [Test]
    public void Creates_SoftwareAssetDistributionReport_Correctly()
    {
        var starDate = new DateTime(2025, 02, 01);
        var endDate = new DateTime(2025, 02, 08);
        var reportResult = _service.GetSoftwareAssetDistributionReport(starDate, endDate);
        var report = reportResult.Data;
        
        Assert.That(reportResult.Exception, Is.Null);
        Assert.That(reportResult.Message, Is.Empty);
        Assert.That(reportResult.Ok, Is.True);
        Assert.That(report.Count, Is.GreaterThan(0));
        Assert.That(report[0].DepartmentName, Is.EqualTo("Department 1"));
        Assert.That(report[0].Items.Count, Is.EqualTo(3));
        Assert.That(report[0].Items[0].LicenseTypeName, Is.EqualTo("License Type 1"));
        Assert.That(report[0].Items[1].NumberOfLicenses, Is.EqualTo(2));
    }

    [Test]
    public void Creates_TicketReport_Correctly()
    {
        var starDate = new DateTime(2025, 02, 9);
        var endDate = new DateTime(2025, 02, 20);
        
        var result = _service.GetTicketsReport(starDate, endDate);
        var report = result.Data;
        
        Assert.That(result.Exception, Is.Null);
        Assert.That(result.Message, Is.Empty);
        Assert.That(result.Ok, Is.True);
        Assert.That(report.TotalTickets, Is.EqualTo(4));
        Assert.That(report.ReportsList.Count, Is.EqualTo(4));
        Assert.That(report.ReportsList[0].TicketTypeName, Is.EqualTo("Issue"));
        Assert.That(report.ReportsList[0].NumberOfTickets, Is.EqualTo(2));
        Assert.That(report.ReportsList[0].CompletedTickets, Is.EqualTo(2));
        Assert.That(report.ReportsList[0].CancelledTickets, Is.EqualTo(0));
        Assert.That(report.ReportsList[0].UserErrorTickets, Is.EqualTo(0));
        Assert.That(report.ReportsList[0].OtherTickets, Is.EqualTo(0));
        Assert.That(report.ReportsList[1].CancelledTickets, Is.EqualTo(1));
        Assert.That(report.ReportsList[0].AvgResolutionTimeInDays, Is.EqualTo(6));
    }
}
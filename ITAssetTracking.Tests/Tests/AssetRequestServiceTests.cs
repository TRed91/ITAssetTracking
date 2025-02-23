using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Data.Repositories;
using ITAssetTracking.Tests.MockRepos;
using NUnit.Framework;

namespace ITAssetTracking.Tests.Tests;

[TestFixture]
public class AssetRequestServiceTests
{
    private AssetRequestService GetService()
    {
        return new AssetRequestService(
            new MockAssetRequestRepo(), 
            new MockAssetRepo(), 
            new MockEmployeeRepo(), 
            new MockDepartmentRepo());
    }

    [Test]
    public void AddAssetRequest_Success()
    {
        var service = GetService();

        var newRequest = new AssetRequest
        {
            AssetID = 3,
            DepartmentID = 1,
            EmployeeID = 1,
            RequestDate = DateTime.Now,
        };
        
        var result = service.AddAssetRequest(newRequest);
        Assert.That(result.Ok, Is.True);
    }
    
    [Test]
    public void AddAssetRequest_Success_NullEmployeeID()
    {
        var service = GetService();

        var newRequest = new AssetRequest
        {
            AssetID = 3,
            DepartmentID = 1,
            EmployeeID = null,
            RequestDate = DateTime.Now,
        };
        
        var result = service.AddAssetRequest(newRequest);
        Assert.That(result.Ok, Is.True);
    }

    [Test]
    public void AddAssetRequest_Fail_EmployeeDepartmentNoMatch()
    {
        var service = GetService();
        
        var newRequest = new AssetRequest
        {
            AssetID = 3,
            DepartmentID = 1,
            EmployeeID = 3,
            RequestDate = DateTime.Now,
        };
        
        var result = service.AddAssetRequest(newRequest);
        
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Employee is not assigned to the requested department"));
    }
}
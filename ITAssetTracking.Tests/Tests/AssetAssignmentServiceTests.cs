using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Data.Repositories;
using ITAssetTracking.Tests.MockRepos;
using NUnit.Framework;

namespace ITAssetTracking.Tests.Tests;

[TestFixture]
public class AssetAssignmentServiceTests
{
    private AssetAssignmentService GetService()
    {
         return new AssetAssignmentService(
            new MockAssetAssignmentRepo(), 
            new MockEmployeeRepo(),
            new MockDepartmentRepo());
    }
    [Test]
    public void AddsAssignment_Success()
    {
        var service = GetService();

        var newAssignment = new AssetAssignment
        {
            AssetID = 2,
            DepartmentID = 1,
            EmployeeID = 1,
        };
        
        var result = service.AddAssetAssignment(newAssignment);
        
        Assert.That(result.Ok, Is.True);
        Assert.That(newAssignment.AssetAssignmentID, Is.EqualTo(4));
    }

    [Test]
    public void NewAssetAssignment_Fail_AssetInUse()
    {
        var service = GetService();

        var newAssignment = new AssetAssignment
        {
            AssetID = 1,
            DepartmentID = 2,
            EmployeeID = 3,
        };
        var result = service.AddAssetAssignment(newAssignment);
        
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Asset is currently in use"));
    }

    [Test]
    public void NewAssetAssignment_Fail_EmployeeNotFound()
    {
        var service = GetService();
        
        var newAssignment = new AssetAssignment
        {
            AssetID = 2,
            DepartmentID = 1,
            EmployeeID = 4,
        };
        var result = service.AddAssetAssignment(newAssignment);
        
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Employee not found"));
    }

    [Test]
    public void NewAssetAssignment_Fail_DepartmentNotFound()
    {
        var service = GetService();
        
        var newAssignment = new AssetAssignment
        {
            AssetID = 2,
            DepartmentID = 3,
            EmployeeID = 3,
        };
        var result = service.AddAssetAssignment(newAssignment);
        
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Department not found"));
    }

    [Test]
    public void UpdateAssetAssignment_Success()
    {
        var service = GetService();

        var updatedAssignmentResult = service.GetAssetAssignmentById(1);
        var updatedAssignment = updatedAssignmentResult.Data;
        updatedAssignment.DepartmentID = 2;
        updatedAssignment.EmployeeID = 3;
        updatedAssignment.ReturnDate = DateTime.Now;
        
        var result = service.UpdateAssetAssignment(updatedAssignment);
        
        Assert.That(result.Ok, Is.True);
    }

    [Test]
    public void UpdateAssetAssignment_Fail_AssignmentNotFound()
    {
        var service = GetService();

        var updatedAssignmentResult = new AssetAssignment
        {
            AssetAssignmentID = 4
        };
        
        var result = service.UpdateAssetAssignment(updatedAssignmentResult);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Asset assignment not found"));
    }

    [Test]
    public void UpdateAssetAssignment_Fail_AssetInUse()
    {
        var service = GetService();
        
        var assignment = service.GetAssetAssignmentById(1).Data;
        assignment.AssetID = 3;
        
        var result = service.UpdateAssetAssignment(assignment);
        
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Asset is currently in use"));
    }
}
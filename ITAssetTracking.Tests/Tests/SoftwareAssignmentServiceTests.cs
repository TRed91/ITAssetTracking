using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Tests.MockRepos;
using NUnit.Framework;

namespace ITAssetTracking.Tests.Tests;

[TestFixture]
public class SoftwareAssignmentServiceTests
{
    private ISoftwareAssetAssignmentService GetService()
    {
        return new SoftwareAssetAssignmentService(
            new MockSoftwareAssignmentRepo(), 
            new MockSoftwareAssetRepo(), 
            new MockAssetRepo(), 
            new MockEmployeeRepo(),
            new MockAssetAssignmentRepo());
    }
    
    [Test]
    public void AddsSoftwareAssignment_Success()
    {
        var service = GetService();

        var newAssignment = new SoftwareAssetAssignment
        {
            EmployeeID = 1,
            AssetID = 1,
            SoftwareAssetID = 2,
        };
        
        var result = service.AddSoftwareAssetAssignment(newAssignment);
        Assert.That(result.Ok, Is.True);
        Assert.That(newAssignment.AssetAssignmentID, Is.EqualTo(4));
        Assert.That(newAssignment.AssignmentDate.Date, Is.EqualTo(DateTime.Today));
    }

    [Test]
    public void AddsSoftwareAssignment_Fail_EmployeeAndAssetNull()
    {
        var service = GetService();
        var newAssignment = new SoftwareAssetAssignment
        {
            SoftwareAssetID = 2,
        };
        var result = service.AddSoftwareAssetAssignment(newAssignment);
        
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Asset Id or Employee Id is required"));
    }

    [Test]
    public void AddsSoftwareAssignment_Fail_SoftwareInUse()
    {
        var service = GetService();
        var newAssignment = new SoftwareAssetAssignment
        {
            EmployeeID = 1,
            AssetID = 1,
            SoftwareAssetID = 1,
        };
        var result = service.AddSoftwareAssetAssignment(newAssignment);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Software asset is already assigned"));
    }

    [Test]
    public void AddsSoftwareAssignment_Fail_EmployeeNotFound()
    {
        var service = GetService();
        var newAssignment = new SoftwareAssetAssignment
        {
            EmployeeID = 4,
            AssetID = 1,
            SoftwareAssetID = 2,
        };
        var result = service.AddSoftwareAssetAssignment(newAssignment);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Employee not found"));
    }

    [Test]
    public void AddsSoftwareAssignment_Fail_AssetNotFound()
    {
        var service = GetService();
        var newAssignment = new SoftwareAssetAssignment
        {
            EmployeeID = 1,
            AssetID = 4,
            SoftwareAssetID = 2,
        };
        var result = service.AddSoftwareAssetAssignment(newAssignment);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Asset not found"));
    }
    
    [Test]
    public void AddsSoftwareAssignment_Fail_SoftwareNotFound()
    {
        var service = GetService();
        var newAssignment = new SoftwareAssetAssignment
        {
            EmployeeID = 1,
            AssetID = 1,
            SoftwareAssetID = 4,
        };
        var result = service.AddSoftwareAssetAssignment(newAssignment);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Software asset not found"));
    }

    [Test]
    public void UpdatesSoftwareAssignment_Success()
    {
        var service = GetService();
        var assignment = new SoftwareAssetAssignment
        {
            AssetAssignmentID = 1, AssetID = 1, EmployeeID = 1, SoftwareAssetID = 2,
            AssignmentDate = new DateTime(2025, 02, 05), ReturnDate = DateTime.Today
        };
        
        var result = service.UpdateSoftwareAssetAssignment(assignment);
        var updatedAssignment = service.GetSoftwareAssetAssignment(assignment.AssetAssignmentID).Data;
        
        Assert.That(result.Ok, Is.True);
        Assert.That(updatedAssignment.ReturnDate, Is.EqualTo(DateTime.Today));
    }

    [Test]
    public void UpdatesSoftwareAssignment_Fail_AlreadyAssigned()
    {
        var service = GetService();
        var assignment = new SoftwareAssetAssignment
        {
            AssetAssignmentID = 1, AssetID = 1, EmployeeID = 1, SoftwareAssetID = 3,
            AssignmentDate = new DateTime(2025, 02, 05), ReturnDate = null
        };
        var result = service.UpdateSoftwareAssetAssignment(assignment);
        
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("Software asset is already assigned"));
    }
}
using ITAssetTracking.App.Services;
using ITAssetTracking.Tests.MockRepos;
using NUnit.Framework;

namespace ITAssetTracking.Tests.Tests;

[TestFixture]
public class EmployeeServiceTests
{
    private EmployeeService _service = new EmployeeService(new MockEmployeeRepo(), new MockDepartmentRepo());
    
    [Test]
    public void GeneratesPassword_Correctly()
    {
        string lastName = "Tester";
        var passwordResult = _service.GeneratePassword(lastName);
        
        Assert.That(passwordResult.Ok, Is.True);
        Assert.That(passwordResult.Data.Length, Is.EqualTo(11));
        Assert.That(passwordResult.Data, Does.StartWith("!Tester"));
    }

    [Test]
    public void GeneratesPassword_LastNameWith2Characters()
    {
        string lastName = "Te";
        var passwordResult = _service.GeneratePassword(lastName);
        
        Assert.That(passwordResult.Ok, Is.True);
        Assert.That(passwordResult.Data.Length, Is.EqualTo(8));
        Assert.That(passwordResult.Data, Does.StartWith("!Te"));
    }

    [Test]
    public void GeneratesPassword_Fail_LastNameWith1Character()
    {
        string lastName = "T";
        var passwordResult = _service.GeneratePassword(lastName);
        
        Assert.That(passwordResult.Ok, Is.False);
        Assert.That(passwordResult.Message, Is.EqualTo("Last name must be at least 2 characters"));
    }
}
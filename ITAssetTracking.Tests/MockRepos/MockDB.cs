using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.Tests.MockRepos;

public class MockDB
{
    public List<Asset> Assets = new List<Asset>
    {
        new Asset
        {
            AssetID = 1, AssetStatusID = 1, AssetTypeID = 1, LocationID = 1, ManufacturerID = 1, ModelID = 1,
            PurchaseDate = new DateTime(2025, 01, 02), PurchasePrice = 100.00m, SerialNumber = "111-111"
        },
        new Asset
        {
            AssetID = 2, AssetStatusID = 2, AssetTypeID = 2, LocationID = 2, ManufacturerID = 1, ModelID = 1,
            PurchaseDate = new DateTime(2025, 01, 02), PurchasePrice = 200.00m, SerialNumber = "222-222"
        },
        new Asset
        {
            AssetID = 3, AssetStatusID = 1, AssetTypeID = 1, LocationID = 1, ManufacturerID = 3, ModelID = 3,
            PurchaseDate = new DateTime(2025, 01, 02), PurchasePrice = 300.00m, SerialNumber = "333-333"
        }
    };

    public List<SoftwareAsset> SoftwareAssets = new List<SoftwareAsset>
    {
        new SoftwareAsset
        {
            SoftwareAssetID = 1, AssetStatusID = 1, ManufacturerID = 1, LicenseTypeID = 1,
            LicenseKey = "1111-1111-1111", NumberOfLicenses = 1, Version = "1.00.1",
            ExpirationDate = new DateTime(2026, 01, 01)
        },
        new SoftwareAsset
        {
            SoftwareAssetID = 2, AssetStatusID = 2, ManufacturerID = 2, LicenseTypeID = 2,
            LicenseKey = "2222-2222-2222", NumberOfLicenses = 2, Version = "2.00.2",
            ExpirationDate = new DateTime(2026, 02, 02)
        },
        new SoftwareAsset
        {
            SoftwareAssetID = 3, AssetStatusID = 1, ManufacturerID = 3, LicenseTypeID = 3,
            LicenseKey = "3333-3333-3333", NumberOfLicenses = 3, Version = "3.00.3",
            ExpirationDate = new DateTime(2024, 03, 03)
        },
    };

    public List<AssetAssignment> AssetAssignments = new List<AssetAssignment>
    {
        new AssetAssignment
        {
            AssetAssignmentID = 1, AssetID = 1, DepartmentID = 1, EmployeeID = 1,
            AssignmentDate = new DateTime(2025, 02, 02), ReturnDate = null
        },
        new AssetAssignment
        {
            AssetAssignmentID = 2, AssetID = 2, DepartmentID = 1, EmployeeID = 2,
            AssignmentDate = new DateTime(2025, 02, 02), ReturnDate = new DateTime(2025, 02, 09)
        },
        new AssetAssignment
        {
            AssetAssignmentID = 3, AssetID = 3, DepartmentID = 2, EmployeeID = 3,
            AssignmentDate = new DateTime(2025, 02, 02), ReturnDate = null
        },
    };

    public List<AssetRequest> AssetRequests = new List<AssetRequest>
    {
        new AssetRequest
        {
            AssetRequestID = 1, AssetID = 1, DepartmentID = 1, EmployeeID = 1,
            RequestDate = new DateTime(2025, 02, 15), RequestResultID = null, RequestNote = null
        },
        new AssetRequest
        {
            AssetRequestID = 2, AssetID = 2, DepartmentID = 1, EmployeeID = 1,
            RequestDate = new DateTime(2025, 02, 10), RequestResultID = 2, RequestNote = "Not for you"
        },
        new AssetRequest
        {
            AssetRequestID = 3, AssetID = 3, DepartmentID = 1, EmployeeID = 2,
            RequestDate = new DateTime(2025, 02, 10), RequestResultID = 1, RequestNote = null
        },
    };

    public List<SoftwareAssetAssignment> SoftwareAssetAssignments = new List<SoftwareAssetAssignment>
    {
        new SoftwareAssetAssignment
        {
            AssetAssignmentID = 1, AssetID = 1, EmployeeID = 1, SoftwareAssetID = 1,
            AssignmentDate = new DateTime(2025, 02, 05), ReturnDate = null
        },
        new SoftwareAssetAssignment
        {
            AssetAssignmentID = 2, AssetID = null, EmployeeID = 2, SoftwareAssetID = 2,
            AssignmentDate = new DateTime(2025, 02, 05), 
            ReturnDate = new DateTime(2025, 02, 12)
        },
        new SoftwareAssetAssignment
        {
            AssetAssignmentID = 3, AssetID = 2, EmployeeID = null, SoftwareAssetID = 3,
            AssignmentDate = new DateTime(2025, 02, 05), ReturnDate = null
        }
    };

    public List<SoftwareAssetRequest> SoftwareAssetRequests = new List<SoftwareAssetRequest>
    {
        new SoftwareAssetRequest
        {
            SoftwareAssetRequestID = 1, AssetID = 1, EmployeeID = 1, SoftwareAssetID = 1,
            RequestDate = new DateTime(2025, 02, 05), RequestResultID = null, RequestNote = null
        },
        new SoftwareAssetRequest
        {
            SoftwareAssetRequestID = 2, AssetID = 2, EmployeeID = null, SoftwareAssetID = 1,
            RequestDate = new DateTime(2025, 02, 05), RequestResultID = 1, RequestNote = null
        },
        new SoftwareAssetRequest
        {
            SoftwareAssetRequestID = 3, AssetID = null, EmployeeID = 2, SoftwareAssetID = 2,
            RequestDate = new DateTime(2025, 02, 05), RequestResultID = 2, RequestNote = "Nope"
        },
    };

    public List<Department> Departments = new List<Department>
    {
        new Department { DepartmentID = 1, DepartmentName = "Department 1" },
        new Department { DepartmentID = 2, DepartmentName = "Department 2" },
    };

    public List<Employee> Employees = new List<Employee>
    {
        new Employee { EmployeeID = 1, DepartmentID = 1, FirstName = "John", LastName = "Doe" },
        new Employee { EmployeeID = 2, DepartmentID = 1, FirstName = "Jane", LastName = "Doe" },
        new Employee { EmployeeID = 3, DepartmentID = 2, FirstName = "Emily", LastName = "Brown" },
    };

    public List<LicenseType> LicenseTypes = new List<LicenseType>
    {
        new LicenseType { LicenseTypeID = 1, LicenseTypeName = "License Type 1", ManufacturerID = 1 },
        new LicenseType { LicenseTypeID = 2, LicenseTypeName = "License Type 2", ManufacturerID = 2 },
        new LicenseType { LicenseTypeID = 3, LicenseTypeName = "License Type 3", ManufacturerID = 3 },
    };

    public List<Location> Locations = new List<Location>
    {
        new Location { LocationID = 1, LocationName = "Location 1" },
        new Location { LocationID = 2, LocationName = "Location 2" },
    };
}
using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class AssignDepartmentModel
{
    public int DepartmentID { get; set; }
    
    public long AssetID { get; set; }
    public string SerialNumber { get; set; }
    public List<Department> Departments { get; set; }
}
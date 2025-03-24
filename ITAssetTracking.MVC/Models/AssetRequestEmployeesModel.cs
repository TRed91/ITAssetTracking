using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class AssetRequestEmployeesModel
{
    public int DepartmentID { get; set; }
    public string DepartmentName { get; set; }
    public long AssetId { get; set; }
    public string SerialNumber { get; set; }
    public int? EmployeeId { get; set; }
    
    public char? StartingLetter { get; set; }
    public SelectList? StartingLetters { get; set; } = GetLetterSelectList();
    
    public List<Employee> Employees { get; set; } = new List<Employee>();
    
    private static SelectList GetLetterSelectList()
    {
        var list = new List<SelectListItem>
        {
            new SelectListItem("A", "A"),
            new SelectListItem("B", "B"),
            new SelectListItem("C", "C"),
            new SelectListItem("D", "D"),
            new SelectListItem("E", "E"),
            new SelectListItem("F", "F"),
            new SelectListItem("G", "G"),
            new SelectListItem("H", "H"),
            new SelectListItem("I", "I"),
            new SelectListItem("J", "J"),
            new SelectListItem("K", "K"),
            new SelectListItem("L", "L"),
            new SelectListItem("M", "M"),
            new SelectListItem("N", "N"),
            new SelectListItem("O", "O"),
            new SelectListItem("P", "P"),
            new SelectListItem("Q", "Q"),
            new SelectListItem("R", "R"),
            new SelectListItem("S", "S"),
            new SelectListItem("T", "T"),
            new SelectListItem("U", "U"),
            new SelectListItem("V", "V"),
            new SelectListItem("W", "W"),
            new SelectListItem("X", "X"),
            new SelectListItem("Y", "Y"),
            new SelectListItem("Z", "Z"),
        };
        return new SelectList(list, "Value", "Text");
    }
}
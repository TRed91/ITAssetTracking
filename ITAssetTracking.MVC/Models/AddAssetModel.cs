using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class AddAssetModel : IValidatableObject
{
    public string? Manufacturer { get; set; }
    public SelectList? Models { get; set; }
    public SelectList? AssetTypes { get; set; }
    public SelectList? Locations { get; set; }
    public int ManufacturerId { get; set; }
    
    [Required]
    [Display(Name = "Model")]
    public int ModelId { get; set; }
    [Required]
    [Display(Name = "Asset Type")]
    public byte AssetTypeId { get; set; }
    [Required]
    [Display(Name = "Location")]
    public byte LocationId { get; set; }
    [Required]
    [Display(Name = "Serial Number")]
    public string SerialNumber { get; set; }

    [Required]
    [Display(Name = "Purchase Date")]
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    [Required]
    [Display(Name = "Purchase Price")]
    [DataType(DataType.Currency)]
    public decimal PurchasePrice { get; set; }

    public Asset ToEntity()
    {
        return new Asset
        {
            AssetTypeID = AssetTypeId,
            ManufacturerID = ManufacturerId,
            ModelID = ModelId,
            LocationID = LocationId,
            SerialNumber = SerialNumber,
            PurchaseDate = PurchaseDate,
            PurchasePrice = PurchasePrice
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        if (ModelId == 0)
        {
            errors.Add(new ValidationResult("Model is required",  ["ModelId"] ));
        }

        if (AssetTypeId == 0)
        {
            errors.Add(new ValidationResult("Asset Type is required",  ["AssetTypeId"] ));
        }

        if (LocationId == 0)
        {
            errors.Add(new ValidationResult("Location is required",  ["LocationId"] ));
        }
        
        return errors;
    }
}
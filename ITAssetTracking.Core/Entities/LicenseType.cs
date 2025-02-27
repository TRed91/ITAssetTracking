﻿namespace ITAssetTracking.Core.Entities;

public class LicenseType
{
    public int LicenseTypeID { get; set; }
    public int ManufacturerID { get; set; }
    public string LicenseTypeName { get; set; }
    
    Manufacturer Manufacturer { get; set; }
    List<SoftwareAsset> SoftwareAssets { get; set; }
}
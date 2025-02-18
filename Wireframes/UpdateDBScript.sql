CREATE TABLE LicenseType(
                            LicenseTypeID int PRIMARY KEY IDENTITY (1, 1),
                            ManufacturerID int FOREIGN KEY REFERENCES Manufacturer(ManufacturerID),
                            LicenseTypeName varchar(20) NOT NULL
);
GO;
ALTER TABLE SoftwareAsset
    ADD CONSTRAINT FK_SoftwareAsset_LicenseType
        FOREIGN KEY (LicenseTypeID) REFERENCES LicenseType(LicenseTypeID);
GO;

CREATE TABLE RequestResult(
                              RequestResultID tinyint PRIMARY KEY IDENTITY (1, 1),
                              RequestResultName varchar(20),
);
GO;

CREATE TABLE AssetRequest(
                             AssetRequestID int PRIMARY KEY IDENTITY (1, 1),
                             AssetID bigint FOREIGN KEY REFERENCES Asset(AssetID),
                             EmployeeID int FOREIGN KEY REFERENCES Employee(EmployeeID),
                             DepartmentID tinyint FOREIGN KEY REFERENCES Department(DepartmentID),
                             RequestResultID tinyint FOREIGN KEY REFERENCES RequestResult(RequestResultID),
                             RequestDate datetime2 DEFAULT GETDATE(),
                             RequestNote varchar(200)
);
GO;

CREATE TABLE SoftwareAssetRequest(
                                     SoftwareAssetRequestID int PRIMARY KEY IDENTITY (1, 1),
                                     SoftwareAssetID int FOREIGN KEY REFERENCES SoftwareAsset(SoftwareAssetID),
                                     EmployeeID int FOREIGN KEY REFERENCES Employee(EmployeeID),
                                     AssetID bigint FOREIGN KEY REFERENCES Asset(AssetID),
                                     RequestResultID tinyint FOREIGN KEY REFERENCES RequestResult(RequestResultID),
                                     RequestDate datetime2 DEFAULT GETDATE(),
                                     RequestNote varchar(200)
);
GO;

CREATE TABLE EventSource(
                            EventSourceID tinyint PRIMARY KEY IDENTITY (1, 1),
                            EventSourceName varchar(10)
);
GO;

CREATE TABLE LogEvent(
                         Id int PRIMARY KEY IDENTITY (1, 1),
                         EventSourceID tinyint FOREIGN KEY REFERENCES EventSource(EventSourceID),
                         Message nvarchar(max),
                         MessageTemplate nvarchar(max),
                         Level nvarchar(max),
                         TimeStamp datetime,
                         Exception nvarchar(max),
                         Properties nvarchar(max)
);
CREATE TABLE EmployeePasswords (
                                   PasswordID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
                                   EmployeeID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Employee (EmployeeID),
                                   Password nvarchar(10) NOT NULL
);
CREATE TABLE EmployeePasswords (
                                   PasswordID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
                                   EmployeeID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Employee (EmployeeID),
                                   Password varchar(50) NOT NULL
);
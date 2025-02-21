INSERT INTO AssetStatus (AssetStatusName)
VALUES ('In Use'),
       ('Repair'),
       ('Storage');

INSERT INTO AssetType (AssetTypeName)
VALUES ('Monitor'),
       ('Keyboard'),
       ('Mouse'),
       ('Card Reader'),
       ('Graphic Display'),
       ('Tablet'),
       ('Notebook'),
       ('Stationary Phone'),
       ('Mobile Phone'),
       ('Printer');

INSERT INTO Department (DepartmentName)
VALUES ('IT'),
       ('Administration'),
       ('Human Resources'),
       ('Marketing'),
       ('Concept Art'),
       ('3d Art'),
       ('Logistics');

INSERT INTO EventSource (EventSourceName)
VALUES ('MVC'), ('API');

INSERT INTO Location (LocationName)
VALUES ('Vienna'), ('Linz');

INSERT INTO Manufacturer (ManufacturerName)
VALUES ('Dell'),
       ('Cherry'),
       ('Microsoft'),
       ('Apple'),
       ('Wacom'),
       ('Maxon'),
       ('HP'),
       ('Brother'),
       ('Autodesk'),
       ('SideFX'),
       ('Acer'),
       ('Adobe'),
       ('Next Limit'),
       ('Samsung');

INSERT INTO RequestResult (RequestResultName)
VALUES ('Confirmed'),
       ('Denied'),
       ('Incompatible');

INSERT INTO TicketPriority (TicketPriorityName)
VALUES ('Low'), ('Medium'), ('High'), ('Critical');

INSERT INTO TicketResolution (TicketResolutionName)
VALUES ('Completed'), ('Cancelled'), ('User Error'), ('Other');

INSERT INTO TicketStatus (TicketStatusName)
VALUES ('Open'), ('In Progress'), ('Closed');

INSERT INTO TicketType (TicketTypeName)
VALUES ('Issue'), ('Maintenance'), ('Request'), ('Other');
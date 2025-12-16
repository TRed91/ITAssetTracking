# IT Asset Tracker
Skill Foundry Project

## Overview
The IT Asset Tracking Project is a multi project solution to manage hardware and software assets, assigning them to employees or departments as well as keeping track of IT support tickets and reading various statistics data like asset values, license usage, ticket resolution times and more.

## Context
This project should provide options for managing and tracking assets, support tickets and reading various reports. 
The project should provide an Interface that is easy to navigate and easy understand created with ASP.Net MVC. It also provides a complete API that allows for implementation of an external front end. 
Both interface and API should have proper Security in Place.

## Solution

### Core Layer
Contains interfaces, models and entities that are shared across the solution

### Data Layer
Connects to the Database for adding, retrieving, updating and deleting data.
I choose Entity Framework to query the DB as I expect to write A LOT of queries. A full ORM like EF will speed up creating and updating the repositories for the Data Layer. Also this project represents an application for a possibly large company where other choices like Dapper, as a third party package, could potentially be unreliable.
I made necessary adjustments to the provided ERD to meet the project’s requirements.

Tech: 
- SqlServer Database
- Entity Framework

**A grahical representation of the ERD can be found in the Design Document.**

## Application Layer
Contains testable business logic between the Data Layer and MVC / API Layer.
The application class methods return “result” types: 
```
{ 
  Ok: bool, 
  Data?: <T>, 
  message: string,
  Ex?: Exception
}
```
which are consumed by the API and MVC Layer. 
That way data and errors are being passed to the upper layers where they are handled properly.

### Testing Layer
Contains unit tests for any testable logic of the Application Layer
Dependencies:
- Nunit
- Microsoft Test Sdk

### Model View Controller
Here the interface of the web application will be created using MVC workflow and Razor syntax.
Users need to authenticate via Username and Password to view any of the sites content. 

Users can have one of the following roles assigned to them: 
- Admin: Access to all sites content and functionalities, can add or update users
- Asset Manager: Can add, edit and delete assets, assign assets, generate asset reports.
- Software License Manager: Manages software assets and their assignment, generate software compliance reports
- Help Desk Technician: Manage support tickets. Creating, updating, assigning and adding notes to tickets.
- Department Manager: View assets assigned to their department, request new assets or reassignments, view ticket status for their department’s assets
- Employee: View and create support tickets for their assigned assets.
- Auditor: Read-Only access to all asset and ticket information, generate and view any reports.
- 
For the styling I will leverage bootstrap for responsive design.
**Wireframes and more detailed explanation for the interface can be found in the Design Document.**

#### App Feedback and Error handling:
Error messages, as well as success or informational messages are displayed below the header.
Error messages are displayed with red background and white text while success or informational messages are displayed with green background and white text.
Error messages can be displayed for various reasons, like database connection errors, form invalidity, and database creating/updating/deleting errors.
Every success, information and error message is also being logged to the console.
Upon error the user is redirected to either the previous page, home page or form page where an error message, as described above will be displayed. 

### Rest API

#### CORS policies
If the environment is set to development the AllowAllOrigins policy is used. Which allows all origins as the name suggests.
In production, the app uses the AllowedOrigins policy which reads the allowed origins from the appsettings.json file.

#### Error Handling
Errors are being logged and controllers send an appropriate response of either 401 Unauthorized, 404 Not Found, 500 Internal Server Error etc.

#### API Endponts
**A list of all Endpoints and detailed descriptions can be found in the design document**

## Security
The MVC and API will leverage ASP Identity Entity Framework to manage Accounts and Roles.
The Password requirements are:
- 8 characters minimal length
  - Passwords are generated => ! + Lastname + 4RandomNumbers (!Doe5278)
  - if the user’s last name is less than three characters long the generated password will substitute with additional random numbers at the end.
- at least one upper case character
- at least one lower case character
- at least one digit or other symbol
- 
As described in the MVC users are assigned one of the following roles
- Admin: Access to all sites content and functionalities, can add or update users
- Asset Manager: Can add, edit and delete assets, assign assets, generate asset reports.
- Software License Manager: Manages software assets and their assignment, generate software compliance reports
- Help Desk Technician: Manage support tickets. Creating, updating, assigning and adding notes to tickets.
- Department Manager: View assets assigned to their department, request new assets or reassignments, view ticket status for their department’s assets
- Employee: View and create support tickets for their assigned assets.
- Auditor: Read-Only access to all asset and ticket information, generate and view any reports.
  
Only User with the Admin Role may assign roles to other users.
See MVC Layer and API Endpoints section for role restricted access.

## Logging
The MVC and API, by default, log information level events and above to the console. 

I use Serilog for logging with Console and MSSqlServer Sink
- on Database changes like creating, updating or deleting data. Information Logs  
- for invalid or unauthorized http requests. Warning Logs  
- for caught exceptions. Error Logs
  
The applications write logs to the console and optionally to the database.
Database Logging is configurable in the appsettings.json of each program within the “DbLogging” section:
```
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  },
  "DbLogging": {
    "Enabled": "true",
    "LogLevel": "Warning"
  }
},
```
If “Enabled” is set to “true” the application writes events of the configured log level and above to the database.
 The “LogLevel” can be set to:
- “Information”
- “Debug”
- “Warning”
- “Error”

The MVC and API write Logs to the “LogEvents” Table.

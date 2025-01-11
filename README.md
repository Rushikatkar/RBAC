# Role-Based Access Control (RBAC) System for Web Application

## Overview
This project implements a Role-Based Access Control (RBAC) system in an ASP.NET Core web application. The system is designed to manage user roles, enforce access control for specific sections of the application, log audit actions, and check permissions dynamically using custom middleware. It is built for an internal company portal with distinct modules like **User Management**, **Reports**, and **Employee Dashboard**.

## Features
- **User Role Management**: Admins can create, edit, delete roles, and assign roles to users.
- **Access Control**: Restrict access to controllers and actions based on the user's assigned role.
- **Custom Middleware**: Middleware checks the user's permissions dynamically for each request and logs unauthorized access attempts.
- **Audit Logging**: Logs critical actions such as role assignments and unauthorized access attempts.
- **Database Design**: The system uses a SQL Server database with tables for users, roles, permissions, and an audit log.
- **Unit Tests**: Testing functionality using xUnit.

## Role Definitions & Permissions
### Roles:
- **Admin**: Full access to all modules and actions (User Management, Reports, Dashboard).
- **Manager**: Access to the Reports module and viewing team dashboards; no access to User Management.
- **Employee**: Access only to their personal dashboard; no access to User Management or Reports.

### Role-Specific Permissions:
- **Admin**: Full access to all features.
- **Manager**: Access to Reports and team dashboards.
- **Employee**: Access to personal dashboard only.

## User Scenarios
1. **Admin Scenario**: An Admin logs in, creates a new user, assigns the "Manager" role to the user, and generates an audit log for the action.
2. **Manager Scenario**: A Manager logs in, views their team's performance reports, and attempts to access User Management, resulting in an unauthorized attempt log.
3. **Employee Scenario**: An Employee logs in and can only access their personal dashboard.

## Database Schema
### Users Table:
- `UserId`: Unique identifier for the user.
- `Username`: User’s login name.
- `PasswordHash`: Hashed password for authentication.
- `Email`: User's email address.

### Roles Table:
- `RoleId`: Unique identifier for the role.
- `RoleName`: Name of the role (Admin, Manager, Employee).

### Permissions Table:
- `PermissionId`: Unique identifier for the permission.
- `PermissionName`: Name of the permission (ViewReports, ManageUsers, etc.).

### RolePermissions Table:
- `RoleId`: Links roles to permissions (Many-to-Many relationship).

### AuditLogs Table:
- `LogId`: Unique identifier for the log.
- `UserId`: ID of the user who performed the action.
- `Action`: Action performed (e.g., role assignment).
- `Timestamp`: When the action occurred.
- `Details`: Additional details regarding the action.

## Tech Stack
- **Framework**: ASP.NET Core 7.0
- **Authentication**: ASP.NET Identity Framework with JWT Authentication
- **Database**: SQL Server
- **Logging**: Serilog for audit logging
- **Testing**: xUnit for unit tests

## Middleware
- **Permission Checking Middleware**: A custom middleware dynamically checks user permissions for each request and logs unauthorized access attempts to a database.
- **Logging Unauthorized Access**: Unauthorized access attempts are logged with details including the user ID, IP address, attempted action, and timestamp.

## Database Scripts
SQL scripts for creating the database schema, including tables for Users, Roles, Permissions, RolePermissions, and AuditLogs, are included in the repository.

## Folder Structure
- **Controllers**: Contains all API controllers handling requests and responses.
- **Models**: Contains entity models and view models.
- **Services**: Contains business logic for handling user and role management, and permissions.
- **Data**: Contains the database context and migration files.
- **Middleware**: Contains custom middleware for permission checking.
- **Logs**: Folder where logs are stored (if file-based logging is implemented).

## How to Run the Application Locally

1. Clone the repository:
   ```
   git clone https://your-repository-url.git
   cd your-repository-folder
   ```

2. Ensure that you have **Visual Studio 2022** installed with ASP.NET Core 7.0 support.

3. Restore the NuGet packages:
   ```
   dotnet restore
   ```

4. Update your `appsettings.json` file with the correct database connection string:
   ```json
   "ConnectionStrings": {
     "DBCON": "your-database-connection-string"
   }
   ```

5. Run the application:
   ```
   dotnet run
   ```

6. The API will be available at `https://localhost:5001`.

## Role & Permission Management
- Admins can log into the application and access the **User Management** module to create/edit/delete users, assign roles, and manage permissions.
- Managers can view **Reports** but cannot access User Management.
- Employees can only access their personal dashboard.

## Testing
Unit tests are implemented using xUnit. To run the tests:
1. Open the solution in Visual Studio 2022.
2. Right-click the test project and select **Run Tests**.

## Conclusion
This RBAC system provides a secure and flexible way to manage user access and permissions within an application. The system is extensible, and further roles and permissions can be added based on business requirements.

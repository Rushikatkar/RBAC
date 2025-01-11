use RBAC;

select * from AuditLogs;
select * from Permissions;
select * from RolePermissions;
select * from Roles;
select * from Users;

INSERT INTO Roles ( RoleName) VALUES
( 'Admin'),
( 'Manager'),
( 'Employee');

INSERT INTO Permissions ( PermissionName) VALUES
( 'Read'),
( 'Write'),
( 'Delete'),
( 'Update');

INSERT INTO RolePermissions (RoleId, PermissionId) VALUES
(1, 1), -- Admin can Read
(1, 2), -- Admin can Write
(1, 3), -- Admin can Delete
(1, 4); -- Admin can Update

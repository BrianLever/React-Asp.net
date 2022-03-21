
--=========================================================================== 
--=========================================================================== 
-- MEMBERSHIP
--=========================================================================== 
--------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Users)
BEGIN

DBCC CHECKIDENT (Users, RESEED, 1);

INSERT INTO [dbo].[Users]
(
    [Username]
    ,[Email]
    ,[Comment]
    ,[Password]
    ,[PasswordQuestion]
    ,[PasswordAnswer]	
    ,[IsApproved]
    ,[IsLockedOut]
    ,[FailedPasswordAttemptCount]
    ,[FailedPasswordAttemptWindowStart]
    ,[FailedPasswordAnswerAttemptCount]
    ,[FailedPasswordAnswerAttemptWindowStart]
    ,[CreationDate]
    ,[LastActivityDate]
) VALUES
(
    'su',
    '',
    'Built-in Master Admin Account',
    'KgDHBIRIHSwEZPXulJtM7e1LnJLyH5t3HgXZYgTNG1o=',
    NULL,
    NULL,
    1,
    0,
    5,
    GETDATE(),
    0,
    GETDATE(),
    GETDATE(),
    GETDATE()
),
---------------- INSERT Auto-Export credentials -----------
(
    'export_service',
    '',
    'ScreenDox Auto-Export Service',
    'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
    NULL,
    NULL,
    1,
    0,
    2,
    GETDATE(),
    0,
    GETDATE(),
    GETDATE(),
    GETDATE()
),

----------------- Built In Kiosk User -------------
(
    'kiosk',
    '',
    'Built-in Kiosk User',
    'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
    NULL,
    NULL,
    1,
    0,
    2,
    GETDATE(),
    1,
    GETDATE(),
    GETDATE(),
    GETDATE()
),


---------------- INSERT Tech Support User Credentials -----------

(
    'technical_support',
    '',
    'Tech Support Admin Account',
    'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
    NULL,
    NULL,
    1,
    0,
    2,
    GETDATE(),
    0,
    GETDATE(),
    GETDATE(),
    GETDATE()
),
(
    'global_admin',
    '',
    'System Admin Account',
    'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
    NULL,
    NULL,
    1,
    0,
    2,
    GETDATE(),
    0,
    GETDATE(),
    GETDATE(),
    GETDATE()
);
END
;
---------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Roles)
BEGIN


INSERT INTO [Roles] ([Rolename]) VALUES('Super Administrator');
INSERT INTO [Roles] ([Rolename]) VALUES('Branch Administrator');
INSERT INTO [Roles] ([Rolename]) VALUES('Staff');
INSERT INTO [Roles] ([Rolename]) VALUES('Medical Professionals');
INSERT INTO [Roles] ([Rolename]) VALUES('Lead Medical Professionals');

END
;

----------------------
IF NOT EXISTS (SELECT 1 FROM dbo.UsersInRoles)
BEGIN

INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('su', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('technical_support', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('global_admin', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('export_service', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('kiosk', 'Medical Professionals');

END
;

IF NOT EXISTS (SELECT 1 FROM dbo.UserDetails)
BEGIN

INSERT INTO dbo.UserDetails 
(
    UserID,
    FirstName,    
    LastName,
    MiddleName,
    StateCode,
    City,
    ContactPhone,
    AddressLine1,
    AddressLine2,
    PostalCode	
)
VALUES
(1, 'Built-in', 'Superuser', null, null, null,null,null,null,null),
(2, 'Built-in', 'Auto-Export Service', null, null, null,null,null,null,null),
(3, 'Built-in', 'Kiosk', null, null, null,null,null,null,null),
(4, 'Technical', 'Support', null, null, null,null,null,null,null),
(5, 'Global', 'Administrator', null, null, null,null,null,null,null)
;

END
;
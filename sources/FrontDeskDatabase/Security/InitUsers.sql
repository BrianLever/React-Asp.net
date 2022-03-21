--=========================================================================== 
--=========================================================================== 
-- MEMBERSHIP
--=========================================================================== 
--------------------------------------------
IF EXISTS (SELECT * FROM Roles)
    SET NOEXEC ON
GO


INSERT INTO [Roles] ([Rolename]) VALUES('Super Administrator');
INSERT INTO [Roles] ([Rolename]) VALUES('Branch Administrator');
INSERT INTO [Roles] ([Rolename]) VALUES('Staff');
INSERT INTO [Roles] ([Rolename]) VALUES('Medical Professionals');
INSERT INTO [Roles] ([Rolename]) VALUES('Lead Medical Professionals');

SET NOEXEC OFF
GO



IF EXISTS (SELECT * FROM Users)
    SET NOEXEC ON
GO

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
           ,[LastActivityDate])
     VALUES
           (
            'su',
            'testuser@3si2.com',
            'Built-in Master Admin Account',
            'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
            NULL,
            NULL,
            1,
            0,
            5,
            GETDATE(),
            0,
            GETDATE(),
            GETDATE(),
            GETDATE());

---------------- INSERT Tech Support User Credentials -----------
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
           ,[LastActivityDate])
     VALUES
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
    );

---------------- INSERT Tech Support User Credentials -----------
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
           ,[LastActivityDate])
     VALUES
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
    );


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
           ,[LastActivityDate])
     VALUES
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
    );

INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('su', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('technical_support', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('export_service', 'Super Administrator');


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
(
    1,
    'Built-in',    
    'Superuser',
    null,
    null,
    null,
    null,
    null,
    null,
    null	
);


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
(
    2,
    'Technical',    
    'Support',
    null,
    null,
    null,
    null,
    null,
    null,
    null	
);


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
(
    3,
    'Built-in',    
    'Auto-Export Service',
    null,
    null,
    null,
    null,
    null,
    null,
    null	
);

GO

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
(
    4,
    'Built-in',    
    'Kiosk',
    null,
    null,
    null,
    null,
    null,
    null,
    null	
);


SET NOEXEC OFF
GO

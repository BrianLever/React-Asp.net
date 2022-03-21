--------------------------------------------
DBCC CHECKIDENT (Users, RESEED, 0);



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
            'hOJq3/l/ZVdDyfaQFGbHs2bIJ+Q7a6AZOcQujECcKTc=',
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
            'hOJq3/l/ZVdDyfaQFGbHs2bIJ+Q7a6AZOcQujECcKTc=',
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
            'hOJq3/l/ZVdDyfaQFGbHs2bIJ+Q7a6AZOcQujECcKTc=',
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
            'hOJq3/l/ZVdDyfaQFGbHs2bIJ+Q7a6AZOcQujECcKTc=',
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

GO


INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('su', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('technical_support', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('export_service', 'Super Administrator');

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


GO



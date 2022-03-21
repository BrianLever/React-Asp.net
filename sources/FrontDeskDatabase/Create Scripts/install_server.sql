--- Frontdesk Server installation script - v1.0.0.0

USE [master]
GO
IF DB_ID('FrontDesk') IS NOT NULL
BEGIN
    ALTER DATABASE [FrontDesk] SET OFFLINE WITH ROLLBACK IMMEDIATE
    ALTER DATABASE [FrontDesk] SET ONLINE
    DROP DATABASE [FrontDesk];
END	

GO

CREATE DATABASE [FrontDesk]

GO

------------------------------------
-- CREATE TABLES and TRIGERS
USE [FrontDesk]
GO
IF NOT EXISTS(SELECT name FROM master.dbo.syslogins WHERE name = 'frontdesk_appuser')
BEGIN
    CREATE LOGIN [frontdesk_appuser] WITH PASSWORD = '{PASSWORD}' , CHECK_EXPIRATION = OFF;
END
GO
ALTER LOGIN [frontdesk_appuser] WITH  PASSWORD = '{PASSWORD}';
GO

IF NOT EXISTS(SELECT name FROM sys.database_principals WHERE name = 'frontdesk_appuser')
BEGIN
    CREATE USER [frontdesk_appuser] FOR LOGIN [frontdesk_appuser] WITH DEFAULT_SCHEMA = dbo ;
END
GO 



IF NOT EXISTS(SELECT * FROM sys.objects where type='FN' and name='fn_GetFullName')
BEGIN
DECLARE @FunctionDefinition nvarchar(max);
SET @FunctionDefinition = N'
CREATE FUNCTION [dbo].[fn_GetFullName]
(
    @LastName nvarchar(255), 
    @FirstName nvarchar(255), 
    @MiddleName nvarchar(255)
)
RETURNS nvarchar(max)
WITH SCHEMABINDING
AS
BEGIN
    DECLARE @Result nvarchar(max);
    SET @Result = ISNULL(@FirstName, '''');

    IF LEN(ISNULL(@MiddleName, '''')) > 0
    BEGIN
        IF LEN(@Result) > 0
            SET @Result = @Result + '' '';

        SET @Result = @Result + @MiddleName;
    END

    IF LEN(ISNULL(@LastName, '''')) > 0
    BEGIN
        IF LEN(@Result) > 0
            SET @Result = @Result + '' '';

        SET @Result = @Result + @LastName;
    END

    RETURN @Result;
END
';

exec sp_executesql @FunctionDefinition;

END

GO

IF NOT EXISTS(SELECT * FROM sys.objects where type='FN' and name='fn_GetPatientName')
BEGIN

DECLARE @FunctionDefinition nvarchar(max);
SET @FunctionDefinition = N'
CREATE FUNCTION [dbo].[fn_GetPatientName](
    @LastName nvarchar(255), 
    @FirstName nvarchar(255), 
    @MiddleName nvarchar(255)
)
RETURNS nvarchar(max)
WITH SCHEMABINDING
AS
BEGIN

DECLARE @comma bit = 0; -- where comma was added
DECLARE @Result nvarchar(max);
SET @Result = ISNULL(@LastName, '''');

IF LEN(ISNULL(@FirstName, '''')) > 0
BEGIN
    IF LEN(@Result) > 0
    BEGIN
        SET @Result = @Result + '', '';
        SET @comma = 1;
        
        SET @Result = @Result + @FirstName;
    END
    
END	

IF LEN(ISNULL(@MiddleName, '''')) > 0
BEGIN
    IF LEN(@Result) > 0
    BEGIN
        IF @comma = 1
            SET @Result = @Result + '' '';
        ELSE 
        SET @Result = @Result + '', '';
        
        SET @Result = @Result + @MiddleName;
    END

    
END

RETURN @Result;
END
';
exec sp_executesql @FunctionDefinition;

END
GO


--=========================================================================== 
--=========================================================================== 
-- MEMBERSHIP
--=========================================================================== 
--------------------------------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Users')
BEGIN
CREATE TABLE [dbo].[Users](
    [PKID] int NOT NULL IDENTITY(1, 1),
    [Username] [nvarchar](255) NOT NULL,
    [Email] [nvarchar](255) NULL,
    [Comment] [ntext] NULL,
    [Password] [nvarchar](128) NOT NULL,
    [PasswordQuestion] [nvarchar](255) NULL,
    [PasswordAnswer] [nvarchar](255) NULL,
    [IsApproved] [bit] NULL,
    [LastActivityDate] [datetime] NULL,
    [LastLoginDate] [datetime] NULL,
    [LastPasswordChangedDate] [datetime] NULL,
    [CreationDate] [datetime] NULL,
    [IsOnLine] [bit] NULL,
    [IsLockedOut] [bit] NULL,
    [LastLockedOutDate] [datetime] NULL,
    [FailedPasswordAttemptCount] [int] NULL,
    [FailedPasswordAttemptWindowStart] [datetime] NULL,
    [FailedPasswordAnswerAttemptCount] [int] NULL,
    [FailedPasswordAnswerAttemptWindowStart] [datetime] NULL
    CONSTRAINT [PK__Users] PRIMARY KEY NONCLUSTERED 
    (
        [PKID] ASC
    ),
    CONSTRAINT [UQ__Users] UNIQUE NONCLUSTERED 
    (
        [Username] ASC
    )
);

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

END
;


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

END


GO
---------------------------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Roles')
BEGIN

CREATE TABLE [dbo].[Roles](
    [Rolename] [varchar](255) NOT NULL,
    CONSTRAINT [PKRoles] PRIMARY KEY CLUSTERED 
    (
        [Rolename] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)


INSERT INTO [Roles] ([Rolename]) VALUES('Super Administrator');
INSERT INTO [Roles] ([Rolename]) VALUES('Branch Administrator');
INSERT INTO [Roles] ([Rolename]) VALUES('Staff');
INSERT INTO [Roles] ([Rolename]) VALUES('Medical Professionals');
INSERT INTO [Roles] ([Rolename]) VALUES('Lead Medical Professionals');

END
GO
----------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'UsersInRoles')
BEGIN

CREATE TABLE [dbo].[UsersInRoles](
    [Username] [nvarchar](255) NOT NULL,
    [Rolename] [varchar](255) NOT NULL,
    CONSTRAINT [PKUsersInRoles] PRIMARY KEY CLUSTERED ([Username] ASC,[Rolename] ASC),
    CONSTRAINT [FK_UsersInRoles_Roles] FOREIGN KEY([Rolename]) REFERENCES [dbo].[Roles] ([Rolename])
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT FK_UsersInRoles_Users FOREIGN KEY([Username]) REFERENCES [dbo].[Users] ([Username])
        ON UPDATE CASCADE ON DELETE CASCADE
) ;


INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('su', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('technical_support', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('export_service', 'Super Administrator');

END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'UserDetails')
BEGIN
CREATE TABLE dbo.UserDetails
(
    UserID int NOT NULL,
    FirstName nvarchar(128) NOT NULL,    
    LastName nvarchar(128) NOT NULL,
    MiddleName nvarchar(128) NULL,
    ContactPhone nvarchar(24) NULL,
    StateCode char(2) NULL,
    City nvarchar(128) NULL,
    AddressLine1 nvarchar(128) NULL,
    AddressLine2 nvarchar(128) NULL,	
    PostalCode nvarchar(24) NULL,
    IsBlock bit NOT NULL CONSTRAINT DF_UserDetails_IsBlock Default (0),	
    FullName as CONVERT(nvarchar(255),dbo.fn_GetFullName(LastName, FirstName, MiddleName)),

    CONSTRAINT PK_UserDetails Primary Key CLUSTERED(UserID),
    CONSTRAINT FK_UserDetails_Users FOREIGN KEY (UserID)
    REFERENCES dbo.Users (PKID) ON UPDATE CASCADE ON DELETE	 CASCADE	
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

END



IF NOT EXISTS (SELECT * FROM sys.indexes WHERE  name = 'IX_UserDetails_FullName')
    CREATE INDEX IX_UserDetails_FullName ON dbo.UserDetails(FullName ASC)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'UserPasswordHistory')
CREATE TABLE [dbo].[UserPasswordHistory](
    [PKID] [int] NOT NULL,
    [Password1] [nvarchar](128) NOT NULL,
    [Password2] [nvarchar](128) NULL,
    CONSTRAINT [PK_UserPasswordHistory] PRIMARY KEY CLUSTERED ([PKID] ASC),
    CONSTRAINT [FK_UserPasswordHistory_Users] FOREIGN KEY([PKID])
        REFERENCES [dbo].[Users] ([PKID])
        ON UPDATE CASCADE ON DELETE CASCADE
) 
GO



IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'SecurityQuestion')
BEGIN
    CREATE TABLE [dbo].[SecurityQuestion](
        [QuestionID] [int] IDENTITY(1,1) NOT NULL,
        [QuestionText] [nvarchar](255) NOT NULL,
        CONSTRAINT [PK_SecurityQuestion] PRIMARY KEY CLUSTERED ([QuestionID] ASC)
    );

    
END	
GO
delete	from SecurityQuestion;

insert into SecurityQuestion(QuestionText) values('What was your childhood nickname?')
insert into SecurityQuestion(QuestionText) values('What is the name of your favorite childhood friend?')
insert into SecurityQuestion(QuestionText) values('What was your favorite subject in High school?')
insert into SecurityQuestion(QuestionText) values('What was the name of your first stuffed animal?')
insert into SecurityQuestion(QuestionText) values('Who is your favorite movie hero?')
insert into SecurityQuestion(QuestionText) values('What is your ideal vacation destination?')
insert into SecurityQuestion(QuestionText) values('In what city or town did your mother and father meet?')
insert into SecurityQuestion(QuestionText) values('What is the first name of the boy or girl that you first kissed?')
insert into SecurityQuestion(QuestionText) values('What was the last name of your favorite teacher in school?')
insert into SecurityQuestion(QuestionText) values('What is the name of the neighbors near the house you grew up in?')
GO

 
 


--=========================================================================== 
--===========================================================================
-- System Settings 
--=========================================================================== 
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'SystemSettings')
BEGIN	
CREATE TABLE dbo.SystemSettings
(
    [Key] [nvarchar](128) NOT NULL,
    [Value] [nvarchar](255) NULL,
    [Name] [nvarchar](255) NULL,
    [Description] [nvarchar](255) NULL,
    [RegExp] [nvarchar](255) NULL,
    [IsExposed] [bit] NULL,
    CONSTRAINT PK_SystemSettinge Primary Key CLUSTERED([Key])
);


INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('PasswordRenewalPeriodDays', '120', 'Password renewal period in days', 'Password renewal period in days', NULL, 1);

INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ExportedSecurityReportMaximumLength', '3000', 'Maximum length of the exported security report', 'The Maximum number of security report''s rows that can be exported to excel document', NULL, 1);

INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ActivationSupportEmail', 'skryshtop@3si2.com', 'License Activation Email Address','Email address where activation request code need to be send.', NULL, 0);

INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ActivationRequestEmailTemplate', 'Please activate my FrontDesk Behavioral Health Screener product license.%0A%0AMy activation request code: {0}', 'Activation Request Email Template','Email text for sending product activation request code', NULL, 0);

INSERT INTO SystemSettings([Key], [Value], [Name], [Description], [RegExp], IsExposed)
VALUES('ActivationSupportEmailSubject', 'FrontDesk License Activation', 'Activation Email Subject Text','Activation Email Subject Text.', NULL, 0);

END
GO



IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'DbVersion')
BEGIN
CREATE TABLE DbVersion
(
   DbVersion varchar(32) NOT NULL CONSTRAINT PK_DbVersion PRIMARY KEY CLUSTERED,
   UpdatedOnUTC datetime CONSTRAINT DF_DbVersion_UpdatedOn DEFAULT (GETUTCDATE())
)

END

GRANT SELECT ON DbVersion TO PUBLIC

--=========================================================================== 
--===========================================================================
--  Branch Locations & Kiosks
--=========================================================================== 
    

IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'BranchLocation')
CREATE TABLE dbo.BranchLocation(
    BranchLocationID int NOT NULL IDENTITY(1, 1),
    [Name] nvarchar(128) NOT NULL,
    Description nvarchar(max) NULL,
    Disabled bit NOT NULL CONSTRAINT DF_BranchLocation_Disabled DEFAULT(0)
    CONSTRAINT PK_BranchLocation PRIMARY KEY (BranchLocationID) 
)

GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE  name = 'IX_UserDetails_FullName')
    CREATE INDEX IX_BranchLocation_Name ON dbo.BranchLocation(Name ASC);
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Users_BranchLocation')
CREATE TABLE dbo.Users_BranchLocation(
    UserID int NOT NULL,
    BranchLocationID int NOT NULL
        
    CONSTRAINT FK_UsersBranchLocation_UserID FOREIGN KEY(UserID) 
        REFERENCES dbo.Users(PKID) 
            ON UPDATE CASCADE 
            ON DELETE CASCADE,
    CONSTRAINT FK_UsersBranchLocation_BranchLocationID FOREIGN KEY(BranchLocationID) 
        REFERENCES dbo.BranchLocation(BranchLocationID) 
            ON UPDATE NO ACTION 
            ON DELETE NO ACTION,
    
    CONSTRAINT UQ_UsersBranchLocation_UserID UNIQUE(UserID)	
);
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Kiosk')
CREATE TABLE dbo.Kiosk
(
    KioskID smallint identity(1000,1) NOT NULL,
    KioskName nvarchar(255) NOT NULL,
    Description nvarchar(max) NULL,
    CreatedDate dateTimeoffset NOT NULL,
    LastActivityDate dateTimeoffset NULL,
    BranchLocationID int NOT NULL,
    Disabled bit NOT NULL CONSTRAINT DF_Kiosk_Disabled DEFAULT(0)
    CONSTRAINT PK_Kiosk PRIMARY KEY (KioskID),
    CONSTRAINT FK_Kiosk_BranchLocation FOREIGN KEY (BranchLocationID) 
        REFERENCES BranchLocation(BranchLocationID) ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT [UQ__Kiosk] UNIQUE ([KioskName] ASC)	
);
GO


--===========================================================================
--   States 
--=========================================================================== 

IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'State')
CREATE TABLE dbo.State
(
    StateCode char(2) NOT NULL,
    CountryCode varchar(2) NOT NULL,
    Name nvarchar(128) NOT NULL,
    CONSTRAINT PK_State PRIMARY KEY (StateCode, CountryCode),
    CONSTRAINT UQ_State UNIQUE (StateCode) 
)
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE  name = 'IX_State_Name')
    CREATE INDEX IX_State_Name ON dbo.State(Name);
GO


--===========================================================================
--   Error Log 
--=========================================================================== 
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ErrorLog')
    CREATE TABLE dbo.ErrorLog
    (
       ErrorLogID bigint NOT NULL IDENTITY(1,1) NOT FOR REPLICATION,
       KioskID smallint NULL,
       ErrorMessage nvarchar(max) ,
       ErrorTraceLog nvarchar(max),
       CreatedDate datetimeoffset NOT NULL,
       CONSTRAINT PK_ErrorLog PRIMARY KEY(ErrorLogID)

    )
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE  name = 'IX_ErrorLog_CreatedDate')
    CREATE INDEX IX_ErrorLog_CreatedDate ON dbo.ErrorLog(CreatedDate DESC) INCLUDE(KioskID,ErrorMessage);
GO



--===========================================================================
--   Screening Questions 
--=========================================================================== 


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'AnswerScale')
CREATE TABLE dbo.AnswerScale
(
    AnswerScaleID int NOT NULL,
    Description nvarchar(24) NOT NULL,
    CONSTRAINT PK_AnswerScale PRIMARY KEY(AnswerScaleID)
)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'AnswerScaleOption')
CREATE TABLE dbo.AnswerScaleOption
(
    AnswerScaleOptionID int NOT NULL,
    AnswerScaleID int NOT NULL,
    OptionText nvarchar(24) NOT NULL,
    OptionValue int NOT NULL,
    CONSTRAINT PK_AnswerScaleOption PRIMARY KEY(AnswerScaleOptionID ASC),
    CONSTRAINT FK_AnswerScaleOption__AnswerScaleID FOREIGN KEY(AnswerScaleID)
        REFERENCES dbo.AnswerScale(AnswerScaleID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Screening')
    
CREATE TABLE dbo.Screening
(
    ScreeningID char(4) NOT NULL,
    ScreeningName varchar(28) NOT NULL,
    CONSTRAINT PK_Screening PRIMARY KEY (ScreeningID)
)
GO



IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSection')
CREATE TABLE dbo.ScreeningSection
(
    ScreeningSectionID char(5) NOT NULL,
    ScreeningID char(4) NOT NULL,
    ScreeningSectionName varchar(64) NOT NULL,
    ScreeningSectionShortName varchar(16) NOT NULL,
    QuestionText nvarchar(max) NOT NULL,
    OrderIndex tinyint NOT NULL,
    CONSTRAINT PK_ScreeningSection PRIMARY KEY(ScreeningSectionID ASC),
    CONSTRAINT UQ_ScreeningSection UNIQUE (ScreeningSectionID ASC, ScreeningID ASC),
    CONSTRAINT FK_ScreeningSection__Screening FOREIGN KEY(ScreeningID)
        REFERENCES dbo.Screening(ScreeningID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE  name = 'IX_ScreeningSection_OrderIndex')
    CREATE INDEX IX_ScreeningSection_OrderIndex ON ScreeningSection(OrderIndex ASC, ScreeningID ASC)
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionQuestion')
CREATE TABLE dbo.ScreeningSectionQuestion
( 
    QuestionID int NOT NULL,
    ScreeningSectionID char(5) NOT NULL,
    PreambleText nvarchar(max) NULL,
    QuestionText nvarchar(max) NOT NULL,
    AnswerScaleID int NOT NULL,
    CONSTRAINT PK_ScreeningSectionQuestion PRIMARY KEY (ScreeningSectionID, QuestionID),
    CONSTRAINT FK_ScreeningSectionQuestion__ScreeningSection FOREIGN KEY(ScreeningSectionID)
        REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_ScreeningSectionQuestion__AnswerScaleID FOREIGN KEY(AnswerScaleID)
        REFERENCES dbo.AnswerScale(AnswerScaleID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO


--===========================================================================
--   Screening Result 
--=========================================================================== 



IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningScoreLevel')
CREATE TABLE dbo.ScreeningScoreLevel
( 
    ScreeningSectionID char(5) NOT NULL,
    ScoreLevel int NOT NULL,
    [Name] nvarchar(64) NOT NULL,
    Indicates nvarchar(max) NULL
    CONSTRAINT PK_ScreeningScoreLevel PRIMARY KEY(ScreeningSectionID, ScoreLevel),
    CONSTRAINT FK_ScreeningScoreLevel__ScreeningSection FOREIGN KEY(ScreeningSectionID)
        REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE NO ACTION
);

GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningResult')
CREATE TABLE dbo.ScreeningResult
(
    ScreeningResultID bigint NOT NULL IDENTITY(1,1),
    ScreeningID char(4) NOT NULL,
    FirstName nvarchar(128) NOT NULL,
    LastName nvarchar(128) NOT NULL,
    MiddleName nvarchar(128) NULL,
    Birthday date NOT NULL,
    StreetAddress nvarchar(512) NULL,
    City nvarchar(255)	NULL,
    StateID char(2) NULL,
    ZipCode char(5) NULL,
    Phone char(14) NULL,
    KioskID smallint NOT NULL,
    CreatedDate datetimeoffset NOT NULL,
    IsDeleted bit NOT NULL CONSTRAINT DF_ScreeningResult_IsDeleted DEFAULT(0),
    DeletedDate datetimeoffset NULL,
    DeletedBy int NULL,
    WithErrors bit NOT NULL CONSTRAINT DF_ScreeningResult_WithErrors DEFAULT(0),
    ExportDate DateTimeOffset null,
    ExportedBy int null,
    ExportedToPatientID int null,
    ExportedToHRN nvarchar(255) null,
    ExportedToVisitID int null,
    ExportedToVisitDate datetime null,
    ExportedToVisitLocation nvarchar(255) null,
    CONSTRAINT PK_ScreeningResult PRIMARY KEY(ScreeningResultID),
    CONSTRAINT FK_ScreeningResult__Screening FOREIGN KEY (ScreeningID)
        REFERENCES dbo.Screening(ScreeningID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_ScreeningResult__State FOREIGN KEY(StateID) 
        REFERENCES dbo.State(StateCode) 
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_ScreeningResult__Users FOREIGN KEY(DeletedBy) 
        REFERENCES dbo.Users(PKID) 
        ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT FK_ScreeningResult__Kiosk FOREIGN KEY(KioskID) 
        REFERENCES dbo.Kiosk(KioskID) 
        ON DELETE NO ACTION ON UPDATE CASCADE
)
GO



---- CREATE INDEXES
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE  name = 'IX_ScreeningResult_CreatedDate')
    CREATE INDEX IX_ScreeningResult_CreatedDate ON dbo.ScreeningResult(CreatedDate DESC) INCLUDE(FirstName, LastName, MiddleName, Birthday, ScreeningID)
    WHERE IsDeleted = 0;
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE  name = 'IX_ScreeningResult_IsDeleted')
    CREATE INDEX IX_ScreeningResult_IsDeleted ON dbo.ScreeningResult(IsDeleted) INCLUDE(FirstName, LastName, MiddleName, Birthday, CreatedDate, ScreeningID, KioskID )
    Where IsDeleted = 0
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionResult')
CREATE TABLE dbo.ScreeningSectionResult
(
    ScreeningResultID bigint NOT NULL,
    ScreeningSectionID char(5) NOT NULL,
    AnswerValue int NOT NULL,
    Score int NULL,
    ScoreLevel int NULL,
    CONSTRAINT PK_ScreeningSectionResult PRIMARY KEY(ScreeningResultID, ScreeningSectionID),
    CONSTRAINT FK_ScreeningSectionResult__ScreeningResult FOREIGN KEY(ScreeningResultID)
        REFERENCES dbo.ScreeningResult(ScreeningResultID) ON UPDATE NO ACTION ON DELETE CASCADE,
    CONSTRAINT FK_ScreeningSectionResult__ScreeningScoreLevel FOREIGN KEY(ScreeningSectionID, ScoreLevel)
        REFERENCES dbo.ScreeningScoreLevel(ScreeningSectionID, ScoreLevel) 
        ON UPDATE NO ACTION ON DELETE CASCADE
)
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ScreeningSectionQuestionResult')
CREATE TABLE dbo.ScreeningSectionQuestionResult
(
    ScreeningResultID bigint NOT NULL,
    ScreeningSectionID char(5) NOT NULL,
    QuestionID int NOT NULL,
    AnswerValue int NOT NULL,
    CONSTRAINT PK_ScreeningSectionQuestionResult PRIMARY KEY(ScreeningResultID, ScreeningSectionID, QuestionID),
    CONSTRAINT FK_ScreeningSectionQuestionResult__ScreeningSectionResult FOREIGN KEY(ScreeningResultID, ScreeningSectionID)
        REFERENCES dbo.ScreeningSectionResult(ScreeningResultID, ScreeningSectionID)
        ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_ScreeningSectionQuestionResult__ScreeningSectionQuestion FOREIGN KEY(ScreeningSectionID, QuestionID)
        REFERENCES dbo.ScreeningSectionQuestion(ScreeningSectionID, QuestionID)
        ON DELETE NO ACTION ON UPDATE NO ACTION
    
);
GO

-------------------------------------------------
-- Security Log
-------------------------------------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'SecurityEventCategory')

CREATE TABLE [dbo].[SecurityEventCategory](
    [SecurityEventCategoryID] [int] NOT NULL,
    [CategoryName] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_SecurityEventCategory] PRIMARY KEY CLUSTERED([SecurityEventCategoryID] ASC) 
);

GO
--- Security Event
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'SecurityEvent')

CREATE TABLE [dbo].[SecurityEvent](
    [SecurityEventID] [int] NOT NULL,
    [SecurityEventCategoryID] [int] NOT NULL,
    [Description] [nvarchar](255) NOT NULL,
    [Enabled] [bit] NOT NULL CONSTRAINT DF_SecurityEvent_Enabled DEFAULT(0)
    CONSTRAINT [PK_SecurityLogAction] PRIMARY KEY CLUSTERED ([SecurityEventID] ASC),
    CONSTRAINT [FK_SecurityEvent_SecurityEventCategory] FOREIGN KEY([SecurityEventCategoryID])
        REFERENCES [dbo].[SecurityEventCategory] ([SecurityEventCategoryID])
        ON DELETE NO ACTION ON UPDATE NO ACTION
) 
GO



---- security log
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'SecurityLog')
CREATE TABLE [dbo].[SecurityLog](
    [PKID] [int] NOT NULL,
    [LogDate] [datetimeoffset](7) NOT NULL,
    [SecurityEventID] [int] NOT NULL,
    [Metadata] [sql_variant] NULL,
    [RelatedBranchID] [int] NULL,
    CONSTRAINT [PK_SecurityLog] PRIMARY KEY CLUSTERED ([PKID] ASC,[LogDate] ASC),
    CONSTRAINT [FK_SecurityLog_BranchLocationID] FOREIGN KEY([RelatedBranchID])
        REFERENCES [dbo].[BranchLocation] ([BranchLocationID])
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    CONSTRAINT [FK_SecurityLog_SecurityLogAction] FOREIGN KEY([SecurityEventID]) 
        REFERENCES [dbo].[SecurityEvent] ([SecurityEventID]),
    CONSTRAINT [FK_SecurityLog_Users] FOREIGN KEY([PKID])
        REFERENCES [dbo].[Users] ([PKID])
    
) 
GO


  
            
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'License')
CREATE TABLE dbo.License
(
    LicenseKey varchar(128) NOT NULL,
    ActivationKey varchar(128) NULL,
    ActivationRequest varchar(128) NULL,
    CreatedDate datetimeoffset NOT NULL,
    ActivationRequestDate datetimeoffset NULL,
    ActivatedDate datetimeoffset NULL,
    CONSTRAINT PK_License PRIMARY KEY (LicenseKey)
)
GO



--- Permissions
GRANT EXECUTE ON dbo.[fn_GetFullName] TO frontdesk_appuser;
GRANT EXECUTE ON dbo.[fn_GetPatientName] TO frontdesk_appuser;

GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.Users TO frontdesk_appuser;
GRANT SELECT ON dbo.Roles TO frontdesk_appuser;
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UsersInRoles TO frontdesk_appuser;
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UserDetails TO frontdesk_appuser;
GRANT SELECT, DELETE, UPDATE, INSERT ON dbo.UserPasswordHistory TO frontdesk_appuser;
GRANT SELECT ON dbo.SecurityQuestion TO frontdesk_appuser;


GRANT SELECT,UPDATE ON dbo.SystemSettings TO frontdesk_appuser;


GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.BranchLocation TO frontdesk_appuser
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Kiosk TO frontdesk_appuser
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Users_BranchLocation TO frontdesk_appuser
GRANT SELECT ON dbo.State TO frontdesk_appuser


GRANT SELECT ON dbo.AnswerScale TO frontdesk_appuser
GRANT SELECT ON dbo.AnswerScaleOption TO frontdesk_appuser
GRANT SELECT ON dbo.Screening TO frontdesk_appuser
GRANT SELECT ON dbo.ScreeningSection TO frontdesk_appuser
GRANT SELECT ON dbo.ScreeningSectionQuestion TO frontdesk_appuser
GRANT SELECT, UPDATE, DELETE, INSERT ON dbo.ScreeningSection TO frontdesk_appuser

GRANT SELECT ON dbo.ScreeningScoreLevel TO frontdesk_appuser

GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.ScreeningResult TO frontdesk_appuser
GRANT SELECT, INSERT, DELETE ON dbo.ScreeningSectionResult TO frontdesk_appuser
GRANT SELECT, INSERT, DELETE ON dbo.ScreeningSectionQuestionResult TO frontdesk_appuser


---
GRANT SELECT,INSERT,DELETE,UPDATE ON ErrorLog TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.License TO frontdesk_appuser
GRANT SELECT,INSERT, UPDATE ON dbo.ScreeningSection TO frontdesk_appuser

GRANT SELECT ON dbo.SecurityEventCategory TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SecurityEvent TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SecurityLog TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.SystemSettings TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.UserPasswordHistory TO frontdesk_appuser

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.License TO frontdesk_appuser


GO


-- Initialize Screening questions


-- Run InsertQuestionnareData.sql




IF OBJECT_ID('[dbo].[RpmsCredentials]') IS NULL
CREATE TABLE [dbo].[RpmsCredentials]
(
    [Id] uniqueidentifier NOT NULL ,
    AccessCode NVARCHAR(max),
    VerifyCode NVARCHAR(max),
    ExpireAt datetime NOT NULL,

    CONSTRAINT PK__RpmsCredentials PRIMARY KEY(Id)
);
     
GO 
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.RpmsCredentials TO frontdesk_appuser


IF OBJECT_ID('dbo.fn_GetAge') IS NOT NULL
SET NOEXEC ON
GO

CREATE FUNCTION [dbo].[fn_GetAge]
(
	@Birthday date
)
RETURNS int AS 
BEGIN

DECLARE @CurrentDate DATETIME = GETDATE();


 RETURN CASE WHEN dateadd(year, datediff (year, @Birthday, @CurrentDate), @Birthday) > @CurrentDate
        THEN datediff(year, @Birthday, @CurrentDate) - 1
        ELSE datediff(year, @Birthday, @CurrentDate)
    END
END
GO

SET NOEXEC OFF
GO
GRANT EXECUTE  ON [dbo].[fn_GetAge] TO frontdesk_appuser

-----------
INSERT INTO dbo.DbVersion(DbVersion) VALUES('4.5.0.0');

----
CREATE TABLE [dbo].[VisitSettings]
(
    [MeasureToolId] char(5) NOT NULL, 
	[Name] varchar(64) NOT NULL,
    IsEnabled bit NOT NULL CONSTRAINT DF_VisitSettings_IsEnabled DEFAULT 1,
    OrderIndex tinyint NOT NULL,
	LastModifiedDateUTC datetime NOT NULL,
	CONSTRAINT PK_VisitSettings PRIMARY KEY CLUSTERED(MeasureToolId ASC)
)
GO
CREATE INDEX IX_VisitSettings_OrderIndex ON dbo.VisitSettings(OrderIndex ASC, [MeasureToolId] ASC, [Name] ASC)
GO

IF NOT EXISTS(SELECT NULL FROM [dbo].[VisitSettings])
BEGIN
INSERT INTO dbo.VisitSettings(MeasureToolId, Name, IsEnabled, OrderIndex, LastModifiedDateUTC) VALUES
('SIH', 'Smoker in the Home', 0, 10, SYSDATETIMEOFFSET()),
('TCC1', 'Tobacco Use (Ceremony)', 0, 20, SYSDATETIMEOFFSET()),
('TCC2', 'Tobacco Use (Smoking)', 1, 30, SYSDATETIMEOFFSET()),
('TCC3', 'Tobacco Use (Smokeless)', 1, 40, SYSDATETIMEOFFSET()),
('CAGE', 'Alcohol Use (CAGE)', 1, 50, SYSDATETIMEOFFSET()),
('DAST', 'Non-Medical Drug Use (DAST-10)', 1, 60, SYSDATETIMEOFFSET()),
('PHQ1', 'Depression (PHQ-9)', 1, 70, SYSDATETIMEOFFSET()),
('PHQ2', 'Suicide Identiation (PHQ-9)', 1, 80, SYSDATETIMEOFFSET()),
('HITS', 'Intimate Partner/Domestic Violence (HITS)', 1, 90, SYSDATETIMEOFFSET())
;

END




IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.0.0');




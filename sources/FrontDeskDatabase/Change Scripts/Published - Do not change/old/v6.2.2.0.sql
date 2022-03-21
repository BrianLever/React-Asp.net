DELETE FROM dbo.SystemSettings WHERE [Key] = 'IndicatorReport_AgeGroups';

INSERT INTO dbo.SystemSettings([Key], [Name], [Description], [IsExposed], [RegExp], [Value])
VALUES('IndicatorReport_AgeGroups', 'Age groups for Indicator Reports', 'Age groups for the report in format: 0 - 9;10 - 11;12 - 17;18 - 24;25 - 54;55 or Older', 1, '(\d+\s?-\s?\d+\s?)['',]?|(\d+\s?or\s+older\s?)['',]?', '0 - 9;10 - 11;12 - 17;18 - 24;25 - 54;55 or Older') 


GO

IF EXISTS(SELECT 1
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS  WHERE CONSTRAINT_NAME ='FK_ExportedScreeningResult__ScreeningResult')
   ALTER TABLE [export].[SmartExportLog] DROP CONSTRAINT FK_ExportedScreeningResult__ScreeningResult 
;
GO
ALTER TABLE [export].[SmartExportLog] 
ADD CONSTRAINT FK_ExportedScreeningResult__ScreeningResult FOREIGN KEY (ScreeningResultID)
        REFERENCES dbo.ScreeningResult(ScreeningResultID)
		ON UPDATE NO ACTION ON DELETE CASCADE;
GO

IF OBJECT_ID('[dbo].[fn_IntListToTable]') IS NOT NULL
DROP FUNCTION [dbo].[fn_IntListToTable]
GO

CREATE FUNCTION [dbo].[fn_IntListToTable] (@InputString varchar(4000))
RETURNS  @OutputTable TABLE([value] BIGINT)
AS
BEGIN
    DECLARE @val VARCHAR(10),
    @Delimiter nvarchar(1) = ',';

    WHILE LEN(@InputString) > 0
    BEGIN
        SET @val = LEFT(@inputString, 
            ISNULL(NULLIF(CHARINDEX(@Delimiter, @InputString) - 1, -1),
            LEN(@InputString)))

        SET @InputString = SUBSTRING(@InputString,
                                     ISNULL(NULLIF(CHARINDEX(@Delimiter, @InputString), 0),
                                     LEN(@InputString)) + 1, LEN(@InputString))

        INSERT INTO @OutputTable([value]) VALUES (@val)
    END

    RETURN
END
GO


IF OBJECT_ID('[dbo].[fn_GetMilitaryExperienceNames]') IS NOT NULL
DROP FUNCTION [dbo].[fn_GetMilitaryExperienceNames]
GO

CREATE FUNCTION [dbo].[fn_GetMilitaryExperienceNames]
(
    @idValues varchar(32)
)
RETURNS  varchar(4000)
AS
BEGIN
    DECLARE @str NVARCHAR(MAX)

    DECLARE @Delimiter CHAR(1) = ','

    SELECT @str = COALESCE(@str + @Delimiter,'') + Name 
    FROM dbo.MilitaryExperience WHERE ID IN (
        SELECT [value] FROM dbo.fn_IntListToTable(@IdValues)
    )

    RETURN RTRIM(LTRIM(@str))
END
GO

ALTER TABLE [export].[SmartExportLog] ALTER COLUMN FailedReason nvarchar(128) NULL;



IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE username = 'export_service')
BEGIN
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

    INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('export_service', 'Super Administrator');

    declare @exportIserID BIGINT;

    SET @exportIserID = (SELECT PKID FROM dbo.Users WHERE Username = 'export_service');


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
         @exportIserID,
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

END
;



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.2.2.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.2.2.0');
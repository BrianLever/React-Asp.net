IF OBJECT_ID('[dbo].[uspFindMatchedPatientForExport]') IS NOT NULL
    DROP PROCEDURE [dbo].[uspFindMatchedPatientForExport];
GO

CREATE PROCEDURE [dbo].[uspFindMatchedPatientForExport]
    @LastName nvarchar(128),
    @FirstName nvarchar(128),
    @MiddleName nvarchar(128),
    @Birthday date
AS
BEGIN
    SELECT DISTINCT
        r.LastName,
        r.FirstName,
        r.MiddleName,
        r.Birthday,
        DIFFERENCE(ISNULL(r.MiddleName,''), dbo.ufnMapPatientName(@MiddleName)) AS MiddleNameDiff
    FROM dbo.ScreeningResult r
        INNER JOIN export.SmartExportLog l ON r.ScreeningResultID = l.ScreeningResultID 
            AND l.Succeed = 1 and l.Completed = 1
    WHERE r.Birthday = @Birthday
        AND DIFFERENCE(r.LastName, dbo.ufnMapPatientName(@LastName)) = 4
        AND DIFFERENCE(r.FirstName, dbo.ufnMapPatientName(@FirstName)) = 4
    ORDER BY DIFFERENCE(ISNULL(r.MiddleName,''), dbo.ufnMapPatientName(@MiddleName)) DESC
END
GO

GRANT EXECUTE ON [dbo].[uspFindMatchedPatientForExport]  TO frontdesk_appuser;

GO

IF EXISTS(select * from sys.indexes where name = 'IX__SmartExportLog__Succeed' and object_id = OBJECT_ID('[export].[SmartExportLog]'))
    DROP INDEX IX__SmartExportLog__Succeed ON [export].[SmartExportLog]
GO

CREATE INDEX IX__SmartExportLog__Succeed ON [export].[SmartExportLog](Succeed, Completed, ScreeningResultID)
    WHERE Succeed = 1 AND Completed = 1
GO


IF OBJECT_ID('[export].[PatientNameMap]') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [export].[PatientNameMap]
(
    ID INT NOT NULL IDENTITY(1,1),
    Source nvarchar(128) NOT NULL,
    Destination nvarchar(128) NOT NULL,
    
    CONSTRAINT PK__PatientNameMap PRIMARY KEY CLUSTERED (ID)
);

GO

GRANT SELECT ON [export].[PatientNameMap]  TO frontdesk_appuser;

CREATE INDEX IX__PatientNameMap__Source ON [export].[PatientNameMap](Source);
GO
-- INSERT INTO export.PatientNameMap(Source, Destination) VALUES('TDST', 'TEST');

-- insert test data
INSERT INTO export.PatientNameMap(Source, Destination) VALUES('TDST', 'TEST');

SET NOEXEC OFF
GO

-----

IF OBJECT_ID('[dbo].[ufnMapPatientName]') IS NOT NULL
    DROP FUNCTION [dbo].[ufnMapPatientName]
    ;
GO
;
CREATE FUNCTION [dbo].[ufnMapPatientName]
(
    @Source nvarchar(128)
)
RETURNS nvarchar(128)
AS
BEGIN
    DECLARE @Dest nvarchar(128)
    
    SELECT @Dest = Destination FROM export.PatientNameMap WHERE Source = @Source;

    
    RETURN ISNULL(@Dest, UPPER(@Source))
END
;
GO


------------------------------------------------------------------------------
IF OBJECT_ID('[export].[PatientNameCorrectionLog]') IS NOT NULL
    SET NOEXEC ON
GO

CREATE TABLE [export].[PatientNameCorrectionLog]
(
    [ID] INT NOT NULL IDENTITY(1,1),

    [OriginalPatientName] nvarchar(400) NOT NULL,
    [OriginalBirthday] date NOT NULL,
    [CreatedDate] datetimeoffset NOT NULL,
    [CorrectedPatientName] nvarchar(400) NOT NULL,
    [CorrectedBirthday] date NOT NULL,
    [Comments] nvarchar(max) NOT NULL,

    CONSTRAINT PK__PatientNameCorrectionLog PRIMARY KEY CLUSTERED (ID),
)
GO

CREATE INDEX IX__PatientNameCorrectionLog__CorrectedPatientName 
    ON [export].[PatientNameCorrectionLog]([CorrectedPatientName], [CorrectedBirthday])
GO

CREATE INDEX IX__PatientNameCorrectionLog__CreatedDate 
    ON [export].[PatientNameCorrectionLog](CreatedDate DESC)
GO

SET NOEXEC OFF
GO
---------------------------------
GRANT INSERT,SELECT ON [export].[PatientNameCorrectionLog]  TO frontdesk_appuser;
------

IF OBJECT_ID('[export].[uspAddPatientNameCorrectionLog]') IS NOT NULL
    DROP PROCEDURE [export].[uspAddPatientNameCorrectionLog]
GO

CREATE PROCEDURE [export].[uspAddPatientNameCorrectionLog]
    @OriginalPatientName nvarchar(400),
    @OriginalBirthday date,
    @CreatedDate datetimeoffset,
    @CorrectedPatientName nvarchar(400),
    @CorrectedBirthday date,
    @Comments nvarchar(max)
AS
INSERT INTO [export].[PatientNameCorrectionLog] (
    [OriginalPatientName],
    [OriginalBirthday],
    [CreatedDate],
    [CorrectedPatientName],
    [CorrectedBirthday],
    [Comments]
)
VALUES(
    @OriginalPatientName,
    @OriginalBirthday,
    @CreatedDate,
    @CorrectedPatientName,
    @CorrectedBirthday,
    @Comments
)
GO

GRANT EXECUTE ON [export].[uspAddPatientNameCorrectionLog]  TO frontdesk_appuser;


IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '9.5.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('9.5.0.0');
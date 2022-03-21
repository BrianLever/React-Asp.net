IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE [name] = 'HangFire') EXEC ('CREATE SCHEMA [HangFire]')
GO

IF EXISTS(select 1 from.sys.syslogins where loginname = 'smartexport_appuser')
SET NOEXEC ON
GO

CREATE LOGIN [smartexport_appuser] WITH PASSWORD = 'Q1w2e3r4t5-34614!.' , CHECK_EXPIRATION = OFF;
GO
CREATE USER [smartexport_appuser] FOR LOGIN smartexport_appuser WITH DEFAULT_SCHEMA = [HangFire] ;

GO
SET NOEXEC OFF
GO

ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO [smartexport_appuser]
GO
GRANT CREATE TABLE TO [smartexport_appuser]
GO

--------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE [name] = 'export') EXEC ('CREATE SCHEMA [export]')
GO


IF OBJECT_ID('[export].[SmartExportLog]') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [export].[SmartExportLog]
(
    ScreeningResultID bigint NOT NULL,
    Succeed bit NOT NULL,
    ExportDate DateTimeOffset NOT NULL,
    FailedAttemptCount int NOT NULL,
    FailedReason nvarchar(128) NOT NULL,
    LastError nvarchar(max) NULL,
    Completed bit NOT NULL,

    CONSTRAINT PK_ExportedScreeningResult PRIMARY KEY NONCLUSTERED (ScreeningResultID, Succeed),
	CONSTRAINT FK_ExportedScreeningResult__ScreeningResult FOREIGN KEY (ScreeningResultID)
        REFERENCES dbo.ScreeningResult(ScreeningResultID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
)


SET NOEXEC OFf
GO

IF OBJECT_ID('[dbo].[uspGetScreeningResultsForExport]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetScreeningResultsForExport]
GO
CREATE PROCEDURE [dbo].[uspGetScreeningResultsForExport]
@BatchSize int = 50
AS
    SELECT TOP(@BatchSize) 
        r.ScreeningResultID, 
        r.PatientName,
        r.Birthday, 
        r.CreatedDate
    FROM dbo.ScreeningResult r
        LEFT JOIN export.SmartExportLog el ON r.ScreeningResultID = el.ScreeningResultID AND el.Completed = 1
    WHERE r.IsDeleted = 0 AND r.ExportDate IS NULL AND el.ScreeningResultID IS NULL
    ORDER BY r.CreatedDate DESC

RETURN 0
GO

SET NOEXEC OFf
GO


IF OBJECT_ID('[dbo].[uspGetScreeningResults]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetScreeningResults]
GO

CREATE PROCEDURE [dbo].[uspGetScreeningResults]
@StartDate date,
@BatchSize int
AS
    SELECT TOP (@BatchSize)
        r.ScreeningResultID, 
        r.PatientName,
        r.Birthday
    FROM dbo.ScreeningResult r
    WHERE r.IsDeleted = 0 AND CreatedDate >= @StartDate
    ORDER BY r.CreatedDate ASC

RETURN 0
GO
SET NOEXEC OFf
GO

IF OBJECT_ID('[export].[uspLogExportResult]') IS NOT NULL
DROP PROCEDURE [export].[uspLogExportResult]
GO

CREATE PROCEDURE [export].[uspLogExportResult]
    @ScreeningResultID bigint,
    @Succeed int,
    @ExportDate DateTimeOffset,
    @FailedAttemptCount int,
    @FailedReason nvarchar(128),
    @LastError nvarchar(max),
    @Completed bit
AS
MERGE INTO [export].[SmartExportLog] as target
USING (
    SELECT @ScreeningResultID, @Succeed, @ExportDate, @FailedAttemptCount, @FailedReason, @LastError, @Completed) 
	as source(ScreeningResultID, Succeed, ExportDate, FailedAttemptCount, FailedReason, LastError, Completed)
    ON (target.ScreeningResultID = source.ScreeningResultID)  

WHEN MATCHED THEN UPDATE 
	SET target.Succeed = source.Succeed,
	target.ExportDate = source.ExportDate,
    target.FailedAttemptCount = ISNULL(source.FailedAttemptCount, target.FailedAttemptCount),
    target.FailedReason = ISNULL(source.FailedReason, target.FailedReason),
    target.LastError = ISNULL(source.LastError, target.LastError),
	target.Completed = source.Completed
WHEN NOT MATCHED BY TARGET THEN   
	INSERT ([ScreeningResultID]
           ,[Succeed]
           ,[ExportDate]
           ,[FailedAttemptCount]
           ,[FailedReason]
           ,[LastError]
           ,[Completed])
     VALUES
           (source.ScreeningResultID
           ,source.Succeed 
           ,source.ExportDate
           ,source.FailedAttemptCount
           ,source.FailedReason
           ,source.LastError
           ,source.Completed
           )
;

RETURN 0

go
SET NOEXEC OFf
GO




GRANT EXECUTE, SELECT ON SCHEMA::[HangFire]  TO frontdesk_appuser;
GO
GRANT EXECUTE, SELECT ON SCHEMA::[export]  TO frontdesk_appuser;
GO
GRANT EXECUTE, SELECT ON SCHEMA::[export]  TO [smartexport_appuser];
GO






---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.1.0.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.1.0.1');
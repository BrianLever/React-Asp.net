IF OBJECT_ID('FK_ScreeningSectionResult__ScreeningSection') IS NULL
    ALTER TABLE dbo.ScreeningSectionResult 
        ADD  CONSTRAINT FK_ScreeningSectionResult__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE NO ACTION ON DELETE NO ACTION;
GO


IF OBJECT_ID('[dbo].[ScreeningTimeLog]') IS NOT NULL
    SET NOEXEC ON;
GO


CREATE TABLE [dbo].[ScreeningTimeLog]
(
    ID bigint NOT NULL IDENTITY(1,1),
    ScreeningResultID bigint NOT NULL,
    ScreeningSectionID char(5) NULL,
    StartDate DateTimeOffset NOT NULL,
    EndDate DateTimeOffset NOT NULL,
    DurationInSeconds as CONVERT(int, DATEDIFF(SECOND, StartDate, EndDate))  PERSISTED,
    CONSTRAINT PK_ScreeningTimeLog PRIMARY KEY(ID),
    CONSTRAINT UQ_ScreeningTimeLog UNIQUE (ScreeningResultID ASC, ScreeningSectionID ASC),
    CONSTRAINT FK_ScreeningTimeLog__ScreeningResult FOREIGN KEY(ScreeningResultID)
		REFERENCES dbo.ScreeningResult(ScreeningResultID) ON UPDATE NO ACTION ON DELETE CASCADE,
    CONSTRAINT FK_ScreeningTimeLog__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE NO ACTION ON DELETE NO ACTION
);

GO

SET NOEXEC OFF;
GO


IF OBJECT_ID('[dbo].[vScreeningTimeLogReport]') IS NOT NULL
    DROP VIEW [dbo].[vScreeningTimeLogReport]
GO
;

CREATE VIEW [dbo].[vScreeningTimeLogReport]
AS 
SELECT
s.ScreeningSectionName,
s.ScreeningSectionID,
s.OrderIndex,
tlog.DurationInSeconds,
r.ScreeningResultID,
r.CreatedDate,
r.PatientName,
r.Birthday,
k.BranchLocationID
FROM dbo.ScreeningTimeLog tlog
	INNER JOIN dbo.ScreeningResult r ON tlog.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.Kiosk k ON k.KioskID = r.KioskID
	LEFT JOIN dbo.ScreeningSection s ON tlog.ScreeningSectionID = s.ScreeningSectionID    
;
GO
---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.0.0');
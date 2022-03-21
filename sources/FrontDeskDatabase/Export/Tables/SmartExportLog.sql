CREATE TABLE [export].[SmartExportLog]
(
    ScreeningResultID bigint NOT NULL,
    Succeed bit NOT NULL,
    ExportDate DateTimeOffset NOT NULL,
    FailedAttemptCount int NOT NULL,
    FailedReason nvarchar(256) NULL,
    LastError nvarchar(max) NULL,
    Completed bit NOT NULL,

    CONSTRAINT PK_ExportedScreeningResult PRIMARY KEY NONCLUSTERED (ScreeningResultID, Succeed),
	CONSTRAINT FK_ExportedScreeningResult__ScreeningResult FOREIGN KEY (ScreeningResultID)
        REFERENCES dbo.ScreeningResult(ScreeningResultID)
		ON UPDATE NO ACTION ON DELETE CASCADE,
)
GO

CREATE INDEX IX__SmartExportLog__Succeed ON [export].[SmartExportLog](Succeed, Completed, ScreeningResultID)
    WHERE Succeed = 1 AND Completed = 1
GO
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

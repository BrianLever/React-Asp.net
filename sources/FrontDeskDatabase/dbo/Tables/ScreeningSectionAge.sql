CREATE TABLE dbo.ScreeningSectionAge
(
	ScreeningSectionID char(5) NOT NULL,
	MinimalAge tinyint NOT NULL,
    IsEnabled bit NOT NULL DEFAULT 1,
	LastModifiedDateUTC datetime NOT NULL,
    AgeIsNotConfigurable bit NOT NULL CONSTRAINT  DF__ScreeningSectionAge_AgeIsNotConfigurable DEFAULT(0),
	CONSTRAINT PK_ScreeningSectionAge PRIMARY KEY(ScreeningSectionID ASC),
	CONSTRAINT FK_ScreeningSectionAge__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

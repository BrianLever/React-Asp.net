CREATE TABLE ScreeningSectionAge
(
	ScreeningSectionID nvarchar(5) NOT NULL,
	MinimalAge tinyint NOT NULL,
    IsEnabled bit NOT NULL DEFAULT 1,
	LastModifiedDateUTC datetime NOT NULL,
	CONSTRAINT PK_ScreeningSectionAge PRIMARY KEY(ScreeningSectionID),
	CONSTRAINT FK_ScreeningSectionAge__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE CASCADE
)
GO
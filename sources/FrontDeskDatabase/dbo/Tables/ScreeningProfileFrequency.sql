CREATE TABLE dbo.ScreeningProfileFrequency
(
    ScreeningProfileID INT NOT NULL,
	ScreeningSectionID char(5) NOT NULL,
	Frequency int NOT NULL,
	LastModifiedDateUTC datetime NOT NULL,
	CONSTRAINT PK__ScreeningProfileFrequency PRIMARY KEY(ScreeningProfileID ASC, ScreeningSectionID ASC),
    CONSTRAINT FK___ScreeningProfile FOREIGN KEY(ScreeningProfileID) REFERENCES dbo.ScreeningProfile(ID)
        ON UPDATE NO ACTION ON DELETE CASCADE,
    	CONSTRAINT FK__ScreeningProfileFrequency__ScreeningSection FOREIGN KEY(ScreeningSectionID)
		REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

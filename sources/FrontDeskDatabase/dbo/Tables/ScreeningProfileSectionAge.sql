CREATE TABLE dbo.ScreeningProfileSectionAge
(
    ScreeningProfileID INT NOT NULL,
    ScreeningSectionID char(5) NOT NULL,
    MinimalAge tinyint NOT NULL,
    IsEnabled bit NOT NULL DEFAULT 1,
    LastModifiedDateUTC datetime NOT NULL,
    AgeIsNotConfigurable bit NOT NULL 
        CONSTRAINT  DF__ScreeningProfileSectionAge_AgeIsNotConfigurable DEFAULT(0),

    CONSTRAINT PK__ScreeningProfileSectionAge PRIMARY KEY(ScreeningProfileID ASC, ScreeningSectionID ASC),
    CONSTRAINT FK__ScreeningProfileSectionAge__ScreeningSection FOREIGN KEY(ScreeningSectionID)
        REFERENCES dbo.ScreeningSection(ScreeningSectionID) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

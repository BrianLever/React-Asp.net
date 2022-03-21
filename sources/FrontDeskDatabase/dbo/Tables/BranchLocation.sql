CREATE TABLE dbo.BranchLocation(
	BranchLocationID int NOT NULL IDENTITY(1, 1),
	[Name] nvarchar(128) NOT NULL,
    ScreeningProfileID int NOT NULL,
	Description nvarchar(max) NULL,
	Disabled bit NOT NULL CONSTRAINT DF_BranchLocation_Disabled DEFAULT(0)
	CONSTRAINT PK_BranchLocation PRIMARY KEY (BranchLocationID)
    CONSTRAINT FK__BranchLocation__ScreeningProfile FOREIGN KEY(ScreeningProfileID)  REFERENCES dbo.ScreeningProfile(ID)
)
GO
CREATE INDEX IX_BranchLocation_Name ON dbo.BranchLocation(Name ASC);
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.BranchLocation TO frontdesk_appuser
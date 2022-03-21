CREATE TABLE dbo.ScreeningSection
(
	ScreeningSectionID char(5) NOT NULL,
	ScreeningID char(4) NOT NULL,
	ScreeningSectionName varchar(64) NOT NULL,
	ScreeningSectionShortName varchar(16) NOT NULL,
	QuestionText nvarchar(max) NOT NULL,
	OrderIndex tinyint NOT NULL,
	CONSTRAINT PK_ScreeningSection PRIMARY KEY(ScreeningSectionID ASC),
	CONSTRAINT UQ_ScreeningSection UNIQUE (ScreeningSectionID ASC, ScreeningID ASC),
	CONSTRAINT FK_ScreeningSection__Screening FOREIGN KEY(ScreeningID)
		REFERENCES dbo.Screening(ScreeningID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE INDEX IX_ScreeningSection_OrderIndex ON ScreeningSection(OrderIndex ASC, ScreeningID ASC)
GO
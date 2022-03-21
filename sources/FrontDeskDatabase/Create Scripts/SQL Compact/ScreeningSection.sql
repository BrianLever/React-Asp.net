CREATE TABLE ScreeningSection
(
	ScreeningSectionID nvarchar(5) NOT NULL,
	ScreeningID nvarchar(4) NOT NULL,
	ScreeningSectionName nvarchar(64) NOT NULL,
	QuestionText nvarchar(4000) NOT NULL,
	OrderIndex tinyint NOT NULL,
	CONSTRAINT PK_ScreeningSection PRIMARY KEY(ScreeningSectionID),
	CONSTRAINT UQ_ScreeningSection UNIQUE (ScreeningSectionID, ScreeningID),
	CONSTRAINT FK_ScreeningSection__Screening FOREIGN KEY(ScreeningID)
		REFERENCES Screening(ScreeningID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

CREATE INDEX IX_ScreeningSection_OrderIndex ON ScreeningSection(OrderIndex ASC, ScreeningID ASC)
GO

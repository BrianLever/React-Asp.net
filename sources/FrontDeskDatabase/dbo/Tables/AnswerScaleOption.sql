CREATE TABLE dbo.AnswerScaleOption
(
	AnswerScaleOptionID int NOT NULL,
	AnswerScaleID int NOT NULL,
	OptionText nvarchar(48) NOT NULL,
	OptionValue int NOT NULL,
	CONSTRAINT PK_AnswerScaleOption PRIMARY KEY(AnswerScaleOptionID ASC),
	CONSTRAINT FK_AnswerScaleOption__AnswerScaleID FOREIGN KEY(AnswerScaleID)
		REFERENCES dbo.AnswerScale(AnswerScaleID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
GRANT SELECT ON dbo.AnswerScaleOption TO frontdesk_appuser
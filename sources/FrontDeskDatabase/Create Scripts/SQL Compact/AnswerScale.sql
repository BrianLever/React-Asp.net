
CREATE TABLE AnswerScale
(
	AnswerScaleID int NOT NULL,
	Description nvarchar(24) NOT NULL,
	CONSTRAINT PK_AnswerScale PRIMARY KEY(AnswerScaleID)
)
GO


CREATE TABLE AnswerScaleOption
(
	AnswerScaleOptionID int NOT NULL,
	AnswerScaleID int NOT NULL,
	OptionText nvarchar(24) NOT NULL,
	OptionValue int NOT NULL,
	CONSTRAINT PK_AnswerScaleOption PRIMARY KEY(AnswerScaleOptionID),
	CONSTRAINT FK_AnswerScaleOption__AnswerScaleID FOREIGN KEY(AnswerScaleID)
		REFERENCES AnswerScale(AnswerScaleID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
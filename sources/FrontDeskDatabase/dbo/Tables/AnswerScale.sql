CREATE TABLE dbo.AnswerScale
(
	AnswerScaleID int NOT NULL,
	Description nvarchar(24) NOT NULL,
	CONSTRAINT PK_AnswerScale PRIMARY KEY(AnswerScaleID)
)
GO
GRANT SELECT ON dbo.AnswerScale TO frontdesk_appuser
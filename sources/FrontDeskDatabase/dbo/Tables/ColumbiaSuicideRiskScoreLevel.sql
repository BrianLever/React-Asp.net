CREATE TABLE [dbo].[ColumbiaSuicideRiskScoreLevel]
(
    ScoreLevel int NOT NULL,
    [Name] nvarchar(64) NOT NULL,
    Indicates nvarchar(max) NULL,
    [Label] nvarchar(64) NOT NULL,
    CONSTRAINT PK__ColumbiaSuicideRiskScoreLevel PRIMARY KEY(ScoreLevel)
)
